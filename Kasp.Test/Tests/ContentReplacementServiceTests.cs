using Kasp.Test.Classes;
using Xunit;

namespace Kasp.Test.Tests
{
    public class ContentReplacementServiceTests
    {
        private readonly ContentReplacementService _service;

        public ContentReplacementServiceTests()
        {
            _service = new ContentReplacementService();
        }

        #region Test ReplacePasswordPattern Method
        [Fact]
        public void ReplacePasswordPattern_ShouldReplacePasswordPatternCorrectly()
        {
            // Arrange
            var content = "password=%MySecretPassword123%";

            // Act
            var result = _service.ReplacePasswordPattern(content);

            // Assert
            Assert.Equal("password=***PASSWORD***", result);
        }

        [Fact]
        public void ReplacePasswordPattern_ShouldHandleNoPasswordPattern()
        {
            // Arrange
            var content = "There is no password here.";

            // Act
            var result = _service.ReplacePasswordPattern(content);

            // Assert
            Assert.Equal("There is no password here.", result);
        }
        #endregion

        #region Test ReplaceLicenseKeyPattern Method
        [Fact]
        public void ReplaceLicenseKeyPattern_ShouldReplaceLicenseKeyCorrectly()
        {
            // Arrange
            var content = "Here is a license key: ABCDE-12345-FGH67-JK89F";

            // Act
            var result = _service.ReplaceLicenseKeyPattern(content);

            // Assert
            Assert.Equal("Here is a license key: ***LICENCE KEY***", result);
        }

        [Fact]
        public void ReplaceLicenseKeyPattern_ShouldHandleNoLicenseKey()
        {
            // Arrange
            var content = "No license key here.";

            // Act
            var result = _service.ReplaceLicenseKeyPattern(content);

            // Assert
            Assert.Equal("No license key here.", result);
        }
        #endregion

        #region Test ReplaceForbiddenWords Method
        [Fact]
        public void ReplaceForbiddenWords_ShouldReplaceForbiddenWordsCorrectly()
        {
            // Arrange
            var content = "The master system is running smoothly.";

            // Act
            var result = _service.ReplaceForbiddenWords(content);

            // Assert
            Assert.Equal("The primary system is running smoothly.", result);
        }

        [Fact]
        public void ReplaceForbiddenWords_ShouldHandleNoForbiddenWords()
        {
            // Arrange
            var content = "Everything is good.";

            // Act
            var result = _service.ReplaceForbiddenWords(content);

            // Assert
            Assert.Equal("Everything is good.", result);
        }

        [Fact]
        public void ReplaceForbiddenWords_ShouldReplaceMultipleForbiddenWords()
        {
            // Arrange
            var content = "The master and slave processes were updated, but the whitelist is still in use.";

            // Act
            var result = _service.ReplaceForbiddenWords(content);

            // Assert
            Assert.Equal("The primary and secondary processes were updated, but the allowlist is still in use.", result);
        }
        #endregion
    }
}
