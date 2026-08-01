using Framework48Mvc.Dtos;
using Framework48Mvc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

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
    }
}