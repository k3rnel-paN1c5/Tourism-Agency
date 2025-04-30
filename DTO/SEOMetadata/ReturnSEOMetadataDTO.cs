using DTO.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.SEOMetadata
{
    public class ReturnSEOMetadataDTO
    {
        public int Id { get; set; }
        public string UrlSlug { get; set; } = string.Empty;
        public string MetaTitle { get; set; } = string.Empty;
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }

        public ReturnPostDTO? Post { get; set; }
    }
}
