using Payments.Domain.Entities;
using Payments.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payments.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByReferenceIDAsync(string referenceID);
        Task<int> InsertAsync(Payment payment);
        Task UpdateStatusAsync(int paymentId, PaymentStatus status);

    }
}
