using Payments.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payments.Application.DTOs
{
    public class PaymentResponse
    {
        public Guid PaymentGuid { get; set; }
        public string ReferenceID { get; set; }
        public PaymentStatus Status { get; set; }
        public string Message { get; set; }
    }

}
