USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetOvertimeReport]    Script Date: 05/20/2018 2:49:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetOvertimeReport]
	@fromDate DATETIME,
	@toDate DATETIME,
	@floor VARCHAR(200) = '',
	@branchID INT,
	@shiftID VARCHAR(10) = '',
	@cardNo VARCHAR(15) = ''
AS
BEGIN
	SET NOCOUNT ON;

	CREATE TABLE #TempAttendance(
		[Day] [varchar](20) NULL,
		[Date] [date] NULL,
		[CardNo] [varchar](20) NULL,
		[EmployeeName] [varchar](200) NULL,
		[InTime] [varchar](20) NULL,
		[OutTime] [varchar](20) NULL,
		[Status] VARCHAR(10),
		[TotalMinutes] [int] NULL,
		[OTMinutes] [int] NULL,
		[LunchTime] [int] NULL
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
	INSERT INTO #TempAttendance
		SELECT A.Day, A.Date,A.CardNo, A.EmployeeName, A.InTime, A.OutTime, A.Status, A.TotalMinutes AS WorkingHours, A.OTMinutes, A.LunchTime
		FROM ArchiveData A
		INNER JOIN Employees E ON A.EmployeeID = E.EmployeeID
		INNER JOIN ProductionFloorLines P ON P.ProductionFloorLineID=E.ProductionFloorLineID
		WHERE CAST(A.Date As DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)
		AND P.Floor=IIF(@floor = '', P.Floor, @floor)
		AND E.CardNo = IIF(@cardNo = '', E.CardNo, @cardNo)
		AND E.ShiftId = IIF(@shiftID = '', E.ShiftID, @shiftID)
	--RETURN;
	END

	ELSE
	BEGIN
		INSERT INTO #TempAttendance
		EXEC GetAttendanceData @fromDate, @toDate, @floor, @branchID, @shiftID, @cardNo
	END
	


	SELECT CardNo AS OriginalCardNo, EmployeeName, InTime, OutTime, OTMinutes AS TotalHour 
	FROM #TempAttendance WHERE OTMinutes IS NOT NULL AND OTMinutes <> 0
END

--EXEC GetOvertimeReport '2017-08-01', '2017-08-13','', '1','',''
