using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Cache;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Request.Post;
using TweetBook.Contracts.V1.Responses.Pagination;
using TweetBook.Contracts.V1.Responses.Post;
using TweetBook.Contracts.V1.Responses.Queries;
using TweetBook.Domain.Pagination;
using TweetBook.Domain.Posts;
using TweetBook.Domain.Tags;
using TweetBook.Extensions;
using TweetBook.Helpers;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public PostsController(IPostService postService, IMapper mapper, IUriService uriService)
        {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        [Cached(600)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllPostsQuery query, [FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllPostFilter>(query);

            // Returns only needed number of elements thanks to pagination
            var posts = await _postService.GetPostsAsync(filter, pagination);
            var postResponses = _mapper.Map<List<PostResponse>>(posts);

            // PageNumber starts from 0
            if (pagination == null || pagination.PageSize < PaginationQuery.MinPageSize || pagination.PageNumber < PaginationQuery.StartingPageNumber)
            {
                return Ok(new PagedResponse<PostResponse>(postResponses));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, postResponses);

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        [Cached(600)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            // Return Only one element
            var post = await _postService.GetPostByIdAsync(postId);

            return post == null ? NotFound() : Ok(new Response<PostResponse>(_mapper.Map<PostResponse>(post)));
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {
            var newPostId = Guid.NewGuid();
            var post = new Post
            {
                Id = newPostId,
                Title = postRequest.Title,
                Content = postRequest.Content,
                UserId = HttpContext.GetUserId(),
                Tags = postRequest.Tags.Select(t => new PostTag { PostId = newPostId, TagName = t }).ToList()
            };

            await _postService.CreatePostAsync(post);

            //var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            //var location = $"{baseUrl}/{ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString())}";
            var location = _uriService.GetPostUri(post.Id.ToString());

            return Created(location, new Response<PostResponse>(_mapper.Map<PostResponse>(post)));
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if(!userOwnsPost)
            {
                return BadRequest(new { error = "You do not own this post" });
            }

            var post = await _postService.GetPostByIdAsync(postId);
            post.Title = request.Title;
            post.Content = request.Content;

            return await _postService.UpdatePostAsync(post) ? Ok(new Response<PostResponse>(_mapper.Map<PostResponse>(post))) : NotFound();
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new { error = "You do not own this post" });
            }

            return await _postService.DeletePostAsync(postId) ? NoContent() : NotFound();
        }

        private Response<T> Map<T>(object data)
        {
            return new Response<T>(_mapper.Map<T>(data));
        }
    }
}
