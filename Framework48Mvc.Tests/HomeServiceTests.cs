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

            //var request = new CreateMessageRequest
            //{
            //    Message = "Unit Test"
            //};
            var request = new UpdateMessageRequest
            {
                Id = 1,
                Message = "Updated message"
            };

            // Act
            //service.AddMessage(request);
            service.UpdateMessage(request);

            // Assert
            var messages = service.GetMessages();

            //Assert.AreEqual(3, messages.Count);
            //Assert.AreEqual("Unit Test", messages[2].Message);
            Assert.AreEqual("Updated message", messages[0].Message);
        }
    }
}
    