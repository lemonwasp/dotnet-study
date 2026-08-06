using Framework48Mvc.Dtos;
using Framework48Mvc.Services;
using Framework48Mvc.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Framework48Mvc.Tests
{
    [TestClass]
    public class HomeServiceTests
    {
        [TestMethod]
        public void AddMessage_ThrowsArgumentException_WhenMessageIsEmpty()
        {
            // Arrange
            var repository = new FakeHomeRepository();
            var service = new HomeService(repository);

            //var request = new CreateMessageRequest
            //{
            //    Message = "Unit Test"
            //};
            //var request = new UpdateMessageRequest
            //{
            //    Id = 1,
            //    Message = "Updated message"
            //};
            var request = new CreateMessageRequest
            {
                Message = ""
            };

            // Act
            //service.AddMessage(request);
            //service.UpdateMessage(request);
            //service.DeleteMessage(1);

            // Assert
            //var messages = service.GetMessages();

            //Assert.AreEqual(3, messages.Count);
            //Assert.AreEqual("Unit Test", messages[2].Message);
            //Assert.AreEqual("Updated message", messages[0].Message);
            //Assert.IsFalse(messages.Any(m => m.Id == 1));
            Assert.ThrowsExactly<ArgumentException>(() => service.AddMessage(request));    
        }

        [TestMethod]
        public void UpdateMessage_ThrowsArgumentException_WhenIdIsInvalid()
        {
            var repository = new FakeHomeRepository();
            var service = new HomeService(repository);

            var request = new UpdateMessageRequest
            {
                Id = 0,
                Message = "Updated message"
            };

            Assert.ThrowsExactly<ArgumentException>(
                () => service.UpdateMessage(request));
        }

        [TestMethod]
        public void DeleteMessage_ThrowsArgumentException_WhenIdIsInvalid()
        {
            var repository = new FakeHomeRepository();
            var service = new HomeService(repository);

            Assert.ThrowsExactly<ArgumentException>(
                () => service.DeleteMessage(0));
        }
    }
}
    