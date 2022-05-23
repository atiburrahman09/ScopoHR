--IF EXISTS(SELECT * FROM sys.objects WHERE name='GetNewRecruitmentList')
--BEGIN
--	DROP PROC GetNewRecruitmentList
--END
--GO


CREATE PROCEDURE [dbo].[GetNewRecruitmentList] (
	@fromDate datetime,
	@toDate datetime,
	@floor VARCHAR(200) = '',
	@branchID INT,
	@shiftID VARCHAR(10) = '',
	@cardNo VARCHAR(15) = '',
	@departmentID INT
)
AS 


	SELECT E.CardNo, ST.SalaryTypeName,SM.Amount,E.EmployeeName,Desg.DesignationName,E.JoinDate,E.SalaryGrade
	INTO #temp
	FROM Employees E
	INNER JOIN Departments D ON E.DepartmentID=D.DepartmentID
	INNER JOIN Designations Desg ON E.DesignationID= Desg.DesignationID
	INNER JOIN ProductionFloorLines P ON E.ProductionFloorLineID=P.ProductionFloorLineID 
	LEFT JOIN SalaryMappings SM ON E.EmployeeID=SM.EmployeeID
	LEFT JOIN SalaryTypes ST ON SM.SalaryTypeID=ST.SalaryTypeID
	WHERE 
	--E.IsActive=1
	E.CardNo NOT LIKE '14%' 
	AND P.[Floor] = IIF(@floor='', P.[Floor], @floor)
	AND E.ShiftId=IIF(@ShiftId='',E.ShiftId,@ShiftId)
	AND CAST(E.JoinDate as DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)
	GROUP BY E.CardNo,ST.SalaryTypeName,SM.Amount,E.EmployeeName,Desg.DesignationName,E.JoinDate,E.SalaryGrade
	ORDER BY E.CardNo


	SELECT CardNo,EmployeeName,JoinDate,SalaryGrade,DesignationName,[Basic Salary],[Conveyance],[Food Allowance],[Medical Allowance],[House Rent]
	INTO #Details
	FROM #temp
	PIVOT(max(Amount) FOR SalaryTypeName in ([Basic Salary],[Conveyance],[Food Allowance],[Medical Allowance],[House Rent]))AS P2 ;


	SELECT CardNo,EmployeeName,DesignationName,JoinDate,SalaryGrade,[Basic Salary] as BasicSalary,[Conveyance] as Conveyance,[Food Allowance] as FoodAllowance,[Medical Allowance] as MedicalAllowance,[House Rent] as HouseRent
	FROM #Details
	ORDER BY CardNo ASC


DROP TABLE #temp
DROP TABLE #Details
