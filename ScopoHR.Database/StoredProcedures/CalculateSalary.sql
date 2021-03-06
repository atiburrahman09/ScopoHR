--IF EXISTS(SELECT * FROM Sys.objects WHERE name = 'CalculateSalary')
--BEGIN
--	DROP PROCEDURE CalculateSalary
--END
--GO

CREATE PROCEDURE [dbo].[CalculateSalary]
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

	CREATE TABLE #AttendanceData(
		[Day] [varchar](20) NULL,
		[Date] [date] NULL,
		[EmployeeID] [int] NULL,
		[CardNo] [varchar](20) NULL,
		[EmployeeName] [varchar](200) NULL,
		[InTime] [varchar](20) NULL,
		[OutTime] [varchar](20) NULL,
		[Status] VARCHAR(10),
		[TotalMinutes] [int] NULL,
		[OTMinutes] [int] NULL,
		[LunchTime] [int] NULL
	)
	
	INSERT INTO #AttendanceData([Day], [Date], EmployeeID, CardNo, EmployeeName, InTime, OutTime, [Status], TotalMinutes, OTMinutes, LunchTime)
	EXEC GetAttendanceData @fromDate, @toDate, @floor, @branchID, @shiftID, @cardNo

	CREATE TABLE #AbsentData
	(
		EmployeeID INT,
		EmployeeName VARCHAR(500),
		CardNo VARCHAR(50),
		DesignationName VARCHAR(200),
		Floor VARCHAR(200),
		AbsentDate DATETIME,
		Status VARCHAR(10) NULL
	)

	INSERT INTO #AbsentData
	EXEC GetAbsentReport @fromDate, @toDate, @floor, @branchID, @shiftID, @cardNo, 0

	INSERT INTO #AttendanceData
	SELECT DATENAME(DW,CAST(A.AbsentDate AS Date)) as [Day], 
	CAST(A.AbsentDate AS DATE) AS Date,
	A.EmployeeID, A.CardNo, A.EmployeeName, '' AS InTime, '' AS OutTime, A.Status,
	NULL AS TotalMinutes, NULL AS OTMinutes, 60 AS LunchTime
	FROM #AbsentData A

	UPDATE A
	SET A.OTMinutes = ISNULL(A.OTMinutes, 0) + ISNULL(A.TotalMinutes, 0)
	FROM #AttendanceData A
	WHERE dbo.GetHolidayStatus(A.Date, A.EmployeeID, @departmentID) IN ('WH', 'HD') 
	AND InTime IS NOT NULL AND A.Status NOT IN ('WH', 'HD')

	SELECT AT.EmployeeID, AT.CardNo, AT.EmployeeName,
	DATEDIFF(DAY, @fromDate, @toDate) + 1 AS TotalDay,
	COUNT(CASE WHEN AT.Status = 'P' OR AT.Status = 'D' THEN 1 END) AS TotalPresentDay,
	COUNT(CASE WHEN AT.Status = 'WH' OR AT.Status = 'HD' THEN 1 END) AS TotalHoliDay,
	COUNT(CASE WHEN AT.Status = 'A' THEN 1 END) AS TotalAbsentDay,
	COUNT(CASE WHEN AT.Status = 'D'  THEN 1 END) AS TotalLateDay,
	COUNT(CASE WHEN AT.Status <> 'A'  THEN 1 END) AS TotalWorkingDay,
	COUNT(CASE WHEN AT.Status = 'CL'  THEN 1 END) AS CL,
	COUNT(CASE WHEN AT.Status = 'EL'  THEN 1 END) AS EL,
	COUNT(CASE WHEN AT.Status = 'ML'  THEN 1 END) AS ML,
	COUNT(CASE WHEN AT.Status = 'MT'  THEN 1 END) AS MT,
	COUNT(CASE WHEN AT.Status = 'CP' OR AT.Status = 'FL'  THEN 1 END) AS SH,
	SUM(AT.TotalMinutes) AS TotalMinutes, SUM(AT.OTMinutes) AS OTMinutes,
	(SELECT Amount FROM SalaryMappings WHERE EmployeeID = AT.EmployeeID AND SalaryTypeID = 1) AS BasicSalary,
	(SELECT Amount FROM SalaryMappings WHERE EmployeeID = AT.EmployeeID AND SalaryTypeID = 4) AS MedicalAllowance,
	(SELECT Amount FROM SalaryMappings WHERE EmployeeID = AT.EmployeeID AND SalaryTypeID = 5) AS FoodAllowance,
	(SELECT Amount FROM SalaryMappings WHERE EmployeeID = AT.EmployeeID AND SalaryTypeID = 7) AS Conveyance,
	(SELECT Amount FROM SalaryMappings WHERE EmployeeID = AT.EmployeeID AND SalaryTypeID = 6) AS HouseRent
	INTO #Data
	FROM #AttendanceData AT
	INNER JOIN Employees EMP ON AT.CardNo = EMP.CardNo
	GROUP BY AT.EmployeeID, AT.CardNo, AT.EmployeeName
	ORDER BY AT.CardNo

	DELETE FROM MonthlySalaries WHERE CardNo = @cardNo

	INSERT INTO MonthlySalaries(
		[Year], [Month], EmployeeId, [Name], CardNo, 
		[Basic], HouseRent, MedicalAllowance, Conveyance, FoodAllowance,
		GrossWage, 
		PD, HD, TWD, 
		CasualLeave, EarnedLeave, MedicalLeave, MaternityLeave, FLeave,
		AttendanceBonus, 
		PayableWages,
		OTH, OTRate, OTTaka, TotalPay, 
		AbsentDays, IsTaken, BranchID
	)
	SELECT 
	YEAR(@fromDate), MONTH(@fromDate), A.EmployeeID, A.EmployeeName, A.CardNo,
	A.BasicSalary, A.HouseRent, A.MedicalAllowance, A.Conveyance, A.FoodAllowance, 
	A.BasicSalary + A.HouseRent + A.MedicalAllowance + A.FoodAllowance + A.Conveyance,
	A.TotalPresentDay, A.TotalHoliDay, A.TotalWorkingDay,
	A.CL, A.EL, A.ML, A.MT, A.SH,
	IIF(A.TotalAbsentDay > 0 OR A.TotalLateDay > 0 OR A.TotalDay != 31, 0, 500),
	(A.TotalDay * ((A.BasicSalary + A.HouseRent + A.MedicalAllowance + A.FoodAllowance + A.Conveyance) / 31)) - (A.TotalAbsentDay * (A.BasicSalary / 30)) + IIF(A.TotalAbsentDay > 0 OR A.TotalLateDay > 0 OR A.TotalDay != 31, 0, 500),
	A.OTMinutes, A.BasicSalary / 104,
	(A.BasicSalary / (104 * 60)) * A.OTMinutes,
	A.TotalDay * ((A.BasicSalary + A.HouseRent + A.MedicalAllowance + A.FoodAllowance + A.Conveyance) / 31) - (A.TotalAbsentDay * (A.BasicSalary / 30)) + IIF(A.TotalAbsentDay > 0 OR A.TotalLateDay > 0 OR A.TotalDay != 31, 0, 500) + ISNULL((A.BasicSalary / (104 * 60)) * A.OTMinutes, 0),
	A.TotalAbsentDay, 0, 1
	AttendanceBonus
	FROM #Data A

	SELECT * FROM MonthlySalaries WHERE CardNo = @cardNo

	DROP TABLE #AttendanceData
	DROP TABLE #AbsentData
	DROP TABLE #Data
END

--EXEC CalculateSalary '2017-10-01', '2017-10-31','', '1','','1308300'