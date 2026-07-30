using Framework48Mvc.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework48Mvc.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        public MessageResponse GetMessage()
        {
            return new MessageResponse
            {
                Message = "data from repository",
                CreatedAt = DateTime.Now
            };
        }
    }
}