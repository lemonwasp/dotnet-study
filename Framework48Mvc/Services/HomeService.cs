using Framework48Mvc.Dtos;
using Framework48Mvc.Repositories;
using System.Collections.Generic;
using log4net;
using System;

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
        private static readonly ILog _logger = LogManager.GetLogger(typeof(HomeService));
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
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Message))
            {
                throw new ArgumentException(
                    "Message must not be empty.",
                    nameof(request.Message));
            }

            _homeRepository.AddMessage(request);
            _logger.Info($"Message created.");
        }

        public void UpdateMessage(UpdateMessageRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            
            if (request.Id <= 0)
            {
                throw new ArgumentException(
                    "Id must be greater than zero.",
                    nameof(request.Id));

            }

            if (string.IsNullOrWhiteSpace(
                request.Message))
            {
                throw new ArgumentException(
                    "Message must not be empty.",
                    nameof(request.Message));
            }

            _homeRepository.UpdateMessage(request);
            _logger.Info($"Message updated. Id={request.Id}");
        }

        public void DeleteMessage(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Id must be greater than zero.",
                    nameof(id));
            }

            _homeRepository.DeleteMessage(id);
            _logger.Info($"Message deleted. Id={id}");
        }
    }
}