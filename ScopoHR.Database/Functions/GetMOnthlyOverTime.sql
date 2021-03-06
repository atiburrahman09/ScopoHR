--IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'GetMonthlyOverTime')
--BEGIN
--	DROP FUNCTION GetMonthlyOverTime
--END
--GO
CREATE FUNCTION [dbo].[GetMonthlyOverTime](@year INT, @month INT, @branchId INT)
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
