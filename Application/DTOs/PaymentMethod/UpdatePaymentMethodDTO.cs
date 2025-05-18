using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.PaymentMethod
{
   public class UpdatePaymentMethodDTO
    {
        [Required(ErrorMessage = "Payment method ID is required")]
        public int Id { get; set; }
        public string? Method { get; set; }
        public string? Icon { get; set; }
    }
}
