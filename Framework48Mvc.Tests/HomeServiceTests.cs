using Framework48Mvc.Dtos;
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

            var request = new CreateMessageRequest
            {
                Message = "Unit Test"
            };

            // Act
            service.AddMessage(request);

            // Assert
            var messages = service.GetMessages();

            Assert.AreEqual(3, messages.Count);
            Assert.AreEqual("Unit Test", messages[2].Message);
        }
    }
}
    