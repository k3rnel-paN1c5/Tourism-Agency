using DataAccess.Entities;
using DTO.PostTag;
using DTO.SEOMetadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Post
{
   public class UpdatePostDTO
    {
        [Required(ErrorMessage = "Post ID is required")]
        public int Id { get; set; }
        public string? Title { get; set; }

        public string? Body { get; set; }

        public string? Image { get; set; }

        public string? Slug { get; set; }

        [EnumDataType(typeof(PostStatusEnum))]
        public PostStatusEnum? Status { get; set; }
        public string? Summary { get; set; }

        public DateTime? PublishDate { get; set; }

        public int? PostTypeId { get; set; }
        public string? EmployeeId { get; set; }

        public List<UpdatePostTagDTO>? PostTags { get; set; }
        public List<UpdateSEOMetadataDTO>? SEOMetadata { get; set; }
    }
}
