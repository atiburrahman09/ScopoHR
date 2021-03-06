USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[CalculateSalary]    Script Date: 05/20/2018 2:42:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CalculateSalary]
	@fromDate DATETIME,
	@toDate DATETIME,
	@floor VARCHAR(200) = '',
	@branchID INT,
	@shiftID VARCHAR(10) = '',
	@cardNo VARCHAR(15) = '',
	@employeeType INT = 1
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @departmentID INT,@daysOfMonth INT;

	SELECT @daysOfMonth=DATEDIFF(DD,@fromDate,@toDate) + 1
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


	BEGIN 
			IF @cardNo IS NOT NULL AND @cardNo <> ''
	BEGIN
		SELECT @departmentID = DepartmentID FROM Employees WHERE CardNo = @cardNo
	END
	
	UPDATE EMP
	SET IsActive = 1
	FROM Employees EMP
	INNER JOIN InactiveEmployees IE ON EMP.EmployeeID = IE.EmployeeID
	WHERE ApplicableDate > @fromDate


		
	INSERT INTO #AttendanceDataTemp([Day], [Date], EmployeeID, CardNo, EmployeeName, InTime, InTimeDate, OutTime, OutTimeDate, [Status], TotalMinutes, OTMinutes, LunchTime)
	EXEC GetAttendanceData @fromDate, @toDate, @floor, @branchID, @shiftID, @cardNo

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
	EXEC GetAbsentReport @fromDate, @toDate, @floor, @branchID, @shiftID, @cardNo, 0
	
	INSERT INTO #AttendanceDataTemp
	SELECT DATENAME(DW,CAST(A.AbsentDate AS Date)) as [Day], 
	CAST(A.AbsentDate AS DATE) AS Date, A.EmployeeID,
	A.CardNo, A.EmployeeName, '' AS InTime, NULL AS InTimeDate, '' AS OutTime, NULL AS OutTimeDate, A.Status,
	NULL AS TotalMinutes, NULL AS OTMinutes, 0 AS LunchTime
	FROM #AbsentDataTemp A
	
	UPDATE A
	SET A.OTMinutes = ISNULL(A.TotalMinutes, 0), Status = dbo.GetHolidayStatus(A.Date, A.EmployeeID, @departmentID)
	FROM #AttendanceDataTemp A
	WHERE dbo.GetHolidayStatus(A.Date, A.EmployeeID, @departmentID) IN ('WH', 'HD') 
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
	SET A.Status = dbo.GetHolidayStatus(A.Date, A.EmployeeID, @departmentID)
	FROM #AttendanceDataTemp A
	WHERE dbo.GetHolidayStatus(A.Date, A.EmployeeID, @departmentID) IN ('WH', 'HD') 
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

	END


	--SELECT * FROM #AttendanceDataTemp

	SELECT AT.EmployeeID, AT.CardNo, AT.EmployeeName,
	DATEDIFF(DAY, IIF(EMP.JoinDate > @fromDate, EMP.JoinDate, @fromDate), IIF(IE.ApplicableDate IS NULL, @toDate, DATEADD(DAY, -1, IE.ApplicableDate))) + 1 AS TotalDay,
	COUNT(CASE WHEN AT.Status = 'P' OR AT.Status = 'D' THEN 1 END) AS TotalPresentDay,
	COUNT(CASE WHEN AT.Status = 'WH' OR AT.Status = 'HD' THEN 1 END) AS TotalHoliDay,
	COUNT(CASE WHEN (AT.Status = 'WH' OR AT.Status = 'HD') AND (AT.TotalMinutes IS NOT NULL OR AT.TotalMinutes <> 0)  THEN 1 END) AS TotalHoliDayWork,
	COUNT(CASE WHEN AT.Status = 'A' OR AT.Status = 'WP' THEN 1 END) AS TotalAbsentDay,
	COUNT(CASE WHEN AT.Status = 'D' OR ((AT.Status = 'P' OR AT.Status = 'D') AND AT.TotalMinutes < 475)  THEN 1 END) AS TotalLateDay,
	COUNT(CASE WHEN AT.Status <> 'A' AND AT.Status <> 'NJ' AND AT.Status <> '' AND AT.Status <> 'MT' AND AT.Status <> 'WP'  THEN 1 END) AS TotalWorkingDay,
	COUNT(CASE WHEN AT.Status = 'CL'  THEN 1 END) AS CL,
	COUNT(CASE WHEN AT.Status = 'EL'  THEN 1 END) AS EL,
	COUNT(CASE WHEN AT.Status = 'ML'  THEN 1 END) AS ML,
	COUNT(CASE WHEN AT.Status = 'MT'  THEN 1 END) AS MT,
	COUNT(CASE WHEN AT.Status = 'CP' OR AT.Status = 'FL'  THEN 1 END) AS CP,
	COUNT(CASE WHEN AT.Status = 'SL'  THEN 1 END) AS SL,
	COUNT(CASE WHEN AT.Status = 'SH'  THEN 1 END) AS SH,
	SUM(AT.TotalMinutes) AS TotalMinutes, SUM(AT.OTMinutes) AS OTMinutes,
	SUM(IIF(AT.Status = 'P' OR AT.Status = 'D', 480 - (AT.TotalMinutes - At.OTMinutes), 0)) AS SubtractMinutes,
	(SELECT Amount FROM SalaryMappings WHERE EmployeeID = AT.EmployeeID AND SalaryTypeID = 1) AS BasicSalary,
	(SELECT Amount FROM SalaryMappings WHERE EmployeeID = AT.EmployeeID AND SalaryTypeID = 4) AS MedicalAllowance,
	(SELECT Amount FROM SalaryMappings WHERE EmployeeID = AT.EmployeeID AND SalaryTypeID = 5) AS FoodAllowance,
	(SELECT Amount FROM SalaryMappings WHERE EmployeeID = AT.EmployeeID AND SalaryTypeID = 7) AS Conveyance,
	(SELECT Amount FROM SalaryMappings WHERE EmployeeID = AT.EmployeeID AND SalaryTypeID = 6) AS HouseRent,
	OTApplicable, AttendanceBonusApplicable
	INTO #DataTemp
	FROM #AttendanceDataTemp AT
	INNER JOIN Employees EMP ON AT.CardNo = EMP.CardNo
	LEFT JOIN InactiveEmployees IE ON EMP.EmployeeID = IE.EmployeeID
	WHERE EMP.CardNo NOT LIKE '14%'
	--AND EMP.EmployeeType = @employeeType
	GROUP BY AT.EmployeeID, AT.CardNo, AT.EmployeeName, EMP.JoinDate, IE.ApplicableDate, OTApplicable, AttendanceBonusApplicable
	ORDER BY AT.CardNo

	--SELECT * FROM #DataTemp
	--RETURN

	IF @cardNo IS NOT NULL AND @cardNo <> ''
	BEGIN
		DELETE FROM MonthlySalaries WHERE CardNo = @cardNo AND [Month] = MONTH(@fromDate) AND [Year] = YEAR(@fromDate)
	END
	ELSE
	BEGIN
		DELETE MSL FROM MonthlySalaries MSL 
		INNER JOIN Employees EMP ON MSL.EmployeeId = EMP.EmployeeID
		WHERE [Month] = MONTH(@fromDate) AND [Year] = YEAR(@fromDate)
		AND EMP.DepartmentID <> 6 AND EMP.ShiftId = @shiftID AND EMP.EmployeeType = @employeeType
	END

	IF @employeeType = 1
	BEGIN
		PRINT 'Worker'

		INSERT INTO MonthlySalaries(
			[Year], [Month], EmployeeId, [Name], CardNo, 
			[Basic], HouseRent, MedicalAllowance, Conveyance, FoodAllowance,
			GrossWage, 
			PD, HD, TWD, 
			CasualLeave, EarnedLeave, MedicalLeave, MaternityLeave, FLeave,ShortLeave,SHLeave,
			AttendanceBonus, 
			PayableWages,
			OTH, OTRate, OTTaka, TotalPay, 
			AbsentDays, Food, IsTaken, BranchID
		)
		SELECT 
		YEAR(@fromDate), MONTH(@fromDate), A.EmployeeID, A.EmployeeName, A.CardNo,
		A.BasicSalary, A.HouseRent, A.MedicalAllowance, A.Conveyance, A.FoodAllowance, 
		A.BasicSalary + A.HouseRent + A.MedicalAllowance + A.FoodAllowance + A.Conveyance,
		A.TotalPresentDay + A.TotalHoliDayWork, A.TotalHoliDay - A.TotalHoliDayWork, A.TotalWorkingDay,
		A.CL, A.EL, A.ML, A.MT, A.CP,A.SL,A.SH,

		IIF(A.TotalAbsentDay > 0 OR A.TotalLateDay > 0 OR A.TotalDay != @daysOfMonth OR A.MT > 0, 0, 500) AS AttendanceBonus,

		((A.TotalDay - A.MT) * ((A.BasicSalary + A.HouseRent + A.MedicalAllowance + A.FoodAllowance + A.Conveyance) / @daysOfMonth)) 
			- (A.TotalAbsentDay * (A.BasicSalary / 30)) 
			- A.SubtractMinutes * (A.BasicSalary / (30 * 480))
			+ IIF(A.TotalAbsentDay > 0 OR A.TotalLateDay > 0 OR A.TotalDay != @daysOfMonth OR A.MT > 0, 0, 500),
		A.OTMinutes, A.BasicSalary / 104,

		(A.BasicSalary / (104 * 60)) * A.OTMinutes,

		((A.TotalDay - A.MT) * ((A.BasicSalary + A.HouseRent + A.MedicalAllowance + A.FoodAllowance + A.Conveyance) / @daysOfMonth))
			- (A.TotalAbsentDay * (A.BasicSalary / 30)) 
			- A.SubtractMinutes * (A.BasicSalary / (30 * 480))
			+ IIF(A.TotalAbsentDay > 0 OR A.TotalLateDay > 0 OR A.TotalDay != @daysOfMonth OR A.MT > 0, 0, 500) 
			+ ISNULL((A.BasicSalary / (104 * 60)) * A.OTMinutes, 0),

		A.TotalAbsentDay, A.SubtractMinutes * (A.BasicSalary / (30 * 480)) AS Food, 0, 1
		FROM #DataTemp A
	END
	ELSE IF @employeeType = 2
	BEGIN
		PRINT 'Staff'

		INSERT INTO MonthlySalaries(
			[Year], [Month], EmployeeId, [Name], CardNo, 
			[Basic], HouseRent, MedicalAllowance, Conveyance, FoodAllowance,
			GrossWage, 
			PD, HD, TWD, 
			CasualLeave, EarnedLeave, MedicalLeave, MaternityLeave, FLeave,ShortLeave,SHLeave,
			AttendanceBonus, 
			PayableWages,
			OTH, OTRate, OTTaka, TotalPay, 
			AbsentDays, Food, IsTaken, BranchID
		)
		SELECT 
		YEAR(@fromDate), MONTH(@fromDate), A.EmployeeID, A.EmployeeName, A.CardNo,
		A.BasicSalary, A.HouseRent, A.MedicalAllowance, A.Conveyance, A.FoodAllowance, 
		A.BasicSalary + A.HouseRent + A.MedicalAllowance + A.FoodAllowance + A.Conveyance,
		A.TotalPresentDay + A.TotalHoliDayWork, A.TotalHoliDay - A.TotalHoliDayWork, A.TotalWorkingDay,
		A.CL, A.EL, A.ML, A.MT, A.CP,A.SL,A.SH,

		IIF(A.AttendanceBonusApplicable = 0 OR A.TotalAbsentDay > 0 OR A.TotalLateDay > 0 OR A.TotalDay != @daysOfMonth OR A.MT > 0, 0, 500) AS AttendanceBonus,

		((A.TotalDay - A.MT) * ((A.BasicSalary + A.HouseRent + A.MedicalAllowance + A.FoodAllowance + A.Conveyance) / @daysOfMonth)) 
			- (A.TotalAbsentDay * (A.BasicSalary / 30)) 
			- IIF(A.OTApplicable = 0, 0, (A.SubtractMinutes * (A.BasicSalary / (30 * 480))))
			+ IIF(A.AttendanceBonusApplicable = 0 OR A.TotalAbsentDay > 0 OR A.TotalLateDay > 0 OR A.TotalDay != @daysOfMonth OR A.MT > 0, 0, 500),

		IIF(A.OTApplicable = 0, 0, A.OTMinutes), IIF(A.OTApplicable = 0, 0, A.BasicSalary / 104),

		IIF(A.OTApplicable = 0, 0, (A.BasicSalary / (104 * 60)) * A.OTMinutes),

		((A.TotalDay - A.MT) * ((A.BasicSalary + A.HouseRent + A.MedicalAllowance + A.FoodAllowance + A.Conveyance) / @daysOfMonth))
			- (A.TotalAbsentDay * (A.BasicSalary / 30)) 
			- IIF(A.OTApplicable = 0, 0, (A.SubtractMinutes * (A.BasicSalary / (30 * 480))))
			+ IIF(A.AttendanceBonusApplicable = 0 OR A.TotalAbsentDay > 0 OR A.TotalLateDay > 0 OR A.TotalDay != @daysOfMonth OR A.MT > 0, 0, 500)
			+ IIF(A.OTApplicable = 0, 0, ISNULL((A.BasicSalary / (104 * 60)) * A.OTMinutes, 0)),

		A.TotalAbsentDay, 
		IIF(A.OTApplicable = 0, 0, (A.SubtractMinutes * (A.BasicSalary / (30 * 480)))) AS Food,
		0, 1
		FROM #DataTemp A
	END

	UPDATE EMP
	SET IsActive = 0
	FROM Employees EMP
	INNER JOIN InactiveEmployees IE ON EMP.EmployeeID = IE.EmployeeID
	WHERE ApplicableDate > @fromDate

	--SELECT * FROM MonthlySalaries WHERE [Month] = MONTH(@fromDate) AND [Year] = YEAR(@fromDate)
	SELECT * FROM MonthlySalaries WHERE CardNo = @cardNo

	DROP TABLE #AttendanceDataTemp
	DROP TABLE #AbsentDataTemp
	DROP TABLE #DataTemp
END

--EXEC CalculateSalary @fromDate = '2017-12-1', @toDate = '2017-12-31', @floor = '', @branchID = '1', @shiftID = '2', @cardNo = '3367731', @employeeType = 1

--EXEC CalculateSalary @fromDate = '2018-02-01', @toDate = '2018-02-28', @floor = '', @branchID = '1', @shiftID = '', @cardNo = '813400', @employeeType = 1
--EXEC CalculateSalary '2018-04-01', '2018-04-30','', '1','','3108519'
