using System.Linq;
using AutoMapper;
using Domain;

namespace Application.Messages
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Message, MessageDto>()
                .ForMember(d => d.SenderUserName, o => o.MapFrom(s => s.Sender.UserName))
                .ForMember(d => d.SenderDisplayName, o => o.MapFrom(s => s.Sender.DisplayName))
                .ForMember(m => m.SenderPhotoUrl, opt => opt
                    .MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(d => d.RecipientUserName, o => o.MapFrom(s => s.Recipient.UserName))
                .ForMember(d => d.RecipientDisplayName, o => o.MapFrom(s => s.Recipient.DisplayName))
                .ForMember(m => m.RecipientPhotoUrl, opt => opt
                    .MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));
        }
    }
}