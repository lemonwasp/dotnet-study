using Framework48Mvc.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework48Mvc.Services
{
    public interface IHomeService
    {
        List<MessageResponse> GetMessages();

        void AddMessage(CreateMessageRequest request);

        void UpdateMessage(UpdateMessageRequest request);
    }
}