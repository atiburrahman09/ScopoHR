USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetOneTimePunchReport]    Script Date: 05/20/2018 2:49:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetOneTimePunchReport]
	@fromDate DATETIME,
	@shiftID VARCHAR(10) = '',
	@floor VARCHAR(200) = ''
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #Data
	(
		Day	nvarchar(50),
		Date	date,
		EmployeeID int,
		CardNo	nvarchar(200),
		EmployeeName	nvarchar(500),
		InTime	varchar(20),
		InTimeDate datetime,
		OutTime	varchar(20),
		OutTimeDate datetime,
		Status	varchar(5),
		TotalMinutes int,
		OTMinutes	int,
		LunchTime	int
	)

	DECLARE @archiveCount int = (
	SELECT COUNT(*) FROM ArchiveMonth 
	WHERE ArchiveMonth=DATEPART(MM,@fromDate)
	AND ArchiveYear=DATEPART(YYYY,@fromDate)
	GROUP BY ID)

	

	IF(@archiveCount > 0)

	BEGIN
	INSERT INTO #Data
		SELECT A.Day, A.Date,A.EmployeeID, A.CardNo, A.EmployeeName, A.InTime,A.InTimeDate, A.OutTime, A.OutTimeDate,A.Status, A.TotalMinutes AS WorkingHours, A.OTMinutes, A.LunchTime
		FROM ArchiveData A
		INNER JOIN Employees E ON A.EmployeeID = E.EmployeeID
		INNER JOIN ProductionFloorLines P ON P.ProductionFloorLineID=E.ProductionFloorLineID
		WHERE CAST(A.Date As DATE) = CAST(@fromDate AS DATE) 
		AND P.Floor=IIF(@floor = '', P.Floor, @floor)
		--AND E.CardNo = IIF(@cardNo = '', E.CardNo, @cardNo)
		AND E.ShiftId = IIF(@shiftID = '', E.ShiftID, @shiftID)
	--RETURN;
	END

	ELSE 
	BEGIN
		INSERT INTO #Data
		EXEC GetAttendanceData @fromDate, @fromDate, @floor, 1, @shiftID, ''
	END
	
	

	SELECT D.CardNo, D.EmployeeName AS NameOfEmployee, DG.DesignationName , D.InTime, D.OutTime, @FromDate as [Date],@ShiftId as ShifId, @Floor as [Floor],D.InTimeDate, D.OutTimeDate
	FROM #Data D
	LEFT JOIN Employees EM ON EM.CardNo= D.CardNo
	LEFT JOIN Designations DG ON EM.DesignationID=DG.DesignationID
	WHERE (OutTime IS NULL
	OR DATEDIFF(MINUTE, InTimeDate, OutTimeDate) < 6)
	AND D.CardNo NOT LIKE '14%'
	ORDER BY CardNo

	DROP TABLE #Data
END

--EXEC GetOneTimePunchReport '2017-11-25', '1', ''



