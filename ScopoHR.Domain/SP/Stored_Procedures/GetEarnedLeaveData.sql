
ALTER PROCEDURE [dbo].[CalculateEarnedLeave]
	@floor VARCHAR(200) = '',
	@employeeType INT = 1,
	@month INT,
	@year INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Count int = (
	SELECT COUNT(*) FROM EarnLeaveData
	WHERE Month=@month
	AND Year=@year)

	IF(@Count = 0)
	BEGIN		
		CREATE TABLE #Data
		(
			EmployeeID INT,
			CardNo NVARCHAR(200),
			TotalAttendance INT
		)

		CREATE TABLE #ELData
		(
			EmployeeID INT,
			CardNo NVARCHAR(200),
			TotalELRelished INT
		)

		INSERT INTO #Data
		SELECT  E.EmployeeID, E.CardNo,COUNT(*) AS TotalAttendance	
		FROM ArchiveData D
		INNER JOIN Employees E ON D.EmployeeID = E.EmployeeID
		WHERE E.IsActive = 1
		AND D.Status IN ('P','D')
		AND D.Date BETWEEN CAST(CAST((@year-1) AS varchar) + '-' + CAST(DATEPART(MM,CAST(E.JoinDate AS DATE)) AS varchar) + '-' + CAST(DATEPART(DD,CAST(E.JoinDate AS DATE)) AS varchar) AS DATE) AND CAST(CAST(@year AS varchar) + '-' + CAST(DATEPART(MM,CAST(E.JoinDate AS DATE)) AS varchar) + '-' + CAST(DATEPART(DD,CAST(E.JoinDate AS DATE)) AS varchar) AS DATE)
		GROUP BY E.EmployeeID, E.CardNo, D.Status

		INSERT INTO #ELData
		SELECT  E.EmployeeID, E.CardNo,COUNT(*) AS TotalELRelished
		FROM ArchiveData D
		INNER JOIN Employees E ON D.EmployeeID = E.EmployeeID
		WHERE E.IsActive = 1
		AND D.Status IN ('EL')
		AND D.Date BETWEEN CAST(CAST((@year-1) AS varchar) + '-' + CAST(DATEPART(MM,CAST(E.JoinDate AS DATE)) AS varchar) + '-' + CAST(DATEPART(DD,CAST(E.JoinDate AS DATE)) AS varchar) AS DATE) AND CAST(CAST(@year AS varchar) + '-' + CAST(DATEPART(MM,CAST(E.JoinDate AS DATE)) AS varchar) + '-' + CAST(DATEPART(DD,CAST(E.JoinDate AS DATE)) AS varchar) AS DATE)
		GROUP BY E.EmployeeID, E.CardNo, D.Status

		IF @month = 12
		BEGIN
			INSERT INTO #Data
			SELECT  E.EmployeeID, E.CardNo,COUNT(*) AS TotalAttendance	
			FROM ArchiveData D
			INNER JOIN Employees E ON D.EmployeeID = E.EmployeeID
			WHERE E.IsActive = 1
			AND D.Status IN ('P','D')
			AND D.Date BETWEEN CAST(CAST((@year-1) AS varchar) + '-01-01' AS DATE) AND CAST(CAST(@year AS varchar) + '-12-31' AS DATE)
			GROUP BY E.EmployeeID, E.CardNo, D.Status


			INSERT INTO #ELData
			SELECT  E.EmployeeID, E.CardNo,COUNT(*) AS TotalELRelished
			FROM ArchiveData D
			INNER JOIN Employees E ON D.EmployeeID = E.EmployeeID
			WHERE E.IsActive = 1
			AND D.Status IN ('EL')
			AND D.Date BETWEEN CAST(CAST((@year-1) AS varchar) + '-' + CAST(DATEPART(MM,CAST(E.JoinDate AS DATE)) AS varchar) + '-' + CAST(DATEPART(DD,CAST(E.JoinDate AS DATE)) AS varchar) AS DATE) AND CAST(CAST(@year AS varchar) + '-' + CAST(DATEPART(MM,CAST(E.JoinDate AS DATE)) AS varchar) + '-' + CAST(DATEPART(DD,CAST(E.JoinDate AS DATE)) AS varchar) AS DATE)
			GROUP BY E.EmployeeID, E.CardNo, D.Status
		END

		INSERT INTO EarnLeaveData
		SELECT D.CardNo, D.EmployeeID, D.TotalAttendance, (EL.PvsELBalance + EL.ELGathered - EL.Relished), 
		ROUND(D.TotalAttendance/ 18, 0),ED.TotalELRelished, @month ,@year 
		FROM EarnLeaveData EL
		INNER JOIN #Data D ON D.EmployeeID=EL.EmployeeID
		LEFT JOIN #ELData ED ON D.EmployeeID=ED.EmployeeID
		INNER JOIN Employees E ON D.EmployeeID=E.EmployeeID
		WHERE DATEPART(MM,E.JoinDate) = @month
		AND (@year - DATEPART(YY,E.JoinDate)) > 1
		AND  DATEPART(YY,E.JoinDate) > 2014

		DROP TABLE #Data
		DROP TABLE #ELData
	END

	SELECT EM.CardNo,EM.EmployeeName,DG.DesignationName,EM.JoinDate,P.Floor,EL.TotalAttendance,EL.PvsELBalance,EL.ELGathered,EL.Relished,EM.EmployeeType,(M.GrossWage - ISNULL(M.AbsentDays * (M.Basic/30), 0)  - ISNULL(M.Food, 0)) As Salary
	FROM EarnLeaveData EL
	LEFT JOIN Employees EM ON EL.EmployeeID=EM.EmployeeID
	LEFT JOIN ProductionFloorLines P ON EM.ProductionFloorLineID=P.ProductionFloorLineID 
	LEFT JOIN Departments D ON EM.DepartmentID=D.DepartmentID
	LEFT JOIN Designations DG ON EM.DesignationID=DG.DesignationID
	LEFT JOIN MonthlySalaries M ON CAST(EL.EmployeeID AS nvarchar)=M.EmployeeId
	WHERE EL.Month=@month
	AND M.Month=@month-1
	AND EL.Year=@year
	AND p.Floor=@floor
	AND EM.EmployeeType=@employeeType
	ORDER BY EM.CardNo

END

--EXEC GetEarnedLeaveCalculationReport @floor='3rd Way -3', @employeeType=1

