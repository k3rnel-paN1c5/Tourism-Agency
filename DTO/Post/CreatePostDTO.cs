using DataAccess.Entities.Enums;
using DTO.PostTag;
using DTO.SEOMetadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Post
{
    public class CreatePostDTO
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Body content is required")]
        public string Body { get; set; } = string.Empty;
        public string? Image { get; set; }

        [Required(ErrorMessage = "Slug is required")]
        public string Slug { get; set; } = string.Empty;

        [EnumDataType(typeof(PostStatus))]
        public PostStatus Status { get; set; }

        public string? Summary { get; set; }

        public DateTime? PublishDate { get; set; }

        [Required(ErrorMessage = "Post type ID is required")]
        public int PostTypeId { get; set; }

        [Required(ErrorMessage = "Author ID is required")]
        public string EmployeeId { get; set; } = string.Empty;

        public List<CreatePostTagDTO> PostTags { get; set; } = [];
        public List<CreateSEOMetadataDTO> SEOMetadata { get; set; } = [];
    }
}


