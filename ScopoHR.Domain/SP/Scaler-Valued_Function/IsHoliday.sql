IsHolidayUSE [ScopoHR]
GO
/****** Object:  UserDefinedFunction [dbo].[IsHoliday]    Script Date: 05/20/2018 2:52:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[IsHoliday](@date DATETIME)
RETURNS BIT
AS
BEGIN	
	DECLARE @isHoliday BIT = IIF(dbo.GetHolidayIdByDate(@date) > 0, 1, 0)
RETURN @isHoliday;
END
