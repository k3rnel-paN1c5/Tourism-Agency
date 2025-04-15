using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Entities.Enums;
namespace DataAccess.Entities
{
    public partial class Post
    {
        public Post()
        {
            PostTags = new HashSet<PostTag>();
            SEOMetadata = new HashSet<SEOMetadata>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("title", TypeName = "nvarchar(50)")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("body", TypeName = "nvarchar(500)")]
        public string Body { get; set; } = string.Empty;

        [Column("image", TypeName = "nvarchar(100)")]
        public string Image { get; set; } = string.Empty;

        [Required]
        [Column("slug", TypeName = "nvarchar(100)")]
        public string Slug { get; set; } = string.Empty;

        [Column("views")]
        public int Views { get; set; }

        [Column("status")]
        [EnumDataType(typeof(PostStatus))]
        public PostStatus Status { get; set; }

        [Column("summary", TypeName = "nvarchar(200)")]
        public string Summary { get; set; } = string.Empty;

        [Column("publishDate")]
        public DateTime PublishDate { get; set; }

        [Column("postTypeId")]
        [ForeignKey("PostType")]
        public int PostTypeId { get; set; }

        [Required]
        [Column("employeeId")]
        [ForeignKey("Employee")]
        public string EmployeeId { get; set; } = string.Empty;

        // Navigation Properties
        public Employee? Employee { get; set; }
        public PostType? PostType { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
        public ICollection<SEOMetadata> SEOMetadata { get; set; }

    }
}
