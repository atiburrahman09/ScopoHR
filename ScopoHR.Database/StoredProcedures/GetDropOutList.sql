--IF EXISTS(SELECT * FROM Sys.objects WHERE name = 'GetDropOutList')
--BEGIN
--	DROP PROCEDURE GetDropOutList
--END
--GO

CREATE PROCEDURE [dbo].[GetDropOutList]
	@fromDate DATETIME,
	@shiftId VARCHAR(10) = '',
	@floor VARCHAR(20) = ''
AS
BEGIN
	SET NOCOUNT ON;

	SELECT C.EmployeeID, CAST(MAX(A.InOutTime) AS DATE) AS LastPresentDate
    INTO #EmpLastPresentDate
    FROM Attendances A
	INNER JOIN CardNoMappings B ON A.CardNo = B.GeneratedCardNo
	INNER JOIN Employees C ON B.OriginalCardNo = C.CardNo
	LEFT JOIN Designations D ON C.DesignationID = D.DesignationID
    INNER JOIN ProductionFloorLines F ON C.ProductionFloorLineID = F.ProductionFloorLineID
	WHERE C.IsActive = 1
    GROUP BY C.EmployeeID
    HAVING CAST(MAX(A.InOutTime) AS DATE) <> CAST(@fromDate AS DATE)

	SELECT A.EmployeeID, Count(*) as TotalLeave
    INTO #LeaveCount
    FROM #EmpLastPresentDate A
    INNER JOIN LeaveApplications LA ON A.EmployeeID = LA.EmployeeID
    WHERE A.LastPresentDate BETWEEN LA.FromDate AND LA.ToDate
    GROUP BY A.EmployeeID

	SELECT C.EmployeeName, A.EmployeeID, C.CardNo, C.JoinDate, D.DesignationName, F.Floor, 
	A.LastPresentDate AS AbsentFrom, (DATEDIFF(Day,CAST(LastPresentDate as date), CAST(@fromDate as date)) - ISNULL(B.TotalLeave, 0))  AS AbsentDays
	FROM #EmpLastPresentDate A
	INNER JOIN Employees C ON A.EmployeeID = C.EmployeeID
	INNER JOIN CardNoMappings CM ON C.CardNo = CM.OriginalCardNo
	LEFT JOIN Designations D ON C.DesignationID = D.DesignationID
    INNER JOIN ProductionFloorLines F ON C.ProductionFloorLineID = F.ProductionFloorLineID
	LEFT JOIN #LeaveCount B ON A.EmployeeID = B.EmployeeID	
	
	WHERE (DATEDIFF(Day,CAST(LastPresentDate as date), CAST(@fromDate as date)) - ISNULL(B.TotalLeave, 0)) > 2

	DROP TABLE #EmpLastPresentDate
	DROP TABLE #LeaveCount
END

--EXEC [dbo].[GetDropOutList] '2017-08-16'
--EXEC GetDropOutList '11/2/2017','','1'
