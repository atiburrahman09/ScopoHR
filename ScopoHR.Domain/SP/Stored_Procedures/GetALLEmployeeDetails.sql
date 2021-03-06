USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetALLEmployeeDetails]    Script Date: 05/20/2018 2:43:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




ALTER PROCEDURE [dbo].[GetALLEmployeeDetails] (@fromDate as datetime,@toDate as datetime,@floor as nvarchar(max),@shiftId as int)
AS 


	SELECT E.CardNo, ST.SalaryTypeName,SM.Amount,LM.LeaveDays,LT.LeaveTypeName,E.EmployeeName,E.MobileNo,D.DepartmentName,Desg.DesignationName,p.[Floor],p.Line,E.JoinDate,E.TicketNo,E.ModifiedBy,E.LastModified,E.SalaryGrade As Grade
	INTO #temp
	FROM Employees E
	INNER JOIN Departments D ON E.DepartmentID=D.DepartmentID
	INNER JOIN Designations Desg ON E.DesignationID= Desg.DesignationID
	INNER JOIN ProductionFloorLines P ON E.ProductionFloorLineID=P.ProductionFloorLineID 
	LEFT JOIN SalaryMappings SM ON E.EmployeeID=SM.EmployeeID
	LEFT JOIN SalaryTypes ST ON SM.SalaryTypeID=ST.SalaryTypeID
	LEFT JOIN LeaveMappings LM ON E.EmployeeID=LM.EmployeeID
	LEFT JOIN LeaveTypes LT ON LM.LeaveTypeID=LT.LeaveTypeID
	WHERE E.IsActive=1
	AND P.[Floor] = IIF(@floor='', P.[Floor], @floor)
	AND E.ShiftId=IIF(@ShiftId='',E.ShiftId,@ShiftId)
	AND CAST(E.JoinDate as DATE) BETWEEN IIF(@fromDate='',CAST(E.JoinDate AS DATE),CAST(@fromDate AS DATE)) AND IIF(@toDate='',CAST(E.JoinDate AS DATE),CAST(@toDate AS DATE))
	GROUP BY E.CardNo,ST.SalaryTypeName,SM.Amount,LM.LeaveDays,LT.LeaveTypeName,E.EmployeeName,E.MobileNo,D.DepartmentName,Desg.DesignationName,P.[Floor],P.Line,E.JoinDate,E.TicketNo,E.ModifiedBy,E.LastModified,E.SalaryGrade
	ORDER BY E.CardNo


	SELECT CardNo,EmployeeName,MobileNo,DepartmentName,DesignationName,[Floor],Line,JoinDate,TicketNo,LastModified,ModifiedBy, [Over Time],[Attendance Bonus],[Basic Salary],[Conveyance],[Food Allowance],[Medical Allowance],[House Rent], [Medical Leave],[Casual Leave],[Maternity Leave], [Without pay],[compensatory leave],[earned leave],[ShortLeave],[Substitute Holiday Leave],Grade
	INTO #Details
	FROM #temp
	PIVOT(Max(LeaveDays) FOR LeaveTypeName in ([Medical Leave] ,[Casual Leave],[Maternity Leave], [Without pay],[compensatory leave],[earned leave],[ShortLeave],[Substitute Holiday Leave])) as P1 
	PIVOT(max(Amount) FOR SalaryTypeName in ([Over Time],[Attendance Bonus],[Basic Salary],[Conveyance],[Food Allowance],[Medical Allowance],[House Rent]))	 AS P2 ;


	SELECT CardNo,EmployeeName,MobileNo,DepartmentName,DesignationName,[Floor],Line,JoinDate,TicketNo,LastModified,ModifiedBy, [Over Time] as OverTime,[Attendance Bonus] as AttendanceBonus,[Basic Salary] as BasicSalary,[Conveyance] as Conveyance,[Food Allowance] as FoodAllowance,[Medical Allowance] as MedicalAllowance,[House Rent] as HouseRent, [Medical Leave] as MedicalLeave,[Casual Leave] as CasualLeave,[Maternity Leave] as MaternityLeave, [Without pay] as WithOutPay,[compensatory leave] as compensatoryleave,[earned leave] as earnedleave,[ShortLeave] AS SL,[Substitute Holiday Leave] AS SH,@floor as SearchFloor,Grade
	FROM #Details
	ORDER BY CardNo ASC


DROP TABLE #temp
DROP TABLE #Details

--EXEC GetALLEmployeeDetails '','','3rd Way -1', '1'
