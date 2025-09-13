using Navz.PluginEngine.Abstractions;

namespace Navz.PluginEngine.Host
{
    /// <summary>
    /// Lightweight implementation of <see cref="IPluginHostContext"/> optimized for minimal overhead.
    /// Provides basic host information and simple service resolution.
    /// </summary>
    public sealed class DefaultPluginHostContext : IPluginHostContext
    {
        private readonly Dictionary<Type, object> _services = new();
        private readonly Dictionary<string, object> _configuration = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPluginHostContext"/> class.
        /// </summary>
        /// <param name="hostName">The name of the host application.</param>
        /// <param name="hostVersion">The version of the host application.</param>
        public DefaultPluginHostContext(string hostName, string hostVersion)
        {
            HostName = hostName ?? throw new ArgumentNullException(nameof(hostName));
            HostVersion = hostVersion ?? throw new ArgumentNullException(nameof(hostVersion));
        }

        /// <summary>
        /// Gets the name of the host application managing the plugins.
        /// </summary>
        public string HostName { get; }

        /// <summary>
        /// Gets the version of the host application.
        /// </summary>
        public string HostVersion { get; }

        /// <summary>
        /// Registers a service instance with the host context.
        /// </summary>
        /// <typeparam name="T">The service type to register.</typeparam>
        /// <param name="service">The service instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="service"/> is null.</exception>
        public void RegisterService<T>(T service) where T : class
        {
            ArgumentNullException.ThrowIfNull(service);
            _services[typeof(T)] = service;
        }

        /// <summary>
        /// Resolves a service of the specified type from the host environment.
        /// </summary>
        /// <typeparam name="T">The type of the service to resolve.</typeparam>
        /// <returns>The resolved service instance, or null if not found.</returns>
        public T? GetService<T>() where T : class
        {
            return _services.TryGetValue(typeof(T), out var service) ? service as T : null;
        }

        /// <summary>
        /// Resolves a service of the specified type from the host environment (non-generic overload).
        /// </summary>
        /// <param name="serviceType">The type of the service to resolve.</param>
        /// <returns>The resolved service instance, or null if not found.</returns>
        public object? GetService(Type serviceType)
        {
            return _services.TryGetValue(serviceType, out var service) ? service : null;
        }

        /// <summary>
        /// Registers a configuration value for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the configuration value.</typeparam>
        /// <param name="key">The configuration key.</param>
        /// <param name="value">The configuration value.</param>
        public void RegisterConfiguration<T>(string key, T value)
        {
            ArgumentNullException.ThrowIfNull(key);
            if (value != null)
            {
                _configuration[key] = value;
            }
        }

        /// <summary>
        /// Gets a configuration value for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the configuration value.</typeparam>
        /// <param name="key">The configuration key.</param>
        /// <param name="defaultValue">The default value to return if the key is not found.</param>
        /// <returns>The configuration value or the default value if not found.</returns>
        public T GetConfiguration<T>(string key, T defaultValue = default!)
        {
            if (string.IsNullOrEmpty(key))
                return defaultValue;

            if (_configuration.TryGetValue(key, out var value) && value is T typedValue)
                return typedValue;

            // Try environment variables as fallback
            var envValue = Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrEmpty(envValue))
            {
                try
                {
                    return (T)Convert.ChangeType(envValue, typeof(T), System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception ex) when (ex is InvalidCastException || ex is FormatException || ex is OverflowException)
                {
                    // Failed to convert, return default
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Checks if a configuration key exists.
        /// </summary>
        /// <param name="key">The configuration key to check.</param>
        /// <returns>True if the key exists, false otherwise.</returns>
        public bool HasConfiguration(string key)
        {
            return !string.IsNullOrEmpty(key) &&
                   (_configuration.ContainsKey(key) || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)));
        }

        /// <summary>
        /// Logs an error message with optional exception details.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <param name="exception">The optional exception associated with the error.</param>
        public void LogError(string message, Exception? exception = null)
        {
            var logMessage = exception != null
                ? $"[ERROR] {message} | Exception: {exception.Message}"
                : $"[ERROR] {message}";

            Console.Error.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff} {logMessage}");
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        public void LogWarning(string message)
        {
            Console.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff} [WARNING] {message}");
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        public void LogInformation(string message)
        {
            Console.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff} [INFO] {message}");
        }
    }
}
