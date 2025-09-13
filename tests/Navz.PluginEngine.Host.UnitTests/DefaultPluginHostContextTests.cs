using Xunit;
using Moq;
using System;
using Navz.PluginEngine.Abstractions;

namespace Navz.PluginEngine.Host.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="DefaultPluginHostContext"/> class.
    /// </summary>
    public class DefaultPluginHostContextTests
    {
        private const string TestHostName = "TestHost";
        private const string TestHostVersion = "1.0.0";

        #region Constructor Tests
        /// <summary>
        /// Verifies that the constructor initializes the context correctly with valid parameters.
        /// </summary>
        [Fact]
        public void Constructor_WithValidParameters_InitializesCorrectly()
        {
            // Arrange & Act
            var context = new DefaultPluginHostContext(TestHostName, TestHostVersion);

            // Assert
            Assert.Equal(TestHostName, context.HostName);
            Assert.Equal(TestHostVersion, context.HostVersion);
        }

        /// <summary>
        /// Verifies that the constructor throws ArgumentNullException for null parameters.
        /// </summary>
        /// <param name="hostName">The host name to test with.</param>
        /// <param name="hostVersion">The host version to test with.</param>
        [Theory]
        [InlineData(null, "1.0.0")]
        [InlineData("TestHost", null)]
        public void Constructor_WithNullParameters_ThrowsArgumentNullException(string? hostName, string? hostVersion)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DefaultPluginHostContext(hostName!, hostVersion!));
        }
        #endregion

        #region Service Registration and Resolution Tests
        /// <summary>
        /// Verifies that a registered service can be resolved correctly.
        /// </summary>
        [Fact]
        public void RegisterService_WithValidService_CanBeResolved()
        {
            // Arrange
            var context = new DefaultPluginHostContext(TestHostName, TestHostVersion);
            var service = new Mock<IPluginBase>().Object;

            // Act
            context.RegisterService<IPluginBase>(service);
            var resolvedService = context.GetService<IPluginBase>();

            // Assert
            Assert.Same(service, resolvedService);
        }

        /// <summary>
        /// Verifies that registering a null service throws ArgumentNullException.
        /// </summary>
        [Fact]
        public void RegisterService_WithNullService_ThrowsArgumentNullException()
        {
            // Arrange
            var context = new DefaultPluginHostContext(TestHostName, TestHostVersion);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => context.RegisterService<IPluginBase>(null!));
        }

        /// <summary>
        /// Verifies that requesting an unregistered service returns null.
        /// </summary>
        [Fact]
        public void GetService_ForUnregisteredService_ReturnsNull()
        {
            // Arrange
            var context = new DefaultPluginHostContext(TestHostName, TestHostVersion);

            // Act
            var service = context.GetService<IPluginBase>();

            // Assert
            Assert.Null(service);
        }

        /// <summary>
        /// Verifies that the non-generic GetService method returns the correct service.
        /// </summary>
        [Fact]
        public void GetService_NonGeneric_ReturnsCorrectService()
        {
            // Arrange
            var context = new DefaultPluginHostContext(TestHostName, TestHostVersion);
            var service = new Mock<IPluginBase>().Object;
            context.RegisterService(service);

            // Act
            var resolvedService = context.GetService(typeof(IPluginBase));

            // Assert
            Assert.Same(service, resolvedService);
        }
        #endregion
    }
}
