using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    public partial class SEOMetadata
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("urlSlug", TypeName = "nvarchar(100)")]
        public string UrlSlug { get; set; } = string.Empty;

        [Required]
        [Column("metaTitle", TypeName = "nvarchar(50)")]
        public string MetaTitle { get; set; } = string.Empty;

        [Column("metaDescription", TypeName = "nvarchar(200)")]
        public string MetaDescription { get; set; } = string.Empty;

        [Column("metaKeywords", TypeName = "nvarchar(50)")]
        public string MetaKeywords { get; set; } = string.Empty;

        [Column("postId")]
        [ForeignKey("Post")]
        public int PostId { get; set; }


        // Navigation Properties
        public Post? Post { get; set; }


    }
}
