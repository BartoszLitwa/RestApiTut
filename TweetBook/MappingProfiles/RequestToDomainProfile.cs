using AutoMapper;
using TweetBook.Contracts.V1.Responses.Queries;
using TweetBook.Domain.Pagination;

namespace TweetBook.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
        }
    }
}
