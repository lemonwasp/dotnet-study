using Framework48Mvc.Dtos;
using Framework48Mvc.Repositories;
using System;
using System.Collections.Generic;
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
 
        
        public List<MessageResponse> GetMessages()
        {
            return _messages;
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
    }
}
