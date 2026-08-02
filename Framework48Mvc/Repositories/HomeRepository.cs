using Framework48Mvc.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework48Mvc.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly List<MessageResponse> _messages;
        public HomeRepository()
        {
            _messages = new List<MessageResponse>
            {
                new MessageResponse
                {
                    Id = 1,
                    Message = "The first message from the repository",
                    CreatedAt = DateTime.Now
                },
                new MessageResponse
                {
                    Id = 2,
                    Message = "The second message from the repository",
                    CreatedAt = DateTime.Now
                },
                new MessageResponse
                {
                    Id = 3,
                    Message = "The third message from the repository",
                    CreatedAt = DateTime.Now
                }
            };
        }
        public List<MessageResponse> GetMessages()
        {
            return _messages;
        }

        public void AddMessage(CreateMessageRequest request)
        {
            var newMessage = new MessageResponse
            {
                Id = _messages.Count + 1,
                Message = request.Message,
                CreatedAt = DateTime.Now
            };

            _messages.Add(newMessage);
        }

        public void UpdateMessage(UpdateMessageRequest request)
        {
            var message = _messages.FirstOrDefault(m => m.Id == request.Id);
            if (message != null)
            {
                message.Message = request.Message;
            }
        }

        public void DeleteMessage(int id)
        {
            var message = _messages.FirstOrDefault(m => m.Id == id);

            if (message != null)
            {
                _messages.Remove(message);
            }
        }
    }
}