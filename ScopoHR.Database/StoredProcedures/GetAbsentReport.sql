--IF EXISTS(SELECT * FROM sys.objects WHERE name = 'GetAbsentReport')
--BEGIN
--	DROP PROCEDURE GetAbsentReport
--END
--GO

CREATE PROCEDURE [dbo].[GetAbsentReport]
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

	IF (@shiftID IN(1, 2) OR @shiftID = '') AND @departmentID != 6
	BEGIN
		INSERT INTO #Absent(EmployeeID, EmployeeName, CardNo, DesignationName, Floor, AbsentDate)
		SELECT EMP.EmployeeID, EMP.EmployeeName, EMP.CardNo AS CardNo, DG.DesignationName, P.Floor, C.WorkingDate AS AbsentDate
		FROM #CalendarDays C
		CROSS JOIN Employees EMP
		INNER JOIN Designations DG ON EMP.DesignationID = DG.DesignationID
		LEFT JOIN ProductionFloorLines P on EMP.ProductionFloorLineID = P.ProductionFloorLineID
		WHERE EMP.IsActive = 1
		AND EMP.CardNo = IIF(@cardNo = '', EMP.CardNo, @cardNo)
		AND EMP.ShiftId = IIF(@shiftID = '', EMP.ShiftID, @shiftID)
		AND NOT EXISTS (
			SELECT * 
			FROM Attendances A
			INNER JOIN CardNoMappings CM ON A.CardNo = CM.GeneratedCardNo
			WHERE EMP.CardNo = CM.OriginalCardNo
			AND CAST(a.InOutTime AS DATE) = C.WorkingDate);
	END
	ELSE IF @shiftID IN (3, 4, 5, 6) OR @departmentID = 6
	BEGIN
		INSERT INTO #Absent(EmployeeID, EmployeeName, CardNo, DesignationName, Floor, AbsentDate)
		SELECT EMP.EmployeeID, EMP.EmployeeName, EMP.CardNo AS CardNo, DG.DesignationName, P.Floor, C.WorkingDate AS AbsentDate
		FROM #CalendarDays C
		CROSS JOIN Employees EMP
		INNER JOIN Designations DG ON EMP.DesignationID = DG.DesignationID
		LEFT JOIN ProductionFloorLines P on EMP.ProductionFloorLineID = P.ProductionFloorLineID
		WHERE EMP.IsActive = 1
		AND EMP.CardNo = IIF(@cardNo = '', EMP.CardNo, @cardNo)
		AND EMP.DepartmentID = 6
		--AND EMP.ShiftId = IIF(@shiftID = '', EMP.ShiftID, @shiftID)
		AND NOT EXISTS (
			SELECT * 
			FROM Attendances A
			INNER JOIN CardNoMappings CM ON A.CardNo = CM.GeneratedCardNo
			INNER JOIN Employees E ON CM.OriginalCardNo = E.CardNo
			INNER JOIN SecurityGuardRosters SR ON E.EmployeeID = SR.EmployeeId AND CAST(C.WorkingDate AS DATE) = CAST(SR.WorkingDate AS DATE)
			WHERE EMP.CardNo = CM.OriginalCardNo
			AND CAST(a.InOutTime AS DATE) = C.WorkingDate);
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
	--AND LA.Status = 1

	UPDATE A
	SET A.Status = 'A'
	FROM #Absent A
	WHERE A.Status IS NULL

	--IF @AbsentOnly = 1
	--BEGIN
	--	SELECT * FROM #Absent WHERE Status = 'A'
	--END
	--ELSE
	--BEGIN
	--	SELECT * FROM #Absent
	--END

	SELECT * FROM #Absent ORDER BY CardNo

	DROP TABLE #Absent
	DROP TABLE #CalendarDays
END

--EXEC [GetAbsentReport] '2017-11-25', '2017-11-25','', '1','1','', 0
