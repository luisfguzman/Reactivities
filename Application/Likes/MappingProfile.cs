using System.Linq;
using AutoMapper;
using Domain;

namespace Application.Likes
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Like, LikeDto>().ForMember(d => d.UserName, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}