USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetEarnedLeaveCalculationReport]    Script Date: 05/20/2018 2:46:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetEarnedLeaveCalculationReport]
	@floor VARCHAR(200) = '',
	@employeeType INT = 1
AS
BEGIN
	SET NOCOUNT ON;

	--SELECT EM.CardNo,EM.EmployeeName,DG.DesignationName,EM.JoinDate,P.Floor,EL.TotalAttendance,EL.PvsELBalance,EL.ELGathered,EL.Relished,EM.EmployeeType,(M.GrossWage - ISNULL(M.AbsentDays * (M.Basic/30), 0)  - ISNULL(M.Food, 0)) As Salary
	--FROM EarnLeaveData EL
	--INNER JOIN Employees EM ON CAST(EL.CardNo AS nvarchar)=CAST(EM.CardNo as nvarchar)
	--INNER JOIN ProductionFloorLines P ON EM.ProductionFloorLineID=P.ProductionFloorLineID 
	--INNER JOIN Departments D ON EM.DepartmentID=D.DepartmentID
	--INNER JOIN Designations DG ON EM.DesignationID=DG.DesignationID
	--INNER JOIN MonthlySalaries M ON EM.CardNo=M.CardNo
	--WHERE M.Month=12
	--AND p.Floor=@floor
	--AND EM.EmployeeType=@employeeType
	----AND D.DepartmentID=6
	----GROUP BY EM.EmployeeType,P.Floor,EM.CardNo,EM.EmployeeName,DG.DesignationName,EM.JoinDate,EL.TotalAttendance,EL.PvsELBalance,EL.ELGathered,EL.Relished,M.PayableWages, M.AttendanceBonus
	--ORDER BY EM.CardNo
	SELECT EM.CardNo,EM.EmployeeName,DG.DesignationName,EM.JoinDate,P.Floor,EL.TotalAttendance,EL.PvsELBalance,EL.ELGathered,EL.Relished,EM.EmployeeType,(M.GrossWage - ISNULL(M.AbsentDays * (M.Basic/30), 0)  - ISNULL(M.Food, 0)) As Salary
	FROM EarnLeaveData EL
	LEFT JOIN Employees EM ON EL.EmployeeID=EM.EmployeeID
	LEFT JOIN ProductionFloorLines P ON EM.ProductionFloorLineID=P.ProductionFloorLineID 
	LEFT JOIN Departments D ON EM.DepartmentID=D.DepartmentID
	LEFT JOIN Designations DG ON EM.DesignationID=DG.DesignationID
	LEFT JOIN MonthlySalaries M ON CAST(EL.EmployeeID AS nvarchar)=M.EmployeeId
	WHERE EL.Month=4
	AND M.Month=3
	AND p.Floor=@floor
	AND EM.EmployeeType=@employeeType
	--AND D.DepartmentID=6
	--GROUP BY EM.EmployeeType,P.Floor,EM.CardNo,EM.EmployeeName,DG.DesignationName,EM.JoinDate,EL.TotalAttendance,EL.PvsELBalance,EL.ELGathered,EL.Relished,M.PayableWages, M.AttendanceBonus
	ORDER BY EM.CardNo

END

--EXEC GetEarnedLeaveCalculationReport @floor='3rd Way -4', @employeeType=1

