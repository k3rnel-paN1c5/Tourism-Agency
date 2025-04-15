using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public partial class Employee
    {
        public Employee()
        {
            Bookings = new HashSet<Booking>();
            Posts = new HashSet<Post>();
        }
        [Key, Column("id", TypeName = "nvarchar(450)")]
        public string UserId { get; set; } = string.Empty;

        [Required, Column("hireDate")]
        public DateTime HireDate { get; set; }

        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Post> Posts { get; set; }

    }
}
