using Framework48Mvc.Dtos;
using Framework48Mvc.Repositories;
using System.Collections.Generic;

namespace Framework48Mvc.Services
{
    public class HomeService : IHomeService
    {
        //public object GetMessage()
        //{
        //    // 匿名オブジェクトを返す
        //    return new
        //    {
        //        message = "Service에서 생성한 데이터",
        //        createdAt = DateTime.Now
        //    };
        //}
        private readonly IHomeRepository _homeRepository;

        public HomeService(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }

        public List<MessageResponse> GetMessages()
        {
            return _homeRepository.GetMessages();
        }

        public void AddMessage(CreateMessageRequest request)
        {
            _homeRepository.AddMessage(request);
        }

        public void UpdateMessage(UpdateMessageRequest request)
        {
            _homeRepository.UpdateMessage(request);
        }
    }
}