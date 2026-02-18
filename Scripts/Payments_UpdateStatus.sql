CREATE PROCEDURE [dbo].[Payments_UpdateStatus]
    @PaymentID INT,
    @Status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Payments
    SET Status = @Status,
        UpdatedDate = SYSUTCDATETIME()
    WHERE PaymentID = @PaymentID;
END
GO
