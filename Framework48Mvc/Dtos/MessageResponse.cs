using System;

namespace Framework48Mvc.Dtos
{
    public class MessageResponse
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}