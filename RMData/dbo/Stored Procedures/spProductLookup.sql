CREATE PROCEDURE [dbo].[spProductLookup]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id,ProductName,[Description],RetailPrice,QuantityInStock,isTaxable FROM [dbo].[Product] ORDER BY ProductName;
END
