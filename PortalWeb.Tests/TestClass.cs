using System;
using PortalWeb;
using PortalWeb.Services;
using Xunit;
using Moq;
namespace PortalWeb.Tests
{
    public sealed class TestClass
    {
        /// <summary>
        /// This method tests action recognizer via mocking using AAA type of TDD
        /// </summary>
        [Fact]
        public void TestActionRecognizer()
        {
            //Arrange
            Mock<IActionRecognizer> recognizerMock = new Mock<IActionRecognizer>();
            recognizerMock.Setup(ent => ent.Recognize(ServiceType.Http.ToString())).Returns("HttpInvoke");
            IActionRecognizer recognizer = recognizerMock.Object;
            //Act
            var intent = recognizer.Recognize(ServiceType.Http.ToString());
            //Assert
            Assert.Equal("HttpInvoke",intent);
        }
    }
}
