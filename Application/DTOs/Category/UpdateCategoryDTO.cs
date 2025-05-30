﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Category
{
    public class UpdateCategoryDTO
    {
        [Required(ErrorMessage = "Specify which category to edit")]
        public int Id { get; set; }

        [Required(ErrorMessage = "New category name cannot be empty")]
        [StringLength(100)]
        public string? Title { get; set; }
    }
}
