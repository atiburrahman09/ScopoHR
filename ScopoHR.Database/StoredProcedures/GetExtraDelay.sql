--IF EXISTS(SELECT * FROM Sys.objects WHERE name = 'GetExtraDelay')
--BEGIN
--	DROP PROCEDURE GetAttendanceReport
--END
--GO


CREATE PROCEDURE [dbo].[GetExtraDelay]
	@fromDate DATETIME,
	@floor VARCHAR(MAX),
	@shiftID INT
AS
BEGIN
	SET NOCOUNT ON;
    
	SELECT 
	DATENAME(DW,CAST(AT.InOutTime as Date)) as [Day],
	CAST(AT.InOutTime AS DATE) AS [Date],
	EMP.CardNo, EMP.EmployeeName,
	CONVERT(VARCHAR(5), CAST(MIN(AT.InOutTime) as TIME),108) as InTime,
	CONVERT(VARCHAR(5), IIF(MIN(AT.InOutTime) = MAX(AT.InOutTime), NULL, CAST(MAX(AT.InOutTime) AS TIME)),108) AS OutTime,
	DATEDIFF(MINUTE, CAST(MIN(AT.InOutTime) as TIME), CAST(MAX(AT.InOutTime) as TIME)) AS TotalMinutes,
	DATEDIFF(MINUTE, CAST(WS.InTime as TIME), CAST(MIN(AT.InOutTime) as TIME)) AS LateMinutes
	FROM Employees EMP
	INNER JOIN WorkingShifts WS ON EMP.ShiftId = WS.Id 
	INNER JOIN CardNoMappings CM ON EMP.CardNo = CM.OriginalCardNo
	INNER JOIN Attendances AT ON CM.GeneratedCardNo = AT.CardNo
	LEFT JOIN ProductionFloorLines P on EMP.ProductionFloorLineID = P.ProductionFloorLineID
	WHERE EMP.IsActive = 1 
	AND CAST(AT.[InOutTime] as Date) = CAST(@fromDate as Date)
	AND EMP.ShiftId=IIF(@shiftId='',EMP.ShiftId,@shiftId)
	AND P.Floor=IIF(@floor='',p.Floor,@floor)
	GROUP BY CAST(AT.InOutTime AS DATE), EMP.CardNo, EMP.EmployeeName, WS.InTime, WS.OutTime
	HAVING DATEDIFF(MINUTE, CAST(WS.InTime as TIME), CAST(MIN(AT.InOutTime) as TIME)) >=240
	AND 
	MIN(AT.InOutTime) != MAX(AT.InOutTime)
	ORDER BY EMP.CardNo


END
