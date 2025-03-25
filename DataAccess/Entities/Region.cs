using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
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
        public string? Name { get; set; }

        //Navigation Properties
        public ICollection<TripPlan> Plans { get; set; }


    }
}
