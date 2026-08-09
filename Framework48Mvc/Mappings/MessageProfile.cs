using AutoMapper;
using Framework48Mvc.Dtos;
using Framework48Mvc.Models.Entities;

namespace Framework48Mvc.Mappings
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageResponse>()
                .ForMember(
                    dest => dest.Message,
                    opt => opt.MapFrom(src => src.MessageText));

            CreateMap<CreateMessageRequest, Message>()
                .ForMember(
                    dest => dest.MessageText,
                    opt => opt.MapFrom(src => src.Message));

            CreateMap<UpdateMessageRequest, Message>()
                .ForMember(
                    dest => dest.MessageText,
                    opt => opt.MapFrom(src => src.Message))
                .ForMember(
                    dest => dest.Id,
                    opt => opt.Ignore());
        }
    }
}