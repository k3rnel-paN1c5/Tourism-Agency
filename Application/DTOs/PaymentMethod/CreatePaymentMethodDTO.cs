using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.PaymentMethod
{
   public class CreatePaymentMethodDTO
    {
        [Required(ErrorMessage = "Method name is required")]
        public string Method { get; set; } = string.Empty;
        [Required]
        public string Icon { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
