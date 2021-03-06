USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetDropOutList]    Script Date: 05/20/2018 2:45:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[GetDropOutList]
	@fromDate DATETIME,	
	@floor VARCHAR(50) = '',
	@shiftId VARCHAR(10) = '',
	@days INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT C.EmployeeID, CAST(MAX(A.InOutTime) AS DATE) AS LastPresentDate
    INTO #EmpLastPresentDate
    FROM Attendances A
	INNER JOIN CardNoMappings B ON A.CardNo = B.GeneratedCardNo
	INNER JOIN Employees C ON B.OriginalCardNo = C.CardNo
	INNER JOIN WorkingShifts WS ON C.ShiftId = WS.Id
	INNER JOIN ProductionFloorLines F ON C.ProductionFloorLineID = F.ProductionFloorLineID
	WHERE C.IsActive = 1 AND C.CardNo NOT LIKE '14%'
	AND A.InOutTime > DATEADD(HOUR, -2, CAST(CAST(A.InOutTime AS DATE) AS DATETIME) + CAST(CAST(WS.InTime AS TIME) AS DATETIME))
	AND F.Floor=IIF(@floor = '', F.Floor, @floor)
	AND C.ShiftId = IIF(@shiftID = '',C.ShiftId,@shiftId)
    GROUP BY C.EmployeeID
    HAVING CAST(MAX(A.InOutTime) AS DATE) <> CAST(@fromDate AS DATE)

	--SELECT A.EmployeeID, Count(*) as TotalLeave
 --   INTO #LeaveCount
 --   FROM #EmpLastPresentDate A
 --   INNER JOIN LeaveApplications LA ON A.EmployeeID = LA.EmployeeID
 --   WHERE A.LastPresentDate BETWEEN LA.FromDate AND LA.ToDate
 --   GROUP BY A.EmployeeID

	 SELECT EmployeeID ,CAST(MAX(ToDate) AS DATE) AS LastLeaveDate
	 INTO #LastLeaveDate
	 FROM LeaveApplications LA
	 WHERE LA.IsDeleted=0
	 GROUP BY EmployeeID

	 UPDATE A
	 SET A.LastPresentDate =  B.LastLeaveDate
	 FROM #EmpLastPresentDate A
	 INNER JOIN #LastLeaveDate B ON A.EmployeeID = B.EmployeeID
	 WHERE B.LastLeaveDate > A.LastPresentDate

	 
	UPDATE A
	SET A.LastPresentDate=DATEADD(DAY,1,A.LastPresentDate)
	FROM #EmpLastPresentDate A
	WHERE dbo.GetHolidayStatus(DATEADD(DAY,1,A.LastPresentDate), A.EmployeeID, '') IN ('WH', 'HD')

	
	UPDATE A
	SET A.LastPresentDate=DATEADD(DAY,1,A.LastPresentDate)
	FROM #EmpLastPresentDate A
	WHERE dbo.GetHolidayStatus(DATEADD(DAY,1,A.LastPresentDate), A.EmployeeID, '') IN ('WH', 'HD')
	

	SELECT C.EmployeeName, A.EmployeeID, C.CardNo, C.JoinDate, D.DesignationName AS Designation,C.FatherName,DP.DepartmentName,IIF(C.GenderID = 1,'',C.SpouseName) As SpouseName,C.PermanentAddress,C.PresentAddress,
	DATEADD(DAY,1,A.LastPresentDate) AS AbsentFrom, (DATEDIFF(Day,CAST(A.LastPresentDate as date), CAST(@fromDate as date)))  AS AbsentDays
	FROM #EmpLastPresentDate A
	INNER JOIN Employees C ON A.EmployeeID = C.EmployeeID
	INNER JOIN CardNoMappings CM ON C.CardNo = CM.OriginalCardNo
	LEFT JOIN Designations D ON C.DesignationID = D.DesignationID
	LEFT JOIN Departments DP ON C.DepartmentID = DP.DepartmentID
	WHERE (DATEDIFF(Day,CAST(A.LastPresentDate as date), CAST(@fromDate as date))) >= @days
	
	AND C.IsActive=1
	ORDER BY C.CardNo
	DROP TABLE #EmpLastPresentDate
	DROP TABLE #LastLeaveDate
END

--EXEC [dbo].[GetDropOutList] '2017-08-16'
--EXEC GetDropOutList '11/2/2017','','1'
--EXEC GetDropOutList '5/14/2018','','1','7'
--EXEC GetDropOutList '4/22/2018','Night Shit All Common','2','10'
