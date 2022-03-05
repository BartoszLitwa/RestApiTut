using AutoMapper;
using System.Linq;
using TweetBook.Contracts.V1.Responses.Post;
using TweetBook.Contracts.V1.Responses.Tag;
using TweetBook.Domain.Posts;
using TweetBook.Domain.Tags;

namespace TweetBook.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        // Automaticaly finds names to map whatever it can
        public DomainToResponseProfile()
        {
            CreateMap<Post, PostResponse>()
                // Dest - PostResponse
                .ForMember(dest => dest.Tags, opt => 
                    opt.MapFrom(src => src.Tags.Select(t => new TagResponse { Name = t.TagName})));

            CreateMap<Tag, TagResponse>();
        }
    }
}
