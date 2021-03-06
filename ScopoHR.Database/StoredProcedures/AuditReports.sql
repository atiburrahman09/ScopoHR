--IF EXISTS(SELECT * FROM sys.objects WHERE name='AuditReportDetail')
--BEGIN
--	DROP PROC AuditReportDetail
--END
--GO

CREATE PROCEDURE [dbo].[AuditReportDetail] (@fromDate AS DATETIME, @toDate AS DATETIME, @branchId AS INT,@shiftId as int, @floor as nvarchar(max))
AS 
BEGIN
	CREATE TABLE #Data
	(
		Day	nvarchar(50),
		Date	date,
		EmployeeID INT NULL,
		CardNo	nvarchar(200),
		EmployeeName	nvarchar(500),
		InTime	varchar(20),
		InTimeDate	DATETIME,
		OutTime	varchar(20),
		OutTimeDate	DATETIME,
		Status	varchar(5),
		TotalMinutes int,
		OTMinutes	int,
		LunchTime	int
	)

	INSERT INTO #Data
	EXEC GetAttendanceData @fromDate, @toDate, @floor, @branchId, @shiftId, ''

	SELECT A.[Date] AS AttendanceDate, A.CardNo, A.EmployeeName, D.DepartmentName AS Section, DG.DesignationName, F.[Floor], WS.[Name] AS 'Shift',
	A.InTime AS EntryTime, A.OutTime AS ExitTime, IIF(A.TotalMinutes IS NOT NULL AND A.TotalMinutes > 60, A.TotalMinutes, 0) AS TotalMinutes,
	ROUND(CAST((IIF(A.TotalMinutes IS NOT NULL AND A.TotalMinutes > 0, A.TotalMinutes, 0)) AS FLOAT) / 60 , 2) AS WorkingHours, ROUND(CAST(A.OTMinutes AS FLOAT) / 60, 2) AS OTHours
	FROM #Data A
	INNER JOIN Employees E ON A.EmployeeID = E.EmployeeID
	LEFT JOIN Departments D ON E.DepartmentID = D.DepartmentID
	LEFT JOIN Designations DG ON E.DesignationID = DG.DesignationID
	LEFT JOIN ProductionFloorLines F ON E.ProductionFloorLineID = F.ProductionFloorLineID
	LEFT JOIN WorkingShifts WS ON E.ShiftId = WS.Id
	ORDER BY F.[Floor], A.CardNo, A.[Date]

	DROP TABLE #Data
END

--EXEC AuditReportDetail '11/18/2017', '11/24/2017','1', '1',''
