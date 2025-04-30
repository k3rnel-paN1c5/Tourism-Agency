using DTO.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Category
{
    public class ReturnCategoryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public List<ReturnCarDTO> Cars { get; set; } = [];
    }
}
