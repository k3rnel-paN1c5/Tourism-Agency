using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.SEOMetadata
{
   public class UpdateSEOMetadataDTO
    {
        [Required(ErrorMessage = "SEO metadata ID is required")]
        public int Id { get; set; }

        public string? UrlSlug { get; set; }

        public string? MetaTitle { get; set; }

        public string? MetaDescription { get; set; }

        public string? MetaKeywords { get; set; }

        public int? PostId { get; set; }
    }
}

