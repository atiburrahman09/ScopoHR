--IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'GetComparableTime')
--BEGIN
--	DROP FUNCTION GetComparableTime
--END
--GO
CREATE FUNCTION [dbo].[GetComparableTime](@date DATETIME)
RETURNS VARCHAR(MAX)
AS 
BEGIN
	DECLARE @res AS VARCHAR(MAX) = (
		SELECT     
		RIGHT('0' + CONVERT([varchar](2), DATEPART(HOUR, @date)), 2) + ':' +
		RIGHT('0' + CONVERT([varchar](2), DATEPART(MINUTE, @date)), 2)
		)
	RETURN @res;
END;
