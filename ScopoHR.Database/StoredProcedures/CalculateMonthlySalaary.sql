--IF EXISTS(SELECT * FROM Sys.objects WHERE name = 'CalculateMonthlySalary')
--BEGIN
--	DROP PROCEDURE CalculateMonthlySalary
--END
--GO
CREATE PROCEDURE [dbo].[CalculateMonthlySalary]( @year INT,@month INT, @minutes INT, @branchId INT, @preparedBy VARCHAR(20))
AS
	INSERT INTO MonthlySalaries
	([Month],EmployeeId,IsTaken,PreparedBy,PreparedDate, [Year],ModifiedBy,LastModified,IsDeleted, BranchID,Amount, SalaryType)		
	SELECT  @month AS [Month], EMP.EmployeeID, 0 AS IsTaken, @preparedBy AS PreparedBy, GETDATE() AS PreparedBy,
		@year AS [Year], @preparedBy AS ModifiedBy,GETDATE() AS LastModified,
		0 AS IsDeleted, @branchId AS BranchID,
		CASE 
			WHEN STP.SalaryTypeID = 3
				THEN OVT.OverTime*SMP.Amount
			WHEN STP.SalaryTypeID = 1
				THEN CAST(SMP.Amount-(SUB.AbsentDays*SMP.Amount/26) AS FLOAT)
			WHEN STP.SalaryTypeID = 4 
				THEN IIF(SUB.AbsentDays > 0, SMP.Amount * 0, SMP.Amount)
			ELSE SMP.Amount
			END AS Amount,
		STP.SalaryTypeID AS SalaryType
	FROM dbo.GetMonthlyOverTime(2017, 03, 1) OVT
	RIGHT JOIN Employees EMP ON OVT.CardNo = EMP.CardNo	
	LEFT JOIN Designations DSG ON EMP.DesignationID = DSG.DesignationID
	LEFT JOIN SalaryMappings SMP ON EMP.EmployeeID = SMP.EmployeeID
	LEFT JOIN SalaryTypes STP ON SMP.SalaryTypeID = STP.SalaryTypeID
	LEFT JOIN (
		SELECT CardNo, COUNT(*) AS AbsentDays FROM DailyAttendances
		WHERE Status = 'UP' OR Status = 'A' AND DATEPART(YEAR, Date) = @year AND DATEPART(MONTH, Date) = @month
		GROUP BY CardNo
	) SUB ON EMP.CardNo = SUB.CardNo
