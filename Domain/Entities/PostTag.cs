using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    public partial class PostTag
    {
        [ForeignKey("Post")]
        public int PostId { get; set; }

        [ForeignKey("Tag")]
        public int TagId { get; set; }

        // Navigation Properties
        public Post? Post { get; set; }
        public Tag? Tag { get; set; }

    }
}
