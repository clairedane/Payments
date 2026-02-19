CREATE PROCEDURE [dbo].[Payments_Select]
	@ReferenceID NVARCHAR(50) = NULL
AS
BEGIN
    SELECT * FROM Payments
    WHERE (@ReferenceID IS NULL OR @ReferenceID = '' OR ReferenceID = @ReferenceID)
END
GO
