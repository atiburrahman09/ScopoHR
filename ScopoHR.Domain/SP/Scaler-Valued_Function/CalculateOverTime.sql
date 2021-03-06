USE [ScopoHR]
GO
/****** Object:  UserDefinedFunction [dbo].[CalculateOverTime]    Script Date: 05/20/2018 2:51:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[CalculateOverTime](@employeeOutTime DATETIME, @officeOutTime DATETIME)
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
