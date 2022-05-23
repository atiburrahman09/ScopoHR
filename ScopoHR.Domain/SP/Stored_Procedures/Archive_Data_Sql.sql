CREATE TABLE #AttendanceDataTemp(
		[Day] [varchar](20) NULL,
		[Date] [date] NULL,
		[EmployeeID] [int] NULL,
		[CardNo] [varchar](20) NULL,
		[EmployeeName] [varchar](200) NULL,
		[InTime] [varchar](20) NULL,
		[InTimeDate] datetime NULL,
		[OutTime] [varchar](20) NULL,
		[OutTimeDate] datetime NULL,
		[Status] VARCHAR(10),
		[TotalMinutes] [int] NULL,
		[OTMinutes] [int] NULL,
		[LunchTime] [int] NULL
	)
	
			UPDATE EMP
	SET IsActive = 1
	FROM Employees EMP
	INNER JOIN InactiveEmployees IE ON EMP.EmployeeID = IE.EmployeeID
	WHERE ApplicableDate > '2018-06-01'

		
	INSERT INTO #AttendanceDataTemp([Day], [Date], EmployeeID, CardNo, EmployeeName, InTime, InTimeDate, OutTime, OutTimeDate, [Status], TotalMinutes, OTMinutes, LunchTime)
	EXEC GetAttendanceData '2018-04-01', '2018-04-30', '', 1, '1', ''

	CREATE TABLE #AbsentDataTemp
	(
		EmployeeID INT,
		EmployeeName VARCHAR(500),
		CardNo VARCHAR(50),
		DesignationName VARCHAR(200),
		Floor VARCHAR(200),
		AbsentDate DATETIME,
		Status VARCHAR(10) NULL
	)

	INSERT INTO #AbsentDataTemp
	EXEC GetAbsentReport '2018-04-01', '2018-04-30', '', 1, '1', '', 0
	
	INSERT INTO #AttendanceDataTemp
	SELECT DATENAME(DW,CAST(A.AbsentDate AS Date)) as [Day], 
	CAST(A.AbsentDate AS DATE) AS Date, A.EmployeeID,
	A.CardNo, A.EmployeeName, '' AS InTime, NULL AS InTimeDate, '' AS OutTime, NULL AS OutTimeDate, A.Status,
	NULL AS TotalMinutes, NULL AS OTMinutes, 0 AS LunchTime
	FROM #AbsentDataTemp A
	
	UPDATE A
	SET A.OTMinutes = ISNULL(A.TotalMinutes, 0), Status = dbo.GetHolidayStatus(A.Date, A.EmployeeID, '')
	FROM #AttendanceDataTemp A
	WHERE dbo.GetHolidayStatus(A.Date, A.EmployeeID, '') IN ('WH', 'HD') 
	AND A.Status IN ('P', 'D')

	UPDATE A
	SET A.Status  = LT.ShortName, A.TotalMinutes = 0, A.OTMinutes = 0
	FROM #AttendanceDataTemp A
	LEFT JOIN LeaveApplications LA ON A.EmployeeID = LA.EmployeeID
	INNER JOIN LeaveTypes LT ON LA.LeaveTypeID = LT.LeaveTypeID
	WHERE 
	A.Status IN ('P', 'D') AND LA.IsDeleted <> 1
	AND A.Date BETWEEN LA.FromDate AND LA.ToDate

	UPDATE A
	SET A.Status = dbo.GetHolidayStatus(A.Date, A.EmployeeID, '')
	FROM #AttendanceDataTemp A
	WHERE dbo.GetHolidayStatus(A.Date, A.EmployeeID, '') IN ('WH', 'HD') 
	AND A.Status = 'WP'

	UPDATE A
	SET A.Status = 'NJ'
	FROM #AttendanceDataTemp A
	INNER JOIN Employees E ON A.EmployeeID = E.EmployeeID
	WHERE A.Date < E.JoinDate

	UPDATE A
	SET A.Status = ''
	FROM #AttendanceDataTemp A
	INNER JOIN InactiveEmployees E ON A.EmployeeID = E.EmployeeID
	WHERE A.Date >= E.ApplicableDate

	
	--INSERT INTO ArchiveData
	select A.Day,A.Date,A.EmployeeID,A.CardNo,A.EmployeeName,A.InTime,A.InTimeDate,A.OutTime,A.OutTimeDate,A.Status,A.TotalMinutes,A.OTMinutes,A.LunchTime,E.DesignationID,E.ShiftId,E.EmployeeType,P.Floor
	from #AttendanceDataTemp A
	INNER JOIN Employees E ON A.EmployeeID = E.EmployeeID
	INNER JOIN ProductionFloorLines P ON P.ProductionFloorLineID=E.ProductionFloorLineID


UPDATE EMP
	SET IsActive = 0
	FROM Employees EMP
	INNER JOIN InactiveEmployees IE ON EMP.EmployeeID = IE.EmployeeID
	WHERE ApplicableDate > '2018-06-01'

	--SELECT * FROM ArchiveData

	--DROP TABLE #AttendanceDataTemp
	--DROP TABLE #AbsentDataTemp
