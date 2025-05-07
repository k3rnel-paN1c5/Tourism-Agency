using Application.DTOs.Post;
using Application.IServices.UseCases.Post;
using Microsoft.AspNetCore.Mvc;

namespace TourismAgency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("CreatePost")] // postman
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO dto)
        {
            if (dto == null)
                return BadRequest("البيانات غير صحيحة!");

            var result = await _postService.CreatePostAsync(dto);
            return CreatedAtAction(nameof(CreatePost), new { id = result.Id }, result);
        }
    }
}
