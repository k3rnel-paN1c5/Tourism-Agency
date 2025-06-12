using Application.DTOs.Post;
using Application.DTOs.PostType;
using Application.DTOs.Tag;
using Application.IServices.UseCases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourismAgency.Areas.Common.Controllers
{
    [Area("Common")]
    [ApiController]
    [Authorize(Roles = "TripSupervisor,CarSupervisor,Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[area]/[controller]")]
    public class CommonDashboardController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IPostTypeService _postTypeService;
        private readonly ITagService _tagService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommonDashboardController(
            IPostService postService,
            IPostTypeService postTypeService,
            ITagService tagService,
            IHttpContextAccessor httpContextAccessor)
        {
            _postService = postService;
            _postTypeService = postTypeService;
            _tagService = tagService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Welcome()
        {
            Console.WriteLine("CommonController is active!");

            return Ok(new { Message = "Welcome to the Common Dashboard! Manage your posts, types, and tags seamlessly." });

        }

        //* PostType Management *//

        [HttpGet("PostTypes")]
        public async Task<IActionResult> GetPostTypes()
        {
            var result = await _postTypeService.GetAllPostTypeAsync();
            return Ok(result);

        }

        [HttpPost("PostType")]
        public async Task<IActionResult> CreatePostType([FromBody] CreatePostTypeDTO dto)
        {
            var newPostType = await _postTypeService.CreatePostTypeAsync(dto);
            return CreatedAtAction(nameof(GetPostTypes), newPostType);
        }

        //* Post Management *//

        [HttpPost("Post")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO dto)
        {
            var postTypeExists = await _postTypeService.CheckIfPostTypeExistsAsync(dto.PostTypeId);
            if (!postTypeExists)
            {
                return BadRequest(new { Error = "Post Type does not exist. Please create a Post Type first!" });
            }

            var newPost = await _postService.CreatePostAsync(dto);
            return CreatedAtAction(nameof(GetPostById), new { id = newPost.Id }, newPost);
        }

        [HttpGet("Post/{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            return Ok(post);
        }

        [HttpPut("Post/{id}")]
        [Authorize] // Owner only
        public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostDTO dto)
        {
            await _postService.UpdatePostAsync(dto);
            return Ok(dto);
        }

        [HttpDelete("Post/{id}")]
        [Authorize] // Owner only
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.DeletePostAsync(id);
            return Ok(new { Message = "Post deleted successfully." });
        }

        [HttpPost("Post/{id}/Submit")]
        [Authorize] // Owner only
        public async Task<IActionResult> SubmitPost(int id)
        {
            await _postService.SubmitPostAsync(id);
            return Ok(new { Message = "Post submitted for approval." });
        }

        [HttpPut("Post/{id}/Approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApprovePost(int id)
        {
            await _postService.ApprovePostAsync(id);
            return Ok(new { Message = "Post approved and published!" });
        }

        [HttpPut("Post/{id}/Reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectPost(int id)
        {
            await _postService.RejectPostAsync(id);
            return Ok(new { Message = "Post rejected and unpublished." });
        }

        //* Tag Management *//

        [HttpGet("Tags")]
        public async Task<IActionResult> GetTags()
        {
            var result = await _tagService.GetAllTagsAsync();
            return Ok(result);
        }

        [HttpPost("Tag")]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagDTO dto)
        {
            var newTag = await _tagService.CreateTagAsync(dto);
            return CreatedAtAction(nameof(GetTags), newTag);
        }

        [HttpPost("Post/{postId}/Tag/{tagId}")]
        [Authorize] // Owner only
        public async Task<IActionResult> AssignTagToPost(int postId, int tagId)
        {
            await _postService.AssignTagToPostAsync(postId, tagId);
            return Ok(new { Message = "Tag assigned to post successfully." });
        }

        [HttpGet("Post/{postId}/Tags")]
        public async Task<IActionResult> GetTagsByPost(int postId)
        {
            var tags = await _postService.GetTagsByPostIdAsync(postId);
            return Ok(tags);
        }
    }
}
