--IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'IsHoliday')
--BEGIN
--	DROP FUNCTION IsHoliday
--END
--GO
CREATE FUNCTION [dbo].[IsHoliday](@date DATETIME)
RETURNS BIT
AS
BEGIN	
	DECLARE @isHoliday BIT = IIF(dbo.GetHolidayIdByDate(@date) > 0, 1, 0)
RETURN @isHoliday;
END
