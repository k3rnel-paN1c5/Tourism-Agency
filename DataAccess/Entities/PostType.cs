using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public partial class PostType
    {
        public PostType()
        {
            Posts = new HashSet<Post>();
        }
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("title", TypeName = "nvarchar(50)")]
        public string? Title { get; set; }

        [Column("description", TypeName = "nvarchar(200)")]
        public string? Description { get; set; }

        // Navigation Properties
        public ICollection<Post> Posts { get; set; }



    }
}
