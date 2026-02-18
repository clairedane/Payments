using Payments.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payments.Domain.Entities
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public Guid PaymentGuid { get; set; }     
        public string ReferenceID { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

}
