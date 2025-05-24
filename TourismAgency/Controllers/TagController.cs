using Application.DTOs.Tag;
using Application.IServices.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace TourismAgency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpPost("CreateTag")]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagDTO tagDto)
        {
            if (tagDto == null)
                return BadRequest(new { message = "Invalid data provided!" });

            var result = await _tagService.CreateTagAsync(tagDto);

            return CreatedAtAction(nameof(CreateTag), new
            {
                id = result.Id,
                message = "Tag has been successfully created!"
            }, result);
        }
        [HttpPut("UpdateTag/{id}")]
        public async Task<IActionResult> UpdateTag(int id, [FromBody] UpdateTagDTO tagDto)
        {
            if (tagDto == null || tagDto.Id != id)
                return BadRequest(new { message = "Invalid data provided!" });

            var result = await _tagService.UpdateTagAsync(tagDto);

            return Ok(new
            {
                id = result.Id,
                message = "Tag has been successfully updated!"
            });
        }
         

    }
}

