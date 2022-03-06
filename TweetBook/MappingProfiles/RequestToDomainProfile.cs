using AutoMapper;
using TweetBook.Contracts.V1.Responses.Queries;
using TweetBook.Domain.Pagination;
using TweetBook.Domain.Posts;

namespace TweetBook.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
            CreateMap<GetAllPostsQuery, GetAllPostFilter>();
        }
    }
}
