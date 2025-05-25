using Application.DTOs.PostType;
using Application.IServices.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace TourismAgency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostTypeController : ControllerBase
    {
        private readonly IPostTypeService _postTypeService;

        public PostTypeController(IPostTypeService postTypeService)
        {
            _postTypeService = postTypeService;
        }

        [HttpPost("CreatePostType")]
        public async Task<IActionResult> CreatePostType([FromBody] CreatePostTypeDTO postTypeDto)
        {
            if (postTypeDto == null)
                return BadRequest(new { message = "Invalid data provided!" });

            var result = await _postTypeService.CreatePostTypeAsync(postTypeDto);

            return CreatedAtAction(nameof(CreatePostType), new
            {
                id = result.Id,
                message = "Post type has been successfully created!"
            }, result);
        }
    }
}

