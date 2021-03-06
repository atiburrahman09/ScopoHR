USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetProgresiveReport]    Script Date: 05/20/2018 2:49:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetProgresiveReport] (@fromDate AS DATETIME, @toDate AS DATETIME, @branchId AS INT,@shiftId as int, @floor as nvarchar(max))
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

	SELECT A.CardNo, A.EmployeeName, D.DepartmentName AS Section, DG.DesignationName AS Designation, F.[Floor], WS.[Name] AS 'Shift',
	CAST(SUM(A.TotalMinutes) as float) AS WorkingHours
	
	FROM #Data A
	INNER JOIN Employees E ON A.EmployeeID = E.EmployeeID
	LEFT JOIN Departments D ON E.DepartmentID = D.DepartmentID
	LEFT JOIN Designations DG ON E.DesignationID = DG.DesignationID
	LEFT JOIN ProductionFloorLines F ON E.ProductionFloorLineID = F.ProductionFloorLineID
	LEFT JOIN WorkingShifts WS ON E.ShiftId = WS.Id
	GROUP BY A.CardNo, A.EmployeeName, D.DepartmentName, DG.DesignationName,F.Floor,WS.Name
	--ORDER BY A.TotalMinutes DESC

	DROP TABLE #Data
END

--EXEC GetProgresiveReport '01/01/2018', '01/06/2018','1', '1','3rd Way -1'
