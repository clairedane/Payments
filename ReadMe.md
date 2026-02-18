# Payments API

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

