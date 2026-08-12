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

        [TestMethod]
        public void GetMessages_ReturnsRequestedPageAndMetadata()
        {
            var repository = new FakeHomeRepository();
            var service = new HomeService(repository);
            var request = new PaginationRequest
            {
                Page = 2,
                PageSize = 1
            };

            var result = service.GetMessages(request);

            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual(1, result.Items[0].Id);
            Assert.AreEqual(2, result.Page);
            Assert.AreEqual(1, result.PageSize);
            Assert.AreEqual(2, result.TotalCount);
            Assert.AreEqual(2, result.TotalPages);
        }

        [TestMethod]
        public void GetMessages_ReturnsEmptyItems_WhenPageIsOutOfRange()
        {
            var repository = new FakeHomeRepository();
            var service = new HomeService(repository);
            var request = new PaginationRequest
            {
                Page = 3,
                PageSize = 1
            };

            var result = service.GetMessages(request);

            Assert.AreEqual(0, result.Items.Count);
            Assert.AreEqual(2, result.TotalCount);
            Assert.AreEqual(2, result.TotalPages);
        }

        [TestMethod]
        public void GetMessages_ThrowsArgumentException_WhenPageIsInvalid()
        {
            var repository = new FakeHomeRepository();
            var service = new HomeService(repository);
            var request = new PaginationRequest
            {
                Page = 0,
                PageSize = 10
            };

            Assert.ThrowsExactly<ArgumentException>(
                () => service.GetMessages(request));
        }

        [TestMethod]
        public void GetMessages_ThrowsArgumentException_WhenPageSizeIsInvalid()
        {
            var repository = new FakeHomeRepository();
            var service = new HomeService(repository);
            var request = new PaginationRequest
            {
                Page = 1,
                PageSize = 101
            };

            Assert.ThrowsExactly<ArgumentException>(
                () => service.GetMessages(request));
        }
    }
}
    