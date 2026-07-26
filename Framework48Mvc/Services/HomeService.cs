using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework48Mvc.Services
{
    public class HomeService
    {
        public object GetMessage()
        {
            // 匿名オブジェクトを返す
            return new
            {
                message = "Service에서 생성한 데이터",
                createdAt = DateTime.Now
            };
        }
    }
}