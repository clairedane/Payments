# Payments API

## Overview
This service provides internal payment processing for trusted integrations. It ensures secure, idempotent handling of payment requests, with JWT authentication enforced for all endpoints. Payment processing is simulated; no real payment gateway integration is included.

## Assumptions
- `ReferenceID` is used as the idempotency key to prevent duplicate payments.  
- Payment processing is simulated; no real payment gateway integration is included.  
- JWT authentication is required for all endpoints. Expired or invalid tokens are rejected.  
- `CreatedDate` is automatically set by the database; `UpdatedDate` is updated whenever the payment status changes.  
- The system is designed for internal services and trusted integrations.

---

## Architecture Decisions
- **Layered Design**: API → Service → Repository → Database, ensuring separation of concerns and maintainability.  
- **Repository + Stored Procedures**: Dapper with stored procedures chosen for explicit control, reliability, and clear SQL execution.  
- **Idempotency Enforcement**: Unique constraint on `ReferenceID` ensures business-level duplicate prevention, with application checks as a secondary layer.  
- **Primary Key vs Business Key**:  
  - `PaymentID` (INT) as the internal DB primary key for indexing and relational integrity.  
  - `ReferenceID` enforces business uniqueness.  
  - `PaymentGuid` provides a safe external identifier for clients.  
- **Status Management**: Payment statuses are constrained (`Pending`, `Success`, `Failed`) at the database level for data integrity.

---

## Trade-offs
- **Stored Procedures vs ORM (Entity Framework)**: SPs provide deterministic SQL control, but less automatic mapping and migration support.  
- **Synchronous Payment Processing**: Easier to implement and test; asynchronous background processing could improve scalability.  
- **Indexes on Status and CreatedDate**: Improves query performance for reporting and status-based queries; minor storage and maintenance overhead.

---

## Getting Started
1. Clone the repo
2. Set up the database
3. Configure `appsettings.json` with DB connection and JWT settings
4. Run the API

---

## API Endpoints

| Method | Endpoint                  | Description            |
|--------|---------------------------|------------------------|
| POST   | /auth/login               | Create a token         |
| POST   | /payments                 | Create a new payment   |
| GET    | /payments/{ReferenceID}   | Retrieve a payment     |

---

## Example Request 

```
POST /payments
{ 
  "ReferenceID": "REF123456",
  "Amount": 150.00,
  "Currency": "USD",
}
```
---

## Example Response
```
POST /payments
{
  "paymentGuid": "17ab16cf-aa67-4aff-b49d-1dc93ee74d09",
  "referenceID": "55111100-e29b-41d4-a716-446655440103",
  "status": 1,
  "message": "Payment processed successfully"
}
```
---

## Database Schema

Payments Table:
```
PaymentID INT IDENTITY(1,1) PRIMARY KEY
PaymentGuid UNIQUEIDENTIFIER UNIQUE
ReferenceID NVARCHAR(50) UNIQUE
Amount DECIMAL(18,2) CHECK (Amount > 0)
Currency CHAR(3) CHECK (LEN(Currency) = 3)
Status NVARCHAR(20) CHECK (Status IN ('Pending', 'Success', 'Failed'))
CreatedDate DATETIME2 DEFAULT SYSUTCDATETIME()
UpdatedDate DATETIME2 NULL
```
---

## Security

- JWT authentication required for all endpoints.
- Idempotency enforced via ReferenceID to prevent duplicate payments.



