using Framework48Mvc.Data;
using Framework48Mvc.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Framework48Mvc.Models.Entities;
using AutoMapper;

namespace Framework48Mvc.Repositories
{
    public class EntityFrameworkHomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EntityFrameworkHomeRepository(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<MessageResponse> GetMessages()
        {
            var messages = _context.Messages
                .OrderByDescending(message => message.Id)
                .ToList();

            return _mapper.Map<List<MessageResponse>>(messages);
        }

        public void AddMessage(CreateMessageRequest request)
        {
            var entity = _mapper.Map<Message>(request);

            _context.Messages.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateMessage(UpdateMessageRequest request)
        {
            var entity = _context.Messages.Find(request.Id);

            if (entity == null)
            {
                throw new KeyNotFoundException(
                    $"Message not found. Id={request.Id}");
            }

            _mapper.Map(request, entity);

            _context.SaveChanges();
        }

        public void DeleteMessage(int id)
        {
            var entity = _context.Messages.Find(id);

            if (entity == null)
            {
                throw new KeyNotFoundException(
                    $"Message not found. Id={id}");
            }

            _context.Messages.Remove(entity);
            _context.SaveChanges();
        }
    }
}