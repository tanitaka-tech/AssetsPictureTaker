using NUnit.Framework;
using TanitakaTech.AssetsPictureTaker.StringConverter;

namespace TanitakaTech.AssetsPictureTaker.Tests
{
    [TestFixture]
    public class RegexStringConverterTests
    {
        [Test]
        public void TestConvertString()
        {
            IStringConverter converter = new RegexStringConverter(
                @"P_GameObject_(\d+)",
                "P_GameObjectPicture_$1"
            );
            
            // Arrange
            string input = "P_GameObject_00123";
            string expectedOutput = "P_GameObjectPicture_00123";

            // Act
            string actualOutput = converter.ConvertString(input);

            // Assert
            Assert.AreEqual(expectedOutput, actualOutput);
        }
    }
}