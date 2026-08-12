using Framework48Mvc.Dtos;
using Framework48Mvc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace Framework48Mvc.Tests.Fakes
{
    public class FakeHomeRepository : IHomeRepository
    {
        private readonly List<MessageResponse> _messages = 
            new List<MessageResponse>
            {
                new MessageResponse
                {
                    Id = 1,
                    Message = "Fake message 1",
                    CreatedAt = new DateTime(2026, 8, 2)
                },
                new MessageResponse
                {
                    Id = 2,
                    Message = "Fake message 2",
                    CreatedAt = new DateTime(2026, 8, 3)
                }
            };


        public int GetMessageCount()
        {
            return _messages.Count;
        }

        public List<MessageResponse> GetMessages(int skip, int take)
        {
            return _messages
                .OrderByDescending(message => message.Id)
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        public void AddMessage(CreateMessageRequest request)
        {
            _messages.Add(new MessageResponse
            {
                Id = _messages.Count + 1,
                Message = request.Message,
                CreatedAt = new DateTime(2026, 8, 4)
            });
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
