using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Framework48Mvc.Dtos;

namespace Framework48Mvc.Repositories
{
    public interface IHomeRepository
    {
        List<MessageResponse> GetMessages();

        void AddMessage(CreateMessageRequest request);
    }
}