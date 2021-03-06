USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetAttendanceReport]    Script Date: 05/20/2018 2:44:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetAttendanceReport]
	@fromDate DATETIME,
	@toDate DATETIME,
	@floor VARCHAR(200) = '',
	@branchID INT,
	@shiftID VARCHAR(10) = '',
	@cardNo VARCHAR(15) = ''
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @archiveCount int = (
	SELECT COUNT(*) FROM ArchiveMonth 
	WHERE ArchiveMonth=DATEPART(MM,@fromDate)
	AND ArchiveYear=DATEPART(YYYY,@fromDate)
	AND ArchiveMonth=DATEPART(MM,@toDate)
	AND ArchiveYear=DATEPART(YYYY,@toDate)
	GROUP BY ID)

	DECLARE @departmentID INT, @isActive BIT;

	IF @cardNo IS NOT NULL AND @cardNo <> ''
	BEGIN
		SELECT @departmentID = DepartmentID FROM Employees WHERE CardNo = @cardNo
	END

	

	IF(@archiveCount > 0 AND @departmentID <> 6)

	BEGIN
		SELECT A.Day, A.Date, A.CardNo, A.EmployeeName, A.InTime, A.OutTime, A.Status, A.TotalMinutes AS WorkingHours, A.OTMinutes, A.LunchTime	 
		FROM ArchiveData A
		INNER JOIN Employees E ON A.EmployeeID = E.EmployeeID
		INNER JOIN ProductionFloorLines P ON P.ProductionFloorLineID=E.ProductionFloorLineID
		WHERE CAST(A.Date As DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)
		AND P.Floor=IIF(@floor = '', P.Floor, @floor)
		AND E.CardNo = IIF(@cardNo = '', E.CardNo, @cardNo)
		AND E.ShiftId = IIF(@shiftID = '', E.ShiftID, @shiftID)
		ORDER BY Date
	RETURN;
	END


	--DECLARE @departmentID INT, @isActive BIT;

	IF @cardNo IS NOT NULL AND @cardNo <> ''
	BEGIN
		SELECT @departmentID = DepartmentID FROM Employees WHERE CardNo = @cardNo

		SELECT @isActive = IsActive FROM Employees WHERE CardNo = @cardNo
		IF @isActive = 0
		BEGIN
			UPDATE Employees SET IsActive = 1 WHERE CardNo = @cardNo
		END
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
	NULL AS TotalMinutes, NULL AS OTMinutes, 0 AS LunchTime
	FROM #Absent A

	
	
	UPDATE A
	SET A.OTMinutes = ISNULL(A.TotalMinutes, 0), Status = dbo.GetHolidayStatus(A.Date, A.EmployeeID, @departmentID)
	FROM #Data A
	WHERE dbo.GetHolidayStatus(A.Date, A.EmployeeID, @departmentID) IN ('WH', 'HD') 
	AND A.Status IN ('P', 'D')

	UPDATE A
	SET A.Status  = LT.ShortName, A.TotalMinutes = 0, A.OTMinutes = 0
	FROM #Data A
	LEFT JOIN LeaveApplications LA ON A.EmployeeID = LA.EmployeeID
	INNER JOIN LeaveTypes LT ON LA.LeaveTypeID = LT.LeaveTypeID
	WHERE 
	A.Status IN ('P', 'D') AND LA.IsDeleted <> 1
	AND A.Date BETWEEN LA.FromDate AND LA.ToDate

	UPDATE A
	SET A.Status = dbo.GetHolidayStatus(A.Date, A.EmployeeID, @departmentID)
	FROM #Data A
	WHERE dbo.GetHolidayStatus(A.Date, A.EmployeeID, @departmentID) IN ('WH', 'HD') 
	AND A.Status = 'WP'

	UPDATE A
	SET A.Status = 'NJ'
	FROM #Data A
	INNER JOIN Employees E ON A.EmployeeID = E.EmployeeID
	WHERE A.Date < E.JoinDate

	UPDATE A
	SET A.Status = 'LEFT', InTime = NULL, OutTime = NULL, TotalMinutes = 0, OTMinutes = 0, LunchTime = 0
	FROM #Data A
	INNER JOIN InactiveEmployees E ON A.EmployeeID = E.EmployeeID
	WHERE A.Date >= E.ApplicableDate

	IF @cardNo IS NOT NULL AND @cardNo <> ''
	BEGIN
		IF @isActive = 0
		BEGIN
			UPDATE Employees SET IsActive = 0 WHERE CardNo = @cardNo
		END
	END

	SELECT Day, Date, CardNo, EmployeeName, InTime, OutTime, Status, TotalMinutes AS WorkingHours, OTMinutes, LunchTime
	FROM #Data ORDER BY Date

	DROP TABLE #Absent
	DROP TABLE #Data
END

--EXEC GetAttendanceReport '2018-04-03', '2018-04-03','', '1','6',''
