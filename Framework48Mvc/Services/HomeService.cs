using Framework48Mvc.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Framework48Mvc.Services
{
    public class HomeService
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
        public MessageResponse GetMessage()
        {
            return new MessageResponse
            {
                Message = "Service에서 생성한 데이터",
                CreatedAt = DateTime.Now
            };
        }
    }
}