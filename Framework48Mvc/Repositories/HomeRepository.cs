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
    }
}