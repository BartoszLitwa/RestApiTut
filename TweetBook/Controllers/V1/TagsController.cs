using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Request.Tag;
using TweetBook.Contracts.V1.Responses.Tag;
using TweetBook.Domain;
using TweetBook.Extensions;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.Poster}")]
    public class TagsController : Controller
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        [Authorize(Policy = PolicyClaims.Tags.PolicyName)]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _tagService.GetAllAsync();
            return Ok(tags);
        }

        [HttpGet(ApiRoutes.Tags.Get)]
        public async Task<IActionResult> Get([FromRoute] string tagName)
        {
            var tag = await _tagService.GetTagByNameAsync(tagName);
            return tag == null ? NotFound() : Ok(tag);
        }

        [HttpPost(ApiRoutes.Tags.Post)]
        public async Task<IActionResult> Post([FromBody] CreateTagRequest tagRequest)
        {
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

            var tagResponse = new TagResponse { TagName = tag.Name };

            return Created(location, tagResponse);
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
