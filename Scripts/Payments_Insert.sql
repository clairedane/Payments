CREATE PROCEDURE [dbo].[Payments_Insert]
    @PaymentGuid UNIQUEIDENTIFIER,
    @ReferenceID NVARCHAR(50),
    @Amount DECIMAL(18,2),
    @Currency CHAR(3),
    @Status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Payments 
           (PaymentGuid,
            ReferenceID,
            Amount, 
            Currency, 
            Status)
    VALUES (@PaymentGuid, 
            @ReferenceID, 
            @Amount, 
            @Currency, 
            @Status);

    SELECT SCOPE_IDENTITY();
END
