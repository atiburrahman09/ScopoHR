--IF EXISTS(SELECT * FROM Sys.objects WHERE name = 'GetAttendanceData')
--BEGIN
--	DROP PROCEDURE GetAttendanceData
--END
--GO

CREATE PROCEDURE [dbo].[GetAttendanceData]
	@fromDate DATETIME,
	@toDate DATETIME,
	@floor VARCHAR(200) = '',
	@branchID INT,
	@shiftID VARCHAR(10) = '',
	@cardNo VARCHAR(15) = ''
AS
BEGIN
	SET NOCOUNT ON;

	CREATE TABLE #Attendance(
		[Day] VARCHAR (20) NULL,
		[Date] DATE NULL,
		[EmployeeID] INT NULL,
		[CardNo] VARCHAR(20) NULL,
		[EmployeeName] VARCHAR(200) NULL,
		[InTime] VARCHAR(20) NULL,
		[OutTime] VARCHAR(20) NULL,
		[ShiftId] INT NULL,
		[ShiftInTime] DATETIME NULL,
		[ShiftOutTime] DATETIME NULL
	)

	DECLARE @departmentID INT = 0;

	IF @cardNo IS NOT NULL AND @cardNo <> ''
	BEGIN
		SELECT @departmentID = DepartmentID FROM Employees WHERE CardNo = @cardNo
	END

	-- Day Shift
	IF (@shiftID = 1 OR @shiftID = '') AND @departmentID <> 6
	BEGIN
		INSERT INTO #Attendance
		SELECT 
		DATENAME(DW,CAST(AT.InOutTime as Date)) as [Day],
		CAST(AT.InOutTime AS DATE) AS [Date],
		EMP.EmployeeID, EMP.CardNo, EMP.EmployeeName,
		MIN(AT.InOutTime) AS InTime,
		IIF(MIN(AT.InOutTime) = MAX(AT.InOutTime), NULL, MAX(AT.InOutTime)) AS OutTime,
		WS.Id AS ShiftId, 
		CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST(WS.InTime AS TIME) AS DATETIME) AS ShiftInTime,
		CASE WHEN Ws.Id = 2 THEN 
			DATEADD(DAY, 1, CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST(WS.OutTime AS TIME) AS DATETIME))
		ELSE
			CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST(WS.OutTime AS TIME) AS DATETIME) END AS ShiftOutTime
		FROM Employees EMP
		INNER JOIN WorkingShifts WS ON EMP.ShiftId = WS.Id 
		INNER JOIN CardNoMappings CM ON EMP.CardNo = CM.OriginalCardNo
		LEFT JOIN Attendances AT ON CM.GeneratedCardNo = AT.CardNo
		LEFT JOIN ProductionFloorLines P on EMP.ProductionFloorLineID = P.ProductionFloorLineID
		WHERE EMP.IsActive = 1 
		AND EMP.DepartmentID <> 6
		AND CAST(AT.[InOutTime] as Date) BETWEEN  CAST(@fromDate as Date) AND CAST(@toDate as Date) 
		AND P.Floor=IIF(@floor = '', P.Floor, @floor)
		AND EMP.CardNo = IIF(@cardNo = '', EMP.CardNo, @cardNo)
		AND EMP.ShiftId = IIF(@shiftID = '', EMP.ShiftID, @shiftID)
		GROUP BY CAST(AT.InOutTime AS DATE), EMP.EmployeeID, EMP.CardNo, EMP.EmployeeName, WS.Id, WS.InTime, WS.OutTime
	END

	-- Night Shift
	ELSE IF (@shiftID = 2) AND @departmentID <> 6
	BEGIN
		INSERT INTO #Attendance
		SELECT 
		DATENAME(DW,CAST(AT.InOutTime as Date)) as [Day],
		CAST(AT.InOutTime AS DATE) AS [Date],
		EMP.EmployeeID, EMP.CardNo, EMP.EmployeeName,
		MIN(AT.InOutTime) AS InTime,
		(SELECT MIN(A.InOutTime) FROM Attendances A WHERE A.CardNo = CM.GeneratedCardNo And CAST(A.InOutTime AS DATE) = DATEADD(DAY, 1, CAST(MIN(AT.InOutTime) AS DATE))) AS OutTime,
		WS.Id AS ShiftId, 
		CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST(WS.InTime AS TIME) AS DATETIME) AS ShiftInTime,
		CASE WHEN Ws.Id = 2 THEN 
			DATEADD(DAY, 1, CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST(WS.OutTime AS TIME) AS DATETIME))
		ELSE
			CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST(WS.OutTime AS TIME) AS DATETIME) END AS ShiftOutTime
		FROM Employees EMP
		INNER JOIN WorkingShifts WS ON EMP.ShiftId = WS.Id 
		INNER JOIN CardNoMappings CM ON EMP.CardNo = CM.OriginalCardNo
		LEFT JOIN Attendances AT ON CM.GeneratedCardNo = AT.CardNo
		LEFT JOIN ProductionFloorLines P on EMP.ProductionFloorLineID = P.ProductionFloorLineID
		WHERE EMP.IsActive = 1 
		AND EMP.DepartmentID <> 6
		AND CAST(AT.[InOutTime] as Date) BETWEEN  CAST(@fromDate as Date) AND CAST(@toDate as Date)
		AND AT.InOutTime > DATEADD(HOUR, -2, CAST(CAST(AT.InOutTime AS DATE) AS DATETIME) + CAST(CAST(WS.InTime AS TIME) AS DATETIME))
		AND P.Floor=IIF(@floor = '', P.Floor, @floor)
		AND EMP.CardNo = IIF(@cardNo = '', EMP.CardNo, @cardNo)
		AND EMP.ShiftId = IIF(@shiftID = '', EMP.ShiftID, @shiftID)
		GROUP BY CAST(AT.InOutTime AS DATE), EMP.EmployeeID, EMP.CardNo, CM.GeneratedCardNo, EMP.EmployeeName, WS.Id, WS.InTime, WS.OutTime
	END

	ELSE IF @shiftID IN (3, 4, 5, 6) OR @departmentID = 6
	BEGIN
		INSERT INTO #Attendance
		SELECT 
		DATENAME(DW,CAST(AT.InOutTime AS Date)) AS [Day],
		CAST(AT.InOutTime AS DATE) AS [Date],
		EMP.EmployeeID, EMP.CardNo, EMP.EmployeeName,
		MIN(AT.InOutTime) AS InTime,
		CASE WHEN WS.Id = 5 THEN 
				(SELECT MIN(a.InOutTime) FROM Attendances A WHERE A.CardNo = CM.GeneratedCardNo And CAST(a.InOutTime AS DATE) = DATEADD(DAY, 1, CAST(MIN(AT.InOutTime) AS DATE)))
			ELSE 
				IIF(MIN(AT.InOutTime) = MAX(AT.InOutTime), NULL, MAX(AT.InOutTime)) 
		END AS OutTime,
		WS.Id AS ShiftId,
		CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST(WS.InTime AS TIME) AS DATETIME) AS ShiftInTime,
		CASE WHEN Ws.Id = 5 THEN 
			DATEADD(DAY, 1, CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST(WS.OutTime AS TIME) AS DATETIME))
		ELSE
			CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST(WS.OutTime AS TIME) AS DATETIME) END AS ShiftOutTime
		FROM Employees EMP
		INNER JOIN CardNoMappings CM ON EMP.CardNo = CM.OriginalCardNo
		LEFT JOIN Attendances AT ON CM.GeneratedCardNo = AT.CardNo
		INNER JOIN SecurityGuardRosters SR ON EMP.EmployeeID = SR.EmployeeId AND CAST(AT.InOutTime AS DATE) = CAST(SR.WorkingDate AS DATE) AND SR.ShiftId = IIF(@shiftID = '', SR.ShiftID, @shiftID)
		INNER JOIN WorkingShifts WS ON SR.ShiftId = WS.Id
		LEFT JOIN ProductionFloorLines P on EMP.ProductionFloorLineID = P.ProductionFloorLineID
		WHERE EMP.IsActive = 1
		AND EMP.DepartmentID = 6
		AND CAST(AT.[InOutTime] as Date) BETWEEN  CAST(@fromDate as Date) AND CAST(@toDate as Date) 
		AND AT.InOutTime > DATEADD(HOUR, -2, CAST(CAST(AT.InOutTime AS DATE) AS DATETIME) + CAST(CAST(WS.InTime AS TIME) AS DATETIME)) 
		--AND SR.ShiftId = IIF(@shiftID = '', SR.ShiftID, @shiftID)
		AND P.Floor=IIF(@floor = '', P.Floor, @floor)
		AND EMP.CardNo = IIF(@cardNo = '', EMP.CardNo, @cardNo)
		GROUP BY CAST(AT.InOutTime AS DATE), EMP.EmployeeID, EMP.CardNo, CM.GeneratedCardNo, EMP.EmployeeName, WS.Id, WS.InTime, WS.OutTime
	END

	SELECT E.[Day], E.[Date], E.EmployeeID, E.CardNo, E.EmployeeName, 
	CONVERT(VARCHAR(5), CAST(E.InTime AS TIME), 108) AS InTime, E.InTime AS InTimeDate, CONVERT(VARCHAR(5), CAST(E.OutTime AS TIME), 108) AS OutTime, E.OutTime AS OutTimeDate,
	IIF(E.InTime < DATEADD(MINUTE, 5, E.ShiftInTime), 'P', 'D') AS Status,
	DATEDIFF(MINUTE, E.ShiftInTime, E.OutTime) - IIF(E.ShiftId IN (1,2,6), 60, 0) AS TotalMinutes,
	IIF(E.OutTime > DATEADD(MINUTE, 5, E.ShiftOutTime),
		DATEDIFF(MINUTE, E.ShiftOutTime, E.OutTime), NULL) AS OTMinutes,
	IIF(E.ShiftId IN (1,2,6), 60, 0) AS LunchTime
	FROM #Attendance E

	DROP TABLE #Attendance
END

--EXEC GetAttendanceData '2017-11-18', '2017-11-24','', '1','2',''