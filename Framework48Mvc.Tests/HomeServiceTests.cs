using Framework48Mvc.Services;
using Framework48Mvc.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Framework48Mvc.Tests
{
    [TestClass]
    public class HomeServiceTests
    {
        [TestMethod]
        public void GetMessages_ReturnsTwoMessages()
        {
            // Arrange
            var repository = new FakeHomeRepository();
            var service = new HomeService(repository);

            // Act
            var result = service.GetMessages();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Fake message 1", result[0].Message);
            Assert.AreEqual(1, result[0].Id);
        }
    }
}
