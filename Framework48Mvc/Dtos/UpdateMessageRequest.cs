using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework48Mvc.Dtos
{
    public class UpdateMessageRequest
    {
        public int Id { get; set; }

        public string Message { get; set; }
    }
}