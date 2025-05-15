using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Category
{
   public class GetCategoryDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        
    }
}
