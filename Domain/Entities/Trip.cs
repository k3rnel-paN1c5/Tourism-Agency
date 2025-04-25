using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class Trip
    {

        public Trip()
        {
            Plans = new HashSet<TripPlan>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name", TypeName = "nvarchar(50)")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("slug", TypeName = "nvarchar(100)")]
        public string Slug { get; set; } = string.Empty;

        [Required]
        [Column("isAvailable")]
        public bool IsAvailable { get; set; }

        [Column("description", TypeName = "nvarchar(200)")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column("isPrivate")]
        public bool IsPrivate { get; set; }


        //Navigation Properties
        public ICollection<TripPlan> Plans { get; set; }


    }
}
