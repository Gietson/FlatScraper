using FlatScraper.Infrastructure.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace FlatScraper.Tests.Services
{
    public class ScrapExtensionsTests
    {

        [Theory]
        [InlineData("100  zł", 100)]
        [InlineData("100.5 zł", 100.5)]
        [InlineData("100,25zł", 100.25)]
        [InlineData("100,252  zł", 100.252)]
        public void convert_string_to_decimal(string value, decimal expectedResult)
        {
            // Arrange
            decimal result;

            // Act
            result = ScrapExtensions.ConvertStringToDecimal(value);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Theory]
        [InlineData(" 50.9", 50.9)]
        [InlineData("c50,3", 50.3)]
        [InlineData("  9", 9)]
        public void convert_string_to_float(string value, float expectedResult)
        {
            // Arrange
            float result;

            // Act
            result = ScrapExtensions.ConvertStringToFloat(value);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Theory]
        [InlineData(" 50.9 s", 51)]
        [InlineData("39,3 v", 39)]
        [InlineData("sd5,5", 6)]
        public void conver_string_to_int(string value, int expectedResult)
        {
            // Arrange
            float result;

            // Act
            result = ScrapExtensions.ConvertStringToInt(value);

            // Assert
            Assert.Equal(result, expectedResult);
        }
    }
}
