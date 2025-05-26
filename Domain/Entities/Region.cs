using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    public partial class Region
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("name", TypeName = "nvarchar(50)")]
        public string? Name { get; set; }

        //Navigation Properties
        public ICollection<TripPlan>? Plans { get; set; }


    }
}
