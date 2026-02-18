using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payments.Application.DTOs
{
    public class PaymentRequest
    {
        [Required(ErrorMessage = "Reference ID is required.")]
        [StringLength(50, ErrorMessage = "Reference ID cannot exceed 50 characters.")]
        public required string ReferenceID { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Currency is required.")]
        [StringLength(10, ErrorMessage = "Currency cannot exceed 10 characters.")]
        [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Currency must be a valid 3-letter ISO code (e.g., USD).")]
        public required string Currency { get; set; }
    }

}
