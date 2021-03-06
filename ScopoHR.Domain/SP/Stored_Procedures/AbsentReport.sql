USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetAbsentReport]    Script Date: 05/20/2018 2:43:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetAbsentReport]
	@fromDate DATETIME,
	@toDate DATETIME,
	@floor VARCHAR(200) = '',
	@branchID INT,
	@shiftID VARCHAR(10) = '',
	@cardNo VARCHAR(15) = '',
	@AbsentOnly BIT = 1
AS
BEGIN
	SET NOCOUNT ON;


	DECLARE @archiveCount int = (
	SELECT COUNT(*) FROM ArchiveMonth 
	WHERE ArchiveMonth=DATEPART(MM,@fromDate)
	AND ArchiveYear=DATEPART(YYYY,@fromDate)
	GROUP BY ID)



	IF(@archiveCount > 0)

	BEGIN
		SELECT A.EmployeeID, A.EmployeeName,A.CardNo,D.DesignationName,P.Floor,@fromDate AS AbsentDate,A.Status	 
		FROM ArchiveData A
		INNER JOIN Employees E ON A.EmployeeID = E.EmployeeID
		INNER JOIN Designations D ON E.DesignationID=D.DesignationID
		INNER JOIN ProductionFloorLines P ON P.ProductionFloorLineID=E.ProductionFloorLineID
		WHERE CAST(A.Date As DATE) = CAST(@fromDate AS DATE) 
		AND P.Floor=IIF(@floor = '', P.Floor, @floor)
		AND E.ShiftId = IIF(@shiftID = '', E.ShiftID, @shiftID)
		AND A.Status NOT IN ('P','D','WH','HD','SL','NJ','LEFT')
		ORDER BY Date
	RETURN;
	END

	SELECT WorkingDate = DATEADD(Day,Number,@fromDate)
	INTO #CalendarDays
	FROM  master..spt_values 
	WHERE Type='P'
	AND DATEADD(day,Number,@fromDate) <= @toDate

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

	DECLARE @departmentID INT = 0;

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
		INSERT INTO #Absent(EmployeeID, EmployeeName, CardNo, DesignationName, Floor, AbsentDate)
		SELECT EMP.EmployeeID, EMP.EmployeeName, EMP.CardNo AS CardNo, DG.DesignationName, P.Floor, C.WorkingDate AS AbsentDate
		FROM #CalendarDays C
		CROSS JOIN Employees EMP
		LEFT JOIN Designations DG ON EMP.DesignationID = DG.DesignationID
		LEFT JOIN ProductionFloorLines P on EMP.ProductionFloorLineID = P.ProductionFloorLineID
		INNER JOIN WorkingShifts WS ON EMP.ShiftId = WS.Id
		WHERE EMP.IsActive = 1 AND EMP.DepartmentID <> 6
		AND EMP.CardNo = IIF(@cardNo = '', EMP.CardNo, @cardNo)
		AND EMP.ShiftId = IIF(@shiftID = '', EMP.ShiftID, @shiftID)
		AND NOT EXISTS (
			SELECT * 
			FROM Attendances A
			INNER JOIN CardNoMappings CM ON A.CardNo = CM.GeneratedCardNo
			WHERE EMP.CardNo = CM.OriginalCardNo
			AND CAST(a.InOutTime AS DATE) = C.WorkingDate
			AND a.InOutTime > DATEADD(HOUR, -2, CAST(CAST(a.InOutTime AS DATE) AS DATETIME) + CAST(CAST(WS.InTime AS TIME) AS DATETIME))
			AND A.IsDeleted=0
		);
	END
	-- Night Shift
	ELSE IF (@shiftID = 2) AND @departmentID <> 6
	BEGIN
		INSERT INTO #Absent(EmployeeID, EmployeeName, CardNo, DesignationName, Floor, AbsentDate)
		SELECT EMP.EmployeeID, EMP.EmployeeName, EMP.CardNo AS CardNo, DG.DesignationName, P.Floor, C.WorkingDate AS AbsentDate
		FROM #CalendarDays C
		CROSS JOIN Employees EMP
		LEFT JOIN Designations DG ON EMP.DesignationID = DG.DesignationID
		LEFT JOIN ProductionFloorLines P on EMP.ProductionFloorLineID = P.ProductionFloorLineID
		INNER JOIN WorkingShifts WS ON EMP.ShiftId = WS.Id
		WHERE EMP.IsActive = 1 AND EMP.DepartmentID <> 6
		AND EMP.CardNo = IIF(@cardNo = '', EMP.CardNo, @cardNo)
		AND EMP.ShiftId = IIF(@shiftID = '', EMP.ShiftID, @shiftID)
		AND NOT EXISTS (
			SELECT * 
			FROM Attendances A
			INNER JOIN CardNoMappings CM ON A.CardNo = CM.GeneratedCardNo
			WHERE EMP.CardNo = CM.OriginalCardNo
			AND CAST(a.InOutTime AS DATE) = C.WorkingDate
			AND a.InOutTime > DATEADD(HOUR, -3, CAST(CAST(a.InOutTime AS DATE) AS DATETIME) + CAST(CAST(WS.InTime AS TIME) AS DATETIME)) 
			AND A.IsDeleted=0
		);
	END
	-- Security
	ELSE IF @shiftID IN (3, 4, 5, 6) OR @departmentID = 6
	BEGIN
		INSERT INTO #Absent(EmployeeID, EmployeeName, CardNo, DesignationName, Floor, AbsentDate)
		SELECT EMP.EmployeeID, EMP.EmployeeName, EMP.CardNo AS CardNo, DG.DesignationName, P.Floor, C.WorkingDate AS AbsentDate
		FROM #CalendarDays C
		CROSS JOIN Employees EMP
		LEFT JOIN Designations DG ON EMP.DesignationID = DG.DesignationID
		LEFT JOIN ProductionFloorLines P on EMP.ProductionFloorLineID = P.ProductionFloorLineID
		WHERE EMP.IsActive = 1
		AND EMP.CardNo = IIF(@cardNo = '', EMP.CardNo, @cardNo)
		AND EMP.DepartmentID = 6
		AND NOT EXISTS (
			SELECT * 
			FROM Attendances A
			INNER JOIN CardNoMappings CM ON A.CardNo = CM.GeneratedCardNo
			INNER JOIN Employees E ON CM.OriginalCardNo = E.CardNo
			INNER JOIN SecurityGuardRosters SR ON E.EmployeeID = SR.EmployeeId AND CAST(C.WorkingDate AS DATE) = CAST(SR.WorkingDate AS DATE)
			WHERE EMP.CardNo = CM.OriginalCardNo
			AND CAST(a.InOutTime AS DATE) = C.WorkingDate)
			AND A.IsDeleted=0
	END

	IF @floor <> ''
		DELETE A FROM #Absent A WHERE A.Floor <> @floor

	
	UPDATE A
	SET A.Status = dbo.GetHolidayStatus(A.AbsentDate, A.EmployeeID, @departmentID)
	FROM #Absent A

	
	UPDATE A
	SET A.Status  = LT.ShortName
	FROM #Absent A
	LEFT JOIN LeaveApplications LA ON A.EmployeeID = LA.EmployeeID
	INNER JOIN LeaveTypes LT ON LA.LeaveTypeID = LT.LeaveTypeID
	WHERE 
	A.AbsentDate BETWEEN LA.FromDate AND LA.ToDate
	AND LA.IsDeleted <> 1
	

	UPDATE A
	SET A.Status = 'A'
	FROM #Absent A
	WHERE A.Status IS NULL
	
	SELECT * FROM #Absent 
	WHERE CardNo NOT LIKE '14%' 
	--AND Status = 'A'
	ORDER BY CardNo

	DROP TABLE #Absent
	DROP TABLE #CalendarDays
END

--EXEC [GetAbsentReport] '2018-04-01', '2018-04-01','3rd Way -4', '1','1','', 0
