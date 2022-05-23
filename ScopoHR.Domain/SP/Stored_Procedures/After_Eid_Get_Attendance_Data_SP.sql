USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetAttendanceData]    Script Date: 07/05/2018 11:50:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[GetAttendanceData]
	@fromDate DATETIME,
	@toDate DATETIME,
	@floor VARCHAR(200) = '',
	@branchID INT,
	@shiftID VARCHAR(10) = '',
	@cardNo VARCHAR(15) = '',
	@departmentID int = null
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

	--DECLARE @departmentID INT = 0;

	IF @cardNo IS NOT NULL AND @cardNo <> ''
	BEGIN
		SELECT @departmentID = DepartmentID FROM Employees WHERE CardNo = @cardNo
	END

	IF @cardNo IS NOT NULL AND @cardNo <> '' AND @departmentID <> 6
	BEGIN
		SELECT @shiftID = ShiftId FROM Employees WHERE CardNo = @cardNo
	END
	

	-- Day Shift
	IF (@shiftID = 1 OR @shiftID = '') AND @departmentID <> 6
		BEGIN
			
			
			--###########################   Ramadan Calculation ################################################--------
			INSERT INTO #Attendance
			SELECT 
			DATENAME(DW,CAST(AT.InOutTime as Date)) as [Day],
			CAST(AT.InOutTime AS DATE) AS [Date],
			EMP.EmployeeID, EMP.CardNo, EMP.EmployeeName,
			MIN(AT.InOutTime) AS InTime,
			IIF(MIN(AT.InOutTime) = MAX(AT.InOutTime), NULL, MAX(AT.InOutTime)) AS OutTime,
			WS.Id AS ShiftId, 
			CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST('2017-07-10 06:30:00.000' AS TIME) AS DATETIME) AS ShiftInTime,
			CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST('2017-07-10 15:00:00.000' AS TIME) AS DATETIME) AS ShiftOutTime
			FROM Employees EMP
			INNER JOIN WorkingShifts WS ON EMP.ShiftId = WS.Id 
			INNER JOIN CardNoMappings CM ON EMP.CardNo = CM.OriginalCardNo
			LEFT JOIN Attendances AT ON CM.GeneratedCardNo = AT.CardNo
			LEFT JOIN ProductionFloorLines P on EMP.ProductionFloorLineID = P.ProductionFloorLineID
			WHERE 
			EMP.IsActive = 1 
			AND EMP.DepartmentID <> 6
			AND CAST(AT.[InOutTime] as Date) BETWEEN  CAST(@fromDate AS DATE) AND IIF(CAST(@fromDate AS DATE) = CAST(@todate AS DATE),@toDate,IIF(CAST(@toDate AS DATE) < '2018-06-14',@toDate,'2018-06-13'))--IIF(CAST(@fromDate AS DATE) = CAST(@todate AS DATE),@fromDate,IIF(CAST(@toDate AS DATE) < '2018-06-14',@toDate,'2018-06-13')) AND CAST(@toDate AS date)
			AND AT.InOutTime > DATEADD(HOUR, -2, CAST(CAST(AT.InOutTime AS DATE) AS DATETIME) + CAST(CAST('2017-07-10 06:30:00.000' AS TIME) AS DATETIME)) 
			AND P.Floor=IIF(@floor = '', P.Floor, @floor)
			AND EMP.CardNo = IIF(@cardNo = '', EMP.CardNo, @cardNo)
			AND EMP.ShiftId = IIF(@shiftID = '', EMP.ShiftID, @shiftID)
			AND AT.IsDeleted=0
			AND(CAST(@fromDate AS DATE) < '2018-06-14')
			GROUP BY CAST(AT.InOutTime AS DATE), EMP.EmployeeID, EMP.CardNo, EMP.EmployeeName, WS.Id

			--######################################## After ramadan #################################################-------------

			INSERT INTO #Attendance
			SELECT 
			DATENAME(DW,CAST(AT.InOutTime as Date)) as [Day],
			CAST(AT.InOutTime AS DATE) AS [Date],
			EMP.EmployeeID, EMP.CardNo, EMP.EmployeeName,
			MIN(AT.InOutTime) AS InTime,
			IIF(MIN(AT.InOutTime) = MAX(AT.InOutTime), NULL, MAX(AT.InOutTime)) AS OutTime,
			WS.Id AS ShiftId, 
			CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST(WS.InTime AS TIME) AS DATETIME) AS ShiftInTime,
			CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST(WS.OutTime AS TIME) AS DATETIME) AS ShiftOutTime
			FROM Employees EMP
			INNER JOIN WorkingShifts WS ON EMP.ShiftId = WS.Id 
			INNER JOIN CardNoMappings CM ON EMP.CardNo = CM.OriginalCardNo
			LEFT JOIN Attendances AT ON CM.GeneratedCardNo = AT.CardNo
			LEFT JOIN ProductionFloorLines P on EMP.ProductionFloorLineID = P.ProductionFloorLineID
			WHERE 
			EMP.IsActive = 1 
			AND EMP.DepartmentID <> 6
			AND CAST(AT.[InOutTime] as Date) BETWEEN  IIF(CAST(@fromDate AS DATE) = CAST(@todate AS DATE),@fromDate,'2018-06-14') AND CAST(@toDate AS DATE)--IIF(CAST(@fromDate AS DATE) = CAST(@todate AS DATE),@todate,'2018-05-18')
			AND AT.InOutTime > DATEADD(HOUR, -2, CAST(CAST(AT.InOutTime AS DATE) AS DATETIME) + CAST(CAST(WS.InTime AS TIME) AS DATETIME)) 
			AND P.Floor=IIF(@floor = '', P.Floor, @floor)
			AND EMP.CardNo = IIF(@cardNo = '', EMP.CardNo, @cardNo)
			AND EMP.ShiftId = IIF(@shiftID = '', EMP.ShiftID, @shiftID)
			AND AT.IsDeleted=0
			AND(CAST(@toDate AS DATE) > '2018-06-13')
			GROUP BY CAST(AT.InOutTime AS DATE), EMP.EmployeeID, EMP.CardNo, EMP.EmployeeName, WS.Id, WS.InTime, WS.OutTime
		END
	
	-- Night Shift
	ELSE IF (@shiftID = 2) AND @departmentID <> 6
	BEGIN
		
		------######################################################### Ramadan Calculation  ##################################------------------
		
		INSERT INTO #Attendance
		SELECT 
		DATENAME(DW,CAST(AT.InOutTime as Date)) as [Day],
		CAST(AT.InOutTime AS DATE) AS [Date],
		EMP.EmployeeID, EMP.CardNo, EMP.EmployeeName,
		MIN(AT.InOutTime) AS InTime,
		(SELECT MIN(a.InOutTime) FROM Attendances A WHERE A.CardNo = CM.GeneratedCardNo And CAST(a.InOutTime AS DATE) = DATEADD(DAY, 1, CAST(MIN(AT.InOutTime) AS DATE)))		
		 AS OutTime,
		WS.Id AS ShiftId, 
		CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST('2017-07-10 20:00:00.000' AS TIME) AS DATETIME) AS ShiftInTime,
		DATEADD(DAY, 1, CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST('2017-07-10 05:00:00.000' AS TIME) AS DATETIME)) AS ShiftOutTime
		FROM Employees EMP
		INNER JOIN WorkingShifts WS ON EMP.ShiftId = WS.Id 
		INNER JOIN CardNoMappings CM ON EMP.CardNo = CM.OriginalCardNo
		LEFT JOIN Attendances AT ON CM.GeneratedCardNo = AT.CardNo
		LEFT JOIN ProductionFloorLines P on EMP.ProductionFloorLineID = P.ProductionFloorLineID
		WHERE EMP.IsActive = 1
		AND EMP.DepartmentID <> 6
		AND CAST(AT.[InOutTime] as Date) BETWEEN  CAST(@fromDate AS DATE) AND IIF(CAST(@fromDate AS DATE) = CAST(@todate AS DATE),@toDate,IIF(CAST(@toDate AS DATE) < '2018-06-14',@toDate,'2018-06-13'))--IIF(CAST(@fromDate AS DATE) = CAST(@todate AS DATE),@fromDate,IIF(CAST(@toDate AS DATE) < '2018-06-14',@toDate,'2018-06-13')) AND CAST(@toDate AS date)
		AND AT.InOutTime > DATEADD(HOUR, -2, CAST(CAST(AT.InOutTime AS DATE) AS DATETIME) + CAST(CAST('2017-07-10 20:00:00.000' AS TIME) AS DATETIME))
		AND P.Floor=IIF(@floor = '', P.Floor, @floor)
		AND EMP.CardNo = IIF(@cardNo = '', EMP.CardNo, @cardNo)
		AND EMP.ShiftId = IIF(@shiftID = '', EMP.ShiftID, @shiftID)
		AND AT.IsDeleted=0
		AND(CAST(@fromDate AS DATE) < '2018-06-14')
		GROUP BY CAST(AT.InOutTime AS DATE), EMP.EmployeeID, EMP.CardNo, CM.GeneratedCardNo, EMP.EmployeeName, WS.Id

		------########################################################## After Ramadan ###################################################--------------------

		INSERT INTO #Attendance
		SELECT 
		DATENAME(DW,CAST(AT.InOutTime as Date)) as [Day],
		CAST(AT.InOutTime AS DATE) AS [Date],
		EMP.EmployeeID, EMP.CardNo, EMP.EmployeeName,
		MIN(AT.InOutTime) AS InTime,
		(SELECT MIN(a.InOutTime) FROM Attendances A WHERE A.CardNo = CM.GeneratedCardNo And CAST(a.InOutTime AS DATE) = DATEADD(DAY, 1, CAST(MIN(AT.InOutTime) AS DATE)))		
		 AS OutTime,
		WS.Id AS ShiftId, 
		CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST(WS.InTime AS TIME) AS DATETIME) AS ShiftInTime,
		DATEADD(DAY, 1, CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST(WS.OutTime AS TIME) AS DATETIME)) AS ShiftOutTime
		FROM Employees EMP
		INNER JOIN WorkingShifts WS ON EMP.ShiftId = WS.Id 
		INNER JOIN CardNoMappings CM ON EMP.CardNo = CM.OriginalCardNo
		LEFT JOIN Attendances AT ON CM.GeneratedCardNo = AT.CardNo
		LEFT JOIN ProductionFloorLines P on EMP.ProductionFloorLineID = P.ProductionFloorLineID
		WHERE EMP.IsActive = 1
		AND EMP.DepartmentID <> 6
		AND CAST(AT.[InOutTime] as Date) BETWEEN  IIF(CAST(@fromDate AS DATE) = CAST(@todate AS DATE),@fromDate,'2018-06-14') AND CAST(@toDate AS DATE)--IIF(CAST(@fromDate AS DATE) = CAST(@todate AS DATE),@todate,'2018-05-18')
		AND AT.InOutTime > DATEADD(HOUR, -2, CAST(CAST(AT.InOutTime AS DATE) AS DATETIME) + CAST(CAST(WS.InTime AS TIME) AS DATETIME))
		AND P.Floor=IIF(@floor = '', P.Floor, @floor)
		AND EMP.CardNo = IIF(@cardNo = '', EMP.CardNo, @cardNo)
		AND EMP.ShiftId = IIF(@shiftID = '', EMP.ShiftID, @shiftID)
		AND AT.IsDeleted=0
		AND(CAST(@toDate AS DATE) > '2018-06-13')
		GROUP BY CAST(AT.InOutTime AS DATE), EMP.EmployeeID, EMP.CardNo, CM.GeneratedCardNo, EMP.EmployeeName, WS.Id, WS.InTime, WS.OutTime
	END

	ELSE IF @shiftID IN (3, 4, 5, 6) OR @departmentID = 6
	BEGIN
		
		------######################################################### Ramadan Calculation  ##################################------------------
		--SELECT @shiftID
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
		CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + IIF(WS.Id = 6,CAST(CAST('2017-07-10 06:30:00.000' AS TIME) AS DATETIME),CAST(CAST(WS.InTime AS TIME) AS DATETIME)) AS ShiftInTime,
		CASE WHEN Ws.Id = 5 THEN 
			DATEADD(DAY, 1, CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + CAST(CAST(WS.OutTime AS TIME) AS DATETIME))
		ELSE
			CAST(CAST(MIN(AT.InOutTime)AS DATE) AS DATETIME) + IIF(WS.Id = 6,CAST(CAST('2017-07-10 15:00:00.000' AS TIME) AS DATETIME),CAST(CAST(WS.OutTime AS TIME) AS DATETIME)) END AS ShiftOutTime
		FROM Employees EMP
		INNER JOIN CardNoMappings CM ON EMP.CardNo = CM.OriginalCardNo
		LEFT JOIN Attendances AT ON CM.GeneratedCardNo = AT.CardNo
		INNER JOIN SecurityGuardRosters SR ON EMP.EmployeeID = SR.EmployeeId AND CAST(AT.InOutTime AS DATE) = CAST(SR.WorkingDate AS DATE) AND SR.ShiftId = IIF(@shiftID = '', SR.ShiftID, @shiftID)
		INNER JOIN WorkingShifts WS ON SR.ShiftId = WS.Id
		LEFT JOIN ProductionFloorLines P on EMP.ProductionFloorLineID = P.ProductionFloorLineID
		WHERE EMP.IsActive = 1
		AND EMP.DepartmentID = 6
		AND CAST(AT.[InOutTime] as Date) BETWEEN  CAST(@fromDate AS DATE) AND IIF(CAST(@fromDate AS DATE) = CAST(@todate AS DATE),@toDate,IIF(CAST(@toDate AS DATE) < '2018-06-14',@toDate,'2018-06-13'))--IIF(CAST(@fromDate AS DATE) = CAST(@todate AS DATE),@fromDate,IIF(CAST(@toDate AS DATE) < '2018-06-14',@toDate,'2018-06-13')) AND CAST(@toDate AS date)
		AND AT.InOutTime > DATEADD(HOUR, -2, CAST(CAST(AT.InOutTime AS DATE) AS DATETIME) + IIF(WS.Id = 6,CAST(CAST('2017-07-10 06:30:00.000' AS TIME) AS DATETIME),CAST(CAST(WS.InTime AS TIME) AS DATETIME))) 
		--AND SR.ShiftId = IIF(@shiftID = '', SR.ShiftID, @shiftID)
		AND P.Floor=IIF(@floor = '', P.Floor, @floor)
		AND EMP.CardNo = IIF(@cardNo = '', EMP.CardNo, @cardNo)
		AND AT.IsDeleted=0
		AND(CAST(@fromDate AS DATE) < '2018-06-14')
		GROUP BY CAST(AT.InOutTime AS DATE), EMP.EmployeeID, EMP.CardNo, CM.GeneratedCardNo, EMP.EmployeeName, WS.Id, WS.InTime, WS.OutTime

		-------############################################################## After Ramadan ############################################-------------------------

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
		AND CAST(AT.[InOutTime] as Date) BETWEEN  IIF(CAST(@fromDate AS DATE) = CAST(@todate AS DATE),@fromDate,'2018-06-14') AND CAST(@toDate AS DATE)--IIF(CAST(@fromDate AS DATE) = CAST(@todate AS DATE),@todate,'2018-05-18')
		AND AT.InOutTime > DATEADD(HOUR, -2, CAST(CAST(AT.InOutTime AS DATE) AS DATETIME) + CAST(CAST(WS.InTime AS TIME) AS DATETIME)) 
		--AND SR.ShiftId = IIF(@shiftID = '', SR.ShiftID, @shiftID)
		AND P.Floor=IIF(@floor = '', P.Floor, @floor)
		AND EMP.CardNo = IIF(@cardNo = '', EMP.CardNo, @cardNo)
		AND AT.IsDeleted=0
		AND(CAST(@toDate AS DATE) > '2018-06-13')
		GROUP BY CAST(AT.InOutTime AS DATE), EMP.EmployeeID, EMP.CardNo, CM.GeneratedCardNo, EMP.EmployeeName, WS.Id, WS.InTime, WS.OutTime

		
	END

	--select * from #Attendance
	--return;

	SELECT E.[Day], E.[Date], E.EmployeeID, E.CardNo, E.EmployeeName, 
	CONVERT(VARCHAR(5), CAST(E.InTime AS TIME), 108) AS InTime, 
	E.InTime AS InTimeDate, CONVERT(VARCHAR(5), 
	CAST(E.OutTime AS TIME), 108) AS OutTime, 
	E.OutTime AS OutTimeDate,
	IIF(E.InTime <= DATEADD(MINUTE, 5, E.ShiftInTime), 'P', 'D') AS Status,
	DATEDIFF
	(
		MINUTE, 
		IIF(E.InTime BETWEEN DATEADD(MINUTE, -9, E.ShiftInTime) AND DATEADD(MINUTE, 5, E.ShiftInTime), E.ShiftInTime, E.InTime), 
		IIF(ABS(DATEPART(MINUTE, E.ShiftOutTime) - DATEPART(MINUTE, E.OutTime)) <= 5, DATEADD(MINUTE, (DATEPART(MINUTE, E.ShiftOutTime) - DATEPART(MINUTE, E.OutTime)), E.OutTime),
			IIF(ABS(DATEPART(MINUTE, E.ShiftOutTime) - DATEPART(MINUTE, E.OutTime)) >= 55, DATEADD(MINUTE, (DATEPART(MINUTE, E.ShiftOutTime) - DATEPART(MINUTE, E.OutTime) + 60), E.OutTime), 
			E.OutTime
			)
		)
	) 
	--IIF(E.ShiftId IN (1,2,6) AND E.OutTime > DATEADD(MINUTE, 330, E.ShiftInTime) AND E.InTime < DATEADD(MINUTE, 330, E.ShiftInTime), 60, 0) AS TotalMinutes, 
	-IIF(E.ShiftId IN (1,2,6) AND E.OutTime > DATEADD(MINUTE, 330, E.ShiftInTime) AND E.InTime < DATEADD(MINUTE, 330, E.ShiftInTime), IIF((CAST(E.Date AS DATE) <= '2018-06-13' AND E.ShiftId IN(1,6)) ,30,60), 0) AS TotalMinutes,

	IIF(E.OutTime > DATEADD(MINUTE, 5, E.ShiftOutTime), 
		DATEDIFF
		(
			MINUTE, 
			IIF(E.ShiftOutTime > E.InTime, E.ShiftOutTime, E.InTIme), 
			IIF(ABS(DATEPART(MINUTE, E.ShiftOutTime) - DATEPART(MINUTE, E.OutTime)) <= 5, DATEADD(MINUTE, (DATEPART(MINUTE, E.ShiftOutTime) - DATEPART(MINUTE, E.OutTime)), E.OutTime),
			IIF(ABS(DATEPART(MINUTE, E.ShiftOutTime) - DATEPART(MINUTE, E.OutTime)) >= 55, DATEADD(MINUTE, (DATEPART(MINUTE, E.ShiftOutTime) - DATEPART(MINUTE, E.OutTime) + 60), E.OutTime), 
			E.OutTime
			)
		)
		)
	, 0)
	+ IIF(E.InTime < DATEADD(MINUTE, -9, E.ShiftInTime), DATEDIFF(MINUTE, E.InTime, E.ShiftInTime), 0) AS OTMinutes,
	IIF(E.ShiftId IN (1,2,6) AND E.OutTime > DATEADD(MINUTE, 330, E.ShiftInTime) AND E.InTime < DATEADD(MINUTE, 330, E.ShiftInTime), IIF((CAST(E.Date AS DATE) <= '2018-06-13' AND E.ShiftId IN(1,6)) ,30,60), 0) AS LunchTime
	FROM #Attendance E
	ORDER BY E.Date

	DROP TABLE #Attendance
END

--EXEC GetAttendanceData '2018-06-23', '2018-06-23','3rd Way -1', '1','1',''