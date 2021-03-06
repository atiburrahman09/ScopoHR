USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[AuditReportDetail]    Script Date: 05/20/2018 2:42:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[AuditReportDetail] (@fromDate AS DATETIME, @toDate AS DATETIME, @branchId AS INT,@shiftId as int, @floor as nvarchar(max))
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

	DECLARE @archiveCount int = (
	SELECT COUNT(*) FROM ArchiveMonth 
	WHERE ArchiveMonth=DATEPART(MM,@fromDate)
	AND ArchiveYear=DATEPART(YYYY,@fromDate)
	AND ArchiveMonth=DATEPART(MM,@toDate)
	AND ArchiveYear=DATEPART(YYYY,@toDate)
	GROUP BY ID)

	

	IF(@archiveCount > 0)

	BEGIN
		SELECT A.Day, A.Date,A.EmployeeID, A.CardNo, A.EmployeeName, A.InTime,A.InTimeDate, A.OutTime, A.OutTimeDate,A.Status, A.TotalMinutes AS WorkingHours, A.OTMinutes, A.LunchTime
		FROM ArchiveData A
		INNER JOIN Employees E ON A.EmployeeID = E.EmployeeID
		INNER JOIN ProductionFloorLines P ON P.ProductionFloorLineID=E.ProductionFloorLineID
		WHERE CAST(A.Date As DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)
		AND P.Floor=IIF(@floor = '', P.Floor, @floor)
		--AND E.CardNo = IIF(@cardNo = '', E.CardNo, @cardNo)
		AND E.ShiftId = IIF(@shiftID = '', E.ShiftID, @shiftID)
	RETURN;
	END

	INSERT INTO #Data
	EXEC GetAttendanceData @fromDate, @toDate, @floor, @branchId, @shiftId, ''

	SELECT A.[Date] AS AttendanceDate, A.CardNo, A.EmployeeName, D.DepartmentName AS Section, DG.DesignationName AS Designation, F.[Floor], WS.[Name] AS 'Shift',
	A.InTime AS EntryTime, A.OutTime AS ExitTime, IIF(A.TotalMinutes IS NOT NULL AND A.TotalMinutes > 60, A.TotalMinutes, 0) AS TotalMinutes,
	ROUND(CAST((IIF(A.TotalMinutes IS NOT NULL AND A.TotalMinutes > 0, A.TotalMinutes, 0)) AS FLOAT) / 60 , 2) AS WorkingHours, IIF(CAST(A.OTMinutes AS FLOAT) / 60 > 2,CAST(2 AS FLOAT),CAST(A.OTMinutes AS FLOAT) / 60) AS TwoHoursOT,
	ROUND(
	IIF(CAST(A.OTMinutes AS FLOAT) / 60 > 5, CAST(3 AS FLOAT),
	IIF(CAST(A.OTMinutes AS FLOAT) / 60 > 2,(CAST(A.OTMinutes AS FLOAT) / 60) - 2,CAST(0 AS FLOAT))),2) AS ExtraOT,
	ROUND(IIF(CAST(A.OTMinutes AS FLOAT) / 60 > 5,(CAST(A.OTMinutes AS FLOAT) / 60) - 5,CAST(0 AS FLOAT)),2) AS NightOT
	FROM #Data A
	INNER JOIN Employees E ON A.EmployeeID = E.EmployeeID
	LEFT JOIN Departments D ON E.DepartmentID = D.DepartmentID
	LEFT JOIN Designations DG ON E.DesignationID = DG.DesignationID
	LEFT JOIN ProductionFloorLines F ON E.ProductionFloorLineID = F.ProductionFloorLineID
	LEFT JOIN WorkingShifts WS ON E.ShiftId = WS.Id
	GROUP BY A.CardNo, A.EmployeeName, D.DepartmentName, DG.DesignationName,A.Date,F.Floor,WS.Name,A.InTime,A.OutTime,A.OTMinutes,A.TotalMinutes
	ORDER BY A.TotalMinutes DESC

	DROP TABLE #Data
END

--EXEC AuditReportDetail '12/01/2017', '12/31/2017','1', '1','3rd Way -1'

