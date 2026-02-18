CREATE PROCEDURE [dbo].[Payments_Select]
	@ReferenceID NVARCHAR(50)
AS
BEGIN
    SELECT * FROM Payments
    WHERE ReferenceID = @ReferenceID
END
GO
