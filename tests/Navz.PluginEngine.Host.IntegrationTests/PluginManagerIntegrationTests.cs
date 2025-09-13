using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Xunit;
using Navz.PluginEngine.Abstractions;
using Navz.PluginEngine.Host.IntegrationTests.TestPlugins;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Runtime.Loader;
using System.Linq;

namespace Navz.PluginEngine.Host.IntegrationTests
{
    /// <summary>
    /// Integration tests for the PluginManager.
    /// </summary>
    public class PluginManagerIntegrationTests : IAsyncLifetime
    {
        private readonly string _testPluginDir;
        private readonly string _mockPluginPath;
        private readonly string _mockAsyncPluginPath;
        private readonly DefaultPluginHostContext _hostContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginManagerIntegrationTests"/> class.
        /// </summary>
        public PluginManagerIntegrationTests()
        {
            _testPluginDir = Path.Combine(Path.GetTempPath(), "PluginTests_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testPluginDir);

            _mockPluginPath = Path.Combine(_testPluginDir, "MockPlugin.dll");
            _mockAsyncPluginPath = Path.Combine(_testPluginDir, "MockAsyncPlugin.dll");
            _hostContext = new DefaultPluginHostContext("TestHost", "1.0.0");
        }

        /// <summary>
        /// Initializes resources for the test class.
        /// </summary>
        public async Task InitializeAsync()
        {
            // Compile and save mock plugins
            await CompileAndSavePluginAsync<MockPlugin>(_mockPluginPath);
            await CompileAndSavePluginAsync<MockAsyncPlugin>(_mockAsyncPluginPath);
        }

        /// <summary>
        /// Disposes resources used by the test class.
        /// </summary>
        public Task DisposeAsync()
        {
            try
            {
                if (Directory.Exists(_testPluginDir))
                {
                    Directory.Delete(_testPluginDir, true);
                }
            }
            catch
            {
                // Best effort cleanup
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Compiles and saves a mock plugin to the specified output path.
        /// </summary>
        /// <typeparam name="T">The plugin type.</typeparam>
        /// <param name="outputPath">The output path for the compiled plugin.</param>
        private async Task CompileAndSavePluginAsync<T>(string outputPath)
        {
            // Determine the source file path based on the type
            string? sourceFile = null;
            if (typeof(T).Name == "MockPlugin")
            {
                sourceFile = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "TestPlugins", "MockPlugin.cs");
            }
            else if (typeof(T).Name == "MockAsyncPlugin")
            {
                sourceFile = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "TestPlugins", "MockAsyncPlugin.cs");
            }
            else
            {
                throw new Exception($"Unknown plugin type: {typeof(T).Name}");
            }

            sourceFile = Path.GetFullPath(sourceFile);
            if (!File.Exists(sourceFile))
                throw new Exception($"Source file not found: {sourceFile}");

            var sourceCode = File.ReadAllText(sourceFile);
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            // Reference the main abstractions DLL explicitly
            // Find the solution root (Navz.PluginEngine.Core) and build the path from there
            var solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
#if TEST_CONFIG_RELEASE
            const string config = "Release";
#elif TEST_CONFIG_DEBUG
            const string config = "Debug";
#else
            const string config = "Release"; // fallback
#endif
            var abstractionsDll = Path.Combine(solutionRoot, "src", "Navz.PluginEngine.Abstractions", "bin", config, "net8.0", "Navz.PluginEngine.Abstractions.dll");
            if (!File.Exists(abstractionsDll))
                throw new Exception($"Abstractions DLL not found: {abstractionsDll}");

            // Add System.Runtime.dll reference for Task and other core types
            var systemRuntimeDll = Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location)!, "System.Runtime.dll");
            if (!File.Exists(systemRuntimeDll))
                throw new Exception($"System.Runtime.dll not found: {systemRuntimeDll}");

            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Task).Assembly.Location),
                MetadataReference.CreateFromFile(systemRuntimeDll),
                MetadataReference.CreateFromFile(abstractionsDll)
            };

            var compilation = CSharpCompilation.Create(
                Path.GetFileNameWithoutExtension(outputPath),
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var errors = string.Join("\n", result.Diagnostics
                    .Where(d => d.Severity == DiagnosticSeverity.Error)
                    .Select(d => d.ToString()));
                throw new Exception($"Failed to compile test plugin. Errors:\n{errors}");
            }

            await File.WriteAllBytesAsync(outputPath, ms.ToArray());
        }

        #region Plugin Loading Tests
        /// <summary>
        /// Tests that plugins are loaded and initialized successfully.
        /// </summary>
        [Fact]
        public async Task LoadPlugins_LoadsAndInitializesPlugins()
        {
            // Arrange
            using var manager = new PluginManager();

            // Act
            await manager.LoadPluginsAsync(_testPluginDir);
            await manager.InitializeAllPluginsAsync(_hostContext);

            // Assert
            Assert.NotEqual(0, manager.Count);
            var pluginsArray = manager.Plugins.ToArray();
            Assert.Contains(pluginsArray, p => p.GetType().Name == "MockPlugin");
            Assert.Contains(pluginsArray, p => p.GetType().Name == "MockAsyncPlugin");
        }
        #endregion

        #region Plugin Lifecycle Tests
        /// <summary>
        /// Tests the complete plugin lifecycle.
        /// </summary>
        [Fact]
        public async Task PluginLifecycle_CompletesSuccessfully()
        {
            // Arrange
            using var manager = new PluginManager();
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act & Assert - Loading
            await manager.LoadPluginsAsync(_testPluginDir);
            Assert.NotEqual(0, manager.Count);

            // Act & Assert - Initialization
            await manager.InitializeAllPluginsAsync(_hostContext, cts.Token);
            Assert.True(true, "Initialization completed successfully");

            // Act & Assert - Stopping
            await manager.StopAllPluginsAsync(cts.Token);
            Assert.True(true, "Stop completed successfully");
        }
        #endregion

        #region Error Handling Tests
        /// <summary>
        /// Tests that loading an invalid plugin is handled gracefully.
        /// </summary>
        [Fact]
        public async Task LoadPlugins_WithInvalidPlugin_HandlesGracefully()
        {
            // Arrange
            using var manager = new PluginManager();
            File.WriteAllBytes(Path.Combine(_testPluginDir, "InvalidPlugin.dll"), new byte[0]);

            // Act
            await manager.LoadPluginsAsync(_testPluginDir);

            // Assert
            var pluginsArray = manager.Plugins.ToArray();
            Assert.DoesNotContain(pluginsArray, p => p.Id == "InvalidPlugin");
        }
        #endregion

        #region Cancellation Tests
        /// <summary>
        /// Tests that plugin initialization handles cancellation gracefully.
        /// </summary>
        [Fact]
        public async Task InitializePlugins_WithCancellation_StopsGracefully()
        {
            // Arrange
            using var manager = new PluginManager();
            await manager.LoadPluginsAsync(_testPluginDir);

            using var cts = new CancellationTokenSource();
            cts.Cancel(); // Cancel immediately

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
                await manager.InitializeAllPluginsAsync(_hostContext, cts.Token));
        }
        #endregion
    }
}
