--IF EXISTS(SELECT * FROM Sys.objects WHERE name = 'GetOneTimePunchReport')
--BEGIN
--	DROP PROCEDURE GetOneTimePunchReport
--END
--GO

CREATE PROCEDURE [dbo].[GetOneTimePunchReport]
	@fromDate DATETIME,
	@shiftID VARCHAR(10) = '',
	@floor VARCHAR(200) = ''
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #Data
	(
		Day	nvarchar(50),
		Date	date,
		EmployeeID int,
		CardNo	nvarchar(200),
		EmployeeName	nvarchar(500),
		InTime	varchar(20),
		InTimeDate datetime,
		OutTime	varchar(20),
		OutTimeDate datetime,
		Status	varchar(5),
		TotalMinutes int,
		OTMinutes	int,
		LunchTime	int
	)
	
	INSERT INTO #Data
	EXEC GetAttendanceData @fromDate, @fromDate, @floor, 1, @shiftID, ''

	SELECT CardNo, EmployeeName AS NameOfEmployee, '' AS Designation, InTime, OutTime, @FromDate as [Date],@ShiftId as ShifId, @Floor as [Floor],InTimeDate, OutTimeDate
	FROM #Data 
	WHERE OutTime IS NULL
	OR DATEDIFF(MINUTE, InTime, OutTime) < 6
	ORDER BY CardNo

	DROP TABLE #Data
END

--EXEC GetOneTimePunchReport '2017-11-25', '1', ''


