USE [ScopoHR]
GO
/****** Object:  UserDefinedFunction [dbo].[GetHolidayIdByDate]    Script Date: 05/20/2018 2:52:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[GetHolidayIdByDate](@date DATETIME)
RETURNS INT
AS
BEGIN
	DECLARE @id INT = 
		(SELECT HLD.HolidayId FROM Holidays HLD
	WHERE CAST(@date AS DATE) >= CAST(HLD.FromDate AS DATE) AND CAST(@date AS DATE) <= CAST(HLD.ToDate AS DATE))
RETURN @id
END
