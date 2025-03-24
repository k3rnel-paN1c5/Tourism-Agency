using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public partial class Trip
    {

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name", TypeName = "nvarchar(50)")]
        public string? Name { get; set; }

        [Required]
        [Column("slug", TypeName = "nvarchar(100)")]
        public string? Slug { get; set; }

        [Required]
        [Column("isAvailable")]
        public bool IsAvailabe { get; set; }

        [Column("description", TypeName = "nvarchar(200)")]
        public string? Description { get; set; }

        [Required]
        [Column("isPrivate")]
        public bool IsPrivate { get; set; }


    }
}
