--IF EXISTS(SELECT * FROM Sys.objects WHERE name = 'GetAttendanceReport')
--BEGIN
--	DROP PROCEDURE GetAttendanceReport
--END
--GO


CREATE PROCEDURE [dbo].[GetAttendanceReport]
	@fromDate DATETIME,
	@toDate DATETIME,
	@floor VARCHAR(200) = '',
	@branchID INT,
	@shiftID VARCHAR(10) = '',
	@cardNo VARCHAR(15) = ''
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @departmentID INT;

	IF @cardNo IS NOT NULL
	BEGIN
		SELECT @departmentID = DepartmentID FROM Employees WHERE CardNo = @cardNo
	END

	CREATE TABLE #Data
	(
		Day	nvarchar(50),
		Date	date,
		EmployeeID INT NULL,
		CardNo	nvarchar(200),
		EmployeeName	nvarchar(500),
		InTime	varchar(20),
		InTimeDate datetime NULL,
		OutTime	varchar(20),
		OutTimeDate datetime NULL,
		Status	varchar(5),
		TotalMinutes int,
		OTMinutes	int,
		LunchTime	int
	)
	
	INSERT INTO #Data
	EXEC GetAttendanceData @fromDate, @toDate, @floor, @branchID, @shiftID, @cardNo

	/********************************* Absent *******************************************/
	CREATE TABLE #Absent
	(
		EmployeeID INT,
		EmployeeName VARCHAR(500),
		CardNo VARCHAR(50),
		DesignationName VARCHAR(200),
		Floor VARCHAR(200),
		AbsentDate DATETIME,
		Status VARCHAR(10) NULL
	)

	INSERT INTO #Absent
	EXEC GetAbsentReport @fromDate, @toDate, @floor, @branchID, @shiftID, @cardNo, 0
	
	INSERT INTO #Data
	SELECT DATENAME(DW,CAST(A.AbsentDate AS Date)) as [Day], 
	CAST(A.AbsentDate AS DATE) AS Date, A.EmployeeID,
	A.CardNo, A.EmployeeName, '' AS InTime, NULL AS InTimeDate, '' AS OutTime, NULL AS OutTimeDate, A.Status,
	NULL AS TotalMinutes, NULL AS OTMinutes, 60 AS LunchTime
	FROM #Absent A
	
	UPDATE A
	SET A.OTMinutes = ISNULL(A.OTMinutes, 0) + ISNULL(A.TotalMinutes, 0)
	FROM #Data A
	WHERE dbo.GetHolidayStatus(A.Date, A.EmployeeID, @departmentID) IN ('WH', 'HD') 
	AND InTime IS NOT NULL AND A.Status NOT IN ('WH', 'HD')
	
	IF @cardNo IS NOT NULL
	BEGIN
		UPDATE A
		SET A.Status = 'NJ'
		FROM #Data A
		INNER JOIN Employees E ON A.EmployeeID = E.EmployeeID
		WHERE A.CardNo = @cardNo AND A.Date < E.JoinDate
	END 

	SELECT Day, Date, CardNo, EmployeeName, InTime, OutTime, Status, TotalMinutes AS WorkingHours, OTMinutes, LunchTime
	FROM #Data ORDER BY Date

	DROP TABLE #Absent
	DROP TABLE #Data
END

--EXEC GetAttendanceReport '2017-10-01', '2017-10-31','', '1','','3108503'
--EXEC GetAttendanceData '11/20/2017 12:00:00 AM', '11/20/2017 12:00:00 AM','', '1','','1312519'