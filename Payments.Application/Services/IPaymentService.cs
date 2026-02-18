using Payments.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payments.Application.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponse?> GetPaymentByReferenceIDAsync(string referenceID);
        Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);
    }

}
