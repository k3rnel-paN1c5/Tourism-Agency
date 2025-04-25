using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    public partial class Region
    {
        public Region()
        {
            Plans = new HashSet<TripPlan>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name", TypeName = "nvarchar(50)")]
        public string Name { get; set; } = string.Empty;

        //Navigation Properties
        public ICollection<TripPlan> Plans { get; set; }


    }
}
