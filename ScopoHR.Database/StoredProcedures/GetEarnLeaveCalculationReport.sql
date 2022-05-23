--IF EXISTS(SELECT * FROM sys.objects WHERE name='GetEarnedLeaveCalculationReport')
--BEGIN
--	DROP PROC GetEarnedLeaveCalculationReport
--END
--GO



CREATE PROCEDURE [dbo].[GetEarnedLeaveCalculationReport]
	@floor VARCHAR(200) = '',
	@employeeType INT = 1
AS
BEGIN
	SET NOCOUNT ON;

	SELECT EM.CardNo,EM.EmployeeName,DG.DesignationName,EM.JoinDate,P.Floor,EL.TotalAttendance,EL.PvsELBalance,EL.ELGathered,EL.Relished,EM.EmployeeType,(M.GrossWage - ISNULL(M.AbsentDays * (M.Basic/30), 0)  - ISNULL(M.Food, 0)) As Salary
	FROM EarnLeaveData EL
	INNER JOIN Employees EM ON CAST(EL.CardNo AS nvarchar)=CAST(EM.CardNo as nvarchar)
	INNER JOIN ProductionFloorLines P ON EM.ProductionFloorLineID=P.ProductionFloorLineID 
	INNER JOIN Departments D ON EM.DepartmentID=D.DepartmentID
	INNER JOIN Designations DG ON EM.DesignationID=DG.DesignationID
	INNER JOIN MonthlySalaries M ON EM.CardNo=M.CardNo
	WHERE EL.Month=1
	AND M.Month=12
	AND p.Floor=@floor
	AND EM.EmployeeType=@employeeType
	--AND D.DepartmentID=6
	--GROUP BY EM.EmployeeType,P.Floor,EM.CardNo,EM.EmployeeName,DG.DesignationName,EM.JoinDate,EL.TotalAttendance,EL.PvsELBalance,EL.ELGathered,EL.Relished,M.PayableWages, M.AttendanceBonus
	ORDER BY EM.CardNo

END

--EXEC GetEarnedLeaveCalculationReport @floor='3rd Way -4', @employeeType=1


--select * from MonthlySalaries
--where month = 1
--and CardNo='3234866'




