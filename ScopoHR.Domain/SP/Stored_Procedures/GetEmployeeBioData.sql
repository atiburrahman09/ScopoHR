USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetEmployeeBioData]    Script Date: 7/15/2018 10:55:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[GetEmployeeBioData] (@cardNo as nvarchar(max))
AS 


	SELECT E.CardNo, ST.SalaryTypeName,SM.Amount,E.EmployeeName,E.MobileNo,D.DepartmentName,Desg.DesignationName,E.BloodGroup,
	E.DateOfBirth,E.FatherName,E.GenderID,E.JoinDate,E.GrandFatherName,E.MaritalStatus,E.MotherName,E.NID,E.NomineeName,E.PermanentAddress,E.PresentAddress,E.SpouseName,E.SalaryGrade
	INTO #temp
	FROM Employees E
	INNER JOIN Departments D ON E.DepartmentID=D.DepartmentID
	INNER JOIN Designations Desg ON E.DesignationID= Desg.DesignationID
	LEFT JOIN SalaryMappings SM ON E.EmployeeID=SM.EmployeeID
	LEFT JOIN SalaryTypes ST ON SM.SalaryTypeID=ST.SalaryTypeID
	WHERE E.CardNo=@cardNo
	ORDER BY E.CardNo


	SELECT CardNo,EmployeeName,MobileNo,DepartmentName,DesignationName,[Basic Salary],[Conveyance],[Food Allowance],[Medical Allowance],[House Rent],BloodGroup,
	DateOfBirth,FatherName,GenderID,JoinDate,GrandFatherName,MaritalStatus,MotherName,NID,NomineeName,PermanentAddress,PresentAddress,SpouseName,SalaryGrade
	INTO #Details
	FROM #temp
	PIVOT(max(Amount) FOR SalaryTypeName in ([Basic Salary],[Conveyance],[Food Allowance],[Medical Allowance],[House Rent]))	 AS P2 ;


	SELECT CardNo,EmployeeName,MobileNo,DepartmentName,DesignationName,[Basic Salary] as BasicSalary,[Conveyance] as Conveyance,[Food Allowance] as FoodAllowance,[Medical Allowance] as MedicalAllowance,[House Rent] as HouseRent,
	DateOfBirth,FatherName,GenderID,JoinDate,GrandFatherName,MaritalStatus,MotherName,NID,NomineeName,PermanentAddress,PresentAddress,SpouseName,BloodGroup,SalaryGrade
	FROM #Details
	ORDER BY CardNo ASC


DROP TABLE #temp
DROP TABLE #Details


--EXEC GetEmployeeBioData '1701103'
