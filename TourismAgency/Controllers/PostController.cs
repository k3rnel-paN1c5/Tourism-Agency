using Application.DTOs.Post;
using Application.IServices.UseCases;
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

        [HttpPost("CreatePost")] // Postman endpoint for creating a post
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO dto)
        {
            // Validate the input DTO
            if (dto == null)
                return BadRequest(new { message = "Invalid data provided!" });

            // Call the service method to create the post
            var result = await _postService.CreatePostAsync(dto);

            // Return a confirmation message along with the created post details
            return CreatedAtAction(nameof(CreatePost), new 
            { 
                id = result.Id, 
                message = "Post has been successfully created!" 
            }, result);
        }

        [HttpPut("UpdatePost/{id}")] // Endpoint to update a post
        public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostDTO dto)
        {
            if (dto == null || dto.Id != id)
                return BadRequest(new { message = "Invalid data provided!" });

        //  Print the order information in the terminal to ensure it is received correctly.
        Console.WriteLine($"Received Update Request: ID={dto.Id}, Title={dto.Title}, Body={dto.Body}, Summary={dto.Summary}, PostTypeId={dto.PostTypeId}");

            var result = await _postService.UpdatePostAsync(dto);

            return Ok(new 
            { 
                id = result.Id, 
                message = "Post has been successfully updated!" 
            });
        }
    }
}
    

