--IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'GetHolidayIdByDate')
--BEGIN
--	DROP FUNCTION GetHolidayIdByDate
--END
--GO
CREATE FUNCTION [dbo].[GetHolidayIdByDate](@date DATETIME)
RETURNS INT
AS
BEGIN
	DECLARE @id INT = 
		(SELECT HLD.HolidayId FROM Holidays HLD
	WHERE CAST(@date AS DATE) >= CAST(HLD.FromDate AS DATE) AND CAST(@date AS DATE) <= CAST(HLD.ToDate AS DATE))
RETURN @id
END
