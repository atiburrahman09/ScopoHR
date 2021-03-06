USE [ScopoHR]
GO
/****** Object:  UserDefinedFunction [dbo].[GetMonthlyOverTime]    Script Date: 05/20/2018 2:50:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[GetMonthlyOverTime](@year INT, @month INT, @branchId INT)
RETURNS @table TABLE
(CardNo VARCHAR(20), OverTime INT)
AS
BEGIN
	INSERT INTO @table
	SELECT DTT.CardNo, SUM(DTT.OverTime) AS OverTime FROM DailyAttendances DTT
	WHERE DATEPART(YEAR, DTT.Date) = @year AND DATEPART(MONTH, DTT.Date) = @month
	GROUP BY DTT.CardNo
	RETURN
END;
