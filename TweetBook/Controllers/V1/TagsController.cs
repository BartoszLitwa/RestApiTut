using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Request.Tag;
using TweetBook.Contracts.V1.Responses.Tag;
using TweetBook.Domain.Tags;
using TweetBook.Extensions;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.Poster}")]
    public class TagsController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public TagsController(ITagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        [Authorize(Policy = PolicyClaims.Tags.PolicyName)]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _tagService.GetAllAsync();
            return Ok(_mapper.Map<List<TagResponse>>(tags));
        }

        [HttpGet(ApiRoutes.Tags.Get)]
        public async Task<IActionResult> Get([FromRoute] string tagName)
        {
            var tag = await _tagService.GetTagByNameAsync(tagName);
            return tag == null ? NotFound() : Ok(_mapper.Map<TagResponse>(tag));
        }

        [HttpPost(ApiRoutes.Tags.Create)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest tagRequest)
        {
            if (!ModelState.IsValid)
            {

            }

            var tag = new Tag
            {
                Name = tagRequest.Name,
                CreatorId = HttpContext.GetUserId(),
                CreatedOn = DateTime.UtcNow
            };

            var created = await _tagService.CreateAsync(tag);

            if (!created)
                return BadRequest("Unable to create tag");

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var location = $"{baseUrl}/{ApiRoutes.Tags.Get.Replace("{tagName}", tag.Name)}";

            return Created(location, _mapper.Map<TagResponse>(tag));
        }

        [HttpDelete(ApiRoutes.Tags.Delete)]
        [Authorize(Policy = PolicyClaims.WorksForCompany.PolicyName)]
        public async Task<IActionResult> Delete([FromRoute] string tagName)
        {
            var tag = await _tagService.DeleteTagAsync(tagName);

            return tag ? NoContent() : NotFound();
        }
    }
}
