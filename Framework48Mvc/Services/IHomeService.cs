using Framework48Mvc.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework48Mvc.Services
{
    public interface IHomeService
    {
        PagedResponse<MessageResponse> GetMessages(PaginationRequest request);

        void AddMessage(CreateMessageRequest request);

        void UpdateMessage(UpdateMessageRequest request);

        void DeleteMessage(int id);
    }
}