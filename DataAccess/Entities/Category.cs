using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public partial class Category
    {
        public Category()
        {
            Cars = new HashSet<Car>();
        }
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("title", TypeName = "nvarchar(50)")]
        public string Title { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<Car> Cars { get; set; }
    }
}
