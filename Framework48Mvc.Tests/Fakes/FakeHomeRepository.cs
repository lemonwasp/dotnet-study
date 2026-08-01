using Framework48Mvc.Repositories;
using System;
using System.Collections.Generic;
using Framework48Mvc.Dtos;

namespace Framework48Mvc.Tests.Fakes
{
    public class FakeHomeRepository : IHomeRepository
    {
      public List<MessageResponse> GetMessages()
        {
            return new List<MessageResponse>
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
        }
    }
}
