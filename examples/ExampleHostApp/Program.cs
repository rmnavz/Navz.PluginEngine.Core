using Navz.PluginEngine.Host;
using Navz.PluginEngine.Abstractions;

namespace ExampleHostApp
{

    internal sealed class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Minimal Navz Plugin Host Example ===");
            var pluginsDir = GetPluginsDirectory();
            if (!Directory.Exists(pluginsDir))
            {
                Console.WriteLine($"Plugins directory not found: {pluginsDir}");
                return;
            }

            using var manager = new PluginManager();
            var hostContext = new DefaultPluginHostContext("MinimalHost", "1.0.0");
            var results = manager.LoadPluginsWithResults(pluginsDir);

            foreach (var result in results)
            {
                if (result.Success)
                    Console.WriteLine($"Loaded: {result.Plugin?.Name} v{result.Plugin?.Version}");
                else
                    Console.WriteLine($"Failed: {System.IO.Path.GetFileName(result.AssemblyPath)} - {result.ErrorMessage}");
            }

            if (manager.Count > 0)
            {
                await manager.InitializeAllPluginsAsync(hostContext);
                Console.WriteLine($"Initialized {manager.Count} plugin(s).");
            }
            else
            {
                Console.WriteLine("No plugins loaded.");
            }
        }

        private static string GetPluginsDirectory()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(baseDir, "plugins");
        }
    }
}
