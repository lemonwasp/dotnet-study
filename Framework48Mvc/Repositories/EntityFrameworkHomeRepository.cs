using Framework48Mvc.Data;
using Framework48Mvc.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Framework48Mvc.Models.Entities; 

namespace Framework48Mvc.Repositories
{
    public class EntityFrameworkHomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;

        public EntityFrameworkHomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<MessageResponse> GetMessages()
        {
            return _context.Messages
                .OrderByDescending(message => message.Id)
                .Select(message => new MessageResponse
                {
                    Id = message.Id,
                    Message = message.MessageText
                })
                .ToList();
        }

        public void AddMessage(CreateMessageRequest request)
        {
            var entity = new Message
            {
                MessageText = request.Message
            };

            _context.Messages.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateMessage(UpdateMessageRequest request)
        {
            var entity = _context.Messages.Find(request.Id);

            if (entity == null)
            {
                throw new KeyNotFoundException($"Message not found. Id={request.Id}");
            }

            entity.MessageText = request.Message;
            _context.SaveChanges();
        }

        public void DeleteMessage(int id)
        {
            var entity = _context.Messages.Find(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Message not found. Id={id}");
            }
            _context.Messages.Remove(entity);
            _context.SaveChanges();
        }
    }
}