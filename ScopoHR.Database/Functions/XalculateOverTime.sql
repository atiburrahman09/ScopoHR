--IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'CalculateOverTime')
--BEGIN
--	DROP FUNCTION CalculateOverTime
--END
--GO
CREATE FUNCTION [dbo].[CalculateOverTime](@employeeOutTime DATETIME, @officeOutTime DATETIME)
RETURNS INT
AS
BEGIN
DECLARE @hour INT = 
	(SELECT IIF(
	DATEDIFF(MINUTE, 
		CAST(DATEADD(MINUTE, 30, CAST(@officeOutTime AS TIME)) AS TIME),
		CAST(@employeeOutTime AS TIME)
	) >= 0,
	DATEDIFF(MINUTE,
		CAST(DATEADD(MINUTE, 30, CAST(@officeOutTime AS TIME)) AS TIME),
		CAST(@employeeOutTime AS TIME)
	), 0
))
RETURN CEILING(CAST(@hour AS FLOAT)/CAST(60 AS FLOAT));
END;
