USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetLeaveDetailsByEmployee]    Script Date: 05/20/2018 2:47:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[GetLeaveDetailsByEmployee] (@cardNo as nvarchar(max))
AS 

		SELECT E.CardNo, E.EmployeeName, D.DesignationName,L.FromDate,L.ToDate , DATEDIFF(DAY,Cast(L.FromDate as Date),Cast(L.ToDate as Date)) + 1 as LeaveDays, 
		 LT.LeaveTypeName,E.JoinDate,ApplicationDate
		INTO #temp
		FROM Employees E
		LEFT JOIN Designations D ON E.DesignationID=D.DesignationID
		INNER JOIN LeaveApplications L ON E.EmployeeID=L.EmployeeID
		INNER JOIN LeaveMappings LM ON L.LeaveTypeID=LM.LeaveTypeID
		INNER JOIN  LeaveTypes LT ON L.LeaveTypeID=LT.LeaveTypeID		

		WHERE E.CardNo=@cardNo


	SELECT CardNo,EmployeeName,JoinDate,DesignationName,FromDate,ToDate,ApplicationDate,
	[Medical Leave],[Casual Leave],[Maternity Leave], [Without pay],[compensatory leave],[earned leave],[ShortLeave],[Substitute Holiday Leave]
	INTO #Details
	FROM #temp
	PIVOT(max(LeaveDays) FOR LeaveTypeName in ([Medical Leave],[Casual Leave],[Maternity Leave], [Without pay],[compensatory leave],[earned leave],[ShortLeave],[Substitute Holiday Leave]))AS P2 ;


	SELECT  CardNo,EmployeeName,DesignationName,JoinDate,FromDate,ToDate,ApplicationDate,[Medical Leave] as MedicalLeave,[Casual Leave] as CasualLeave,[Maternity Leave] as MaternityLeave,[Without pay] as WithoutPay,[compensatory leave] as compensatoryleave,[earned leave] AS earnedleave,[ShortLeave] AS SL,[Substitute Holiday Leave] AS SH
	FROM #Details D
	ORDER BY CardNo ASC




	--EXEC GetLeaveDetailsByEmployee '1313180'