using DTO.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Car
{
   public class ReturnCarDTO
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public int Seats { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public decimal Pph { get; set; } // Price per hour
        public decimal Ppd { get; set; } // Price per day
        public decimal Mbw { get; set; }// Maximum baggage weight
        public ReturnCategoryDTO Category { get; set; }
    }
}
