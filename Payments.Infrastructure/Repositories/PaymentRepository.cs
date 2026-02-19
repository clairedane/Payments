using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Payments.Application.Interfaces;
using Payments.Domain.Entities;
using Payments.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Payments.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly string _connectionString;

        public PaymentRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<Payment?>> GetAllAsync()
        {
            using var con = new SqlConnection(_connectionString);

            return await con.QueryAsync<Payment>(
                "Payments_Select",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Payment?> GetByReferenceIDAsync(string referenceID)
        {
            using var con = new SqlConnection(_connectionString);
            return await con.QueryFirstOrDefaultAsync<Payment>(
                "Payments_Select",
                new { ReferenceID = referenceID },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> InsertAsync(Payment payment)
        {
            using var con = new SqlConnection(_connectionString);
            try
            {
                return await con.ExecuteScalarAsync<int>(
                    "Payments_Insert",
                    new
                    {
                        payment.PaymentGuid,
                        payment.ReferenceID,
                        payment.Amount,
                        payment.Currency,
                        Status = payment.Status.ToString()
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                return 0;
            }
        }
        public async Task UpdateStatusAsync(int paymentId, PaymentStatus status)
        {
            using var con = new SqlConnection(_connectionString);

            await con.ExecuteAsync(
                "Payments_UpdateStatus",
                new
                {
                    PaymentID = paymentId,
                    Status = status.ToString()
                },
                commandType: CommandType.StoredProcedure
            );
        }

    }
}
