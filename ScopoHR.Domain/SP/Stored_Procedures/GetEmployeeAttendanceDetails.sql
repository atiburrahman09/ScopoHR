USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetEmployeeAttendacneDetails]    Script Date: 05/20/2018 2:46:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetEmployeeAttendacneDetails] (@fromDate AS DATETIME, @toDate AS DATETIME, @branchId AS INT, @cardNo as nvarchar(max))
AS 
BEGIN
	DECLARE @lunchBreak INT = 1;
		SELECT 	DTT.Date AS AttendanceDate, DTT.CardNo AS CardNo, EM.EmployeeName AS EmployeeName,
		CONVERT(VARCHAR(5), CAST(DTT.InTime AS TIME),108) AS EntryTime, 
		CONVERT(VARCHAR(5), CAST(DTT.OutTime AS TIME),108) AS ExitTime,		
		(DATEDIFF(MINUTE, DTT.InTime, DTT.OutTime)/60) as WorkingHours,	
		DTT.OverTime, 
		IIF(DTT.Status = 'P' OR DTT.Status = 'E' OR DTT.Status = 'ED','60','') as LunchTime,
		DATENAME(DW,DTT.Date) as [Day], @fromDate as FromDate, @toDate as ToDate,
		IIF(DTT.Status = 'P' or DTT.Status = 'D' or DTT.Status = 'ED','',DTT.[Status]) as Holiday
	FROM DailyAttendances DTT
	INNER JOIN Employees EM ON DTT.CardNo = EM.CardNo
	WHERE Em.CardNo = @cardNo AND
	CAST(DTT.Date AS DATE) >= CAST(@fromDate AS DATE) AND CAST(DTT.Date AS DATE) <= CAST(@toDate AS DATE)

	ORDER BY AttendanceDate ASC
	END
