using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.SEOMetadata
{
   public class CreateSEOMetadataDTO
    {
        [Required(ErrorMessage = "URL slug is required")]
        public string UrlSlug { get; set; } = string.Empty;

        [Required(ErrorMessage = "Meta title is required")]
        public string MetaTitle { get; set; } = string.Empty;

        public string? MetaDescription { get; set; }

        public string? MetaKeywords { get; set; }

        [Required(ErrorMessage = "Post ID is required")]
        public int PostId { get; set; }
    }

}
