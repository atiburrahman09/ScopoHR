USE [ScopoHR]
GO
/****** Object:  UserDefinedFunction [dbo].[GetHolidayStatus]    Script Date: 05/20/2018 2:52:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[GetHolidayStatus](@date DATETIME, @employeeID INT, @departmentID INT)
RETURNS VARCHAR(2)
AS
BEGIN	
	DECLARE @status VARCHAR(2);

	SET @status = (SELECT IIF(HLD.Title = 'Weekend', 'WH', 'HD') AS Holiday FROM Holidays HLD
				WHERE CAST(@date AS DATE) >= CAST(HLD.FromDate AS DATE) AND CAST(@date AS DATE) <= CAST(HLD.ToDate AS DATE))

	IF EXISTS(SELECT * FROM SecurityGuardHoliday WHERE EmployeeID = @employeeID) AND (@status IS NULL  OR @status != 'HD')
	BEGIN
		SET @status = (SELECT 'WH' AS Holiday FROM SecurityGuardHoliday H
		WHERE H.EmployeeID = @employeeID AND H.WeeklyHoliday = DATENAME(DW, @date))
	END

	--IF @departmentID = 6 AND (@status IS NULL  OR @status != 'HD')
	--BEGIN
	--	SET @status = (SELECT 'WH' AS Holiday FROM SecurityGuardHoliday H
	--	WHERE H.EmployeeID = @employeeID AND H.WeeklyHoliday = DATENAME(DW, @date))
	--END
RETURN @status;
END
