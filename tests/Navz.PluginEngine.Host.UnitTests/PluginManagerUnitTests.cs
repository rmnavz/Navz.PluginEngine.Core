using System;
using Xunit;
using Moq;
using Navz.PluginEngine.Host;

namespace Navz.PluginEngine.Host.UnitTests
{
    /// <summary>
    /// Organized and maintainable unit tests for PluginManager.
    /// </summary>
    /// <summary>
    /// Contains unit tests for <see cref="PluginManager"/> covering construction, plugin loading, and plugin list management.
    /// Organized for maintainability, reusability, and clarity.
    /// </summary>
    public class PluginManagerUnitTests
    {
        #region Test Helpers
        /// <summary>
        /// Creates a new instance of <see cref="PluginManager"/> for test purposes.
        /// </summary>
        /// <returns>A new <see cref="PluginManager"/> instance.</returns>
        private static PluginManager CreateManager() => new PluginManager();
        #endregion

        #region Constructor
        /// <summary>
        /// Verifies that the constructor initializes the manager and its plugin list correctly.
        /// </summary>
        [Fact]
        public void Constructor_InitializesCorrectly()
        {
            var manager = CreateManager();
            Assert.NotNull(manager);
            Assert.Equal(0, manager.Count);
        }
        #endregion

        #region Plugin Loading
        /// <summary>
        /// Verifies that loading plugins from a non-existent directory throws DirectoryNotFoundException.
        /// </summary>
        [Fact]
        public void LoadPlugins_ThrowsOnInvalidDirectory()
        {
            var manager = CreateManager();
            Assert.Throws<System.IO.DirectoryNotFoundException>(() => manager.LoadPlugins("non_existent_dir"));
        }

        /// <summary>
        /// Verifies that loading plugins from an empty directory does not throw and results in an empty plugin list.
        /// </summary>
        [Fact]
        public void LoadPlugins_EmptyDirectory_DoesNotThrow()
        {
            var tempDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
            System.IO.Directory.CreateDirectory(tempDir);
            var manager = CreateManager();
            try
            {
                manager.LoadPlugins(tempDir);
                Assert.Equal(0, manager.Count);
            }
            finally
            {
                System.IO.Directory.Delete(tempDir);
            }
        }
        #endregion

        #region Plugin List Management
        /// <summary>
        /// Verifies that the plugin list is empty on initialization.
        /// </summary>
        [Fact]
        public void PluginsList_IsEmptyOnInit()
        {
            var manager = CreateManager();
            Assert.Empty(manager.PluginsList);
        }
        #endregion

        // Future: Add tests for plugin unloading, error handling, and edge cases as functionality expands
    }
}
