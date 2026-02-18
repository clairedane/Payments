using Payments.Application.DTOs;
using Payments.Application.Interfaces;
using Payments.Domain.Entities;
using Payments.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payments.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;

        public PaymentService(IPaymentRepository repository)
        {
            _repository = repository;
        }
        public async Task<PaymentResponse?> GetPaymentByReferenceIDAsync(string referenceID)
        {
            var payment = await _repository.GetByReferenceIDAsync(referenceID);
            if (payment == null) return null;

            return new PaymentResponse
            {
                PaymentGuid = payment.PaymentGuid,
                ReferenceID = payment.ReferenceID,
                Status = payment.Status,
                Message = "Payment retrieved successfully"
            };
        }


        public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
        {
            var existing = await _repository.GetByReferenceIDAsync(request.ReferenceID);
            if (existing != null)
            {
                return MapToResponse(existing, "Duplicate payment detected");
            }

            var payment = new Payment
            {
                PaymentGuid = Guid.NewGuid(),
                ReferenceID = request.ReferenceID,
                Amount = request.Amount,
                Currency = request.Currency,
                Status = PaymentStatus.Pending,
            };

            int newId = await _repository.InsertAsync(payment);

            if (newId == 0)
            {
                var duplicate = await _repository.GetByReferenceIDAsync(request.ReferenceID);
                return MapToResponse(duplicate!, "Duplicate payment detected");
            }

            payment.PaymentID = newId;

            // Simulate processing
            payment.Status = PaymentStatus.Success;
            await _repository.UpdateStatusAsync(payment.PaymentID, payment.Status);

            return MapToResponse(payment, "Payment processed successfully");
        }

        private PaymentResponse MapToResponse(Payment payment, string message)
        {
            return new PaymentResponse
            {
                PaymentGuid = payment.PaymentGuid,
                ReferenceID = payment.ReferenceID,
                Status = payment.Status,
                Message = message
            };
        }
    }
}
