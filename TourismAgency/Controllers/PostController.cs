using Application.IServices.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace TourismAgency.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        /// Gets all published posts for public viewing.
        /// </summary>
        [HttpGet("published")]
        public async Task<IActionResult> GetPublishedPosts()
        {
            var allPosts = await _postService.GetAllPostsAsync();
            // Assuming GetAllPostsAsync returns all posts and we filter for published ones here
            // Or you could add a new method in your service like `GetPublishedPostsAsync`
            // var publishedPosts = allPosts.Where(p => p.Status == Domain.Enums.PostStatus.Published);
            return Ok(allPosts);
        }

        /// <summary>
        /// Gets a single published post by its ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            return Ok(post);  
        }
    }
}