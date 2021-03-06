--IF EXISTS(SELECT * FROM Sys.objects WHERE name = 'SyncInTime')
--BEGIN
--	DROP PROCEDURE SyncInTime
--END
--GO
CREATE PROC [dbo].[SyncInTime] @branchId INT, @date DATETIME, @shiftId VARCHAR(1), @modifiedBy VARCHAR(MAX)
AS
	DECLARE @hdStatus VARCHAR(2) = dbo.GetHolidayStatus(@date);
	
IF @shiftId = '1' OR @shiftId = '2'
BEGIN
INSERT INTO DailyAttendances
([Date], InTime, CardNo, OutTime, [Status], OverTime, ModifiedBy, LastModified, IsDeleted, EmployeeId)
	SELECT  CAST(@date AS DATETIME) AS [Date], 
	IIF(ATT.InTime IS NULL, NULL, ATT.InTime) AS InTime, EM.CardNo, NULL AS OutTime,
	IIF(@hdStatus IS NOT NULL, @hdStatus, 
	IIF(ATT.InTime IS NULL, 
		--check leave app
		IIF(LApp.Status = 1, IIF(LType.IsPayable > 0, 'PL', 'UP'), 'A'),
		-- else check the timing
		IIF(
		DATEDIFF(MINUTE,  CAST( WS.InTime AS TIME), CAST(ATT.InTime AS TIME)) <= 5 AND DATEDIFF(MINUTE,  CAST( WS.InTime AS TIME), CAST(ATT.InTime AS TIME)) >= -59, 'P',
		IIF(
			DATEDIFF(MINUTE,  CAST( WS.InTime AS TIME), CAST(ATT.InTime AS TIME)) <= 59 AND DATEDIFF(MINUTE,  CAST( WS.InTime AS TIME), CAST(ATT.InTime AS TIME)) >= 6, 'D',
			IIF(
			DATEDIFF(MINUTE,  CAST( WS.InTime AS TIME), CAST(ATT.InTime AS TIME)) <= 239 AND DATEDIFF(MINUTE,  CAST( WS.InTime AS TIME), CAST(ATT.InTime AS TIME)) >= 60, 'ED',
			'A'
		)
		)
	)
	)
	) AS Status, 0 AS OverTime, @modifiedBy AS ModifiedBy, GETDATE() AS LastModified, 0 AS IsDeleted, EM.EmployeeID
	FROM Employees EM
	INNER JOIN CardNoMappings CM ON EM.CardNo = CM.OriginalCardNo
	LEFT JOIN Designations DS ON EM.DesignationID = DS.DesignationID
	LEFT JOIN (
		SELECT ATT.CardNo, MIN(ATT.InOutTime) AS InTime 
		FROM Attendances ATT
		WHERE CAST(@date AS DATE) = CAST(ATT.InOutTime AS DATE)
		GROUP BY ATT.CardNo
	) ATT ON ATT.CardNo = CM.GeneratedCardNo

	LEFT JOIN (
		SELECT LApp.Status, LApp.EmployeeID, LApp.LeaveTypeID FROM LeaveApplications LApp
		WHERE CAST(@date AS DATE) >= CAST(LApp.FromDate AS DATE) AND CAST(@date AS DATE) <= CAST(LApp.ToDate AS DATE)	
	) LApp ON LApp.EmployeeID = EM.EmployeeID
	LEFT JOIN LeaveTypes Ltype ON LApp.LeaveTypeID = Ltype.LeaveTypeID
	LEFT JOIN WorkingShifts WS ON WS.Id = @shiftId
	WHERE EM.ShiftId = @shiftId AND EM.IsActive = 1 AND DS.DesignationName != 'Security Guard'
	AND DS.DesignationName != 'ASIC'AND DS.DesignationName != 'SIC'

	ORDER BY CardNo
END

ELSE

BEGIN
INSERT INTO DailyAttendances
([Date], InTime, CardNo, OutTime, [Status], OverTime, ModifiedBy, LastModified, IsDeleted, EmployeeId)
	SELECT  CAST(@date AS DATETIME) AS [Date], 
	IIF(ATT.InTime IS NULL, NULL, ATT.InTime) AS InTime, EM.CardNo, NULL AS OutTime,

	IIF(ATT.InTime IS NOT NULL, 
	-- check statu
		IIF(
		DATEDIFF(MINUTE,  CAST( WS.InTime AS TIME), CAST(ATT.InTime AS TIME)) <= 5 AND DATEDIFF(MINUTE,  CAST( WS.InTime AS TIME), CAST(ATT.InTime AS TIME)) >= -59, 'P',
		IIF(
			DATEDIFF(MINUTE,  CAST( WS.InTime AS TIME), CAST(ATT.InTime AS TIME)) <= 59 AND DATEDIFF(MINUTE,  CAST( WS.InTime AS TIME), CAST(ATT.InTime AS TIME)) >= 6, 'D',
			IIF(
			DATEDIFF(MINUTE,  CAST( WS.InTime AS TIME), CAST(ATT.InTime AS TIME)) <= 239 AND DATEDIFF(MINUTE,  CAST( WS.InTime AS TIME), CAST(ATT.InTime AS TIME)) >= 60, 'ED',
			'A'
		)
		)
	)
	,
	-- check holiday
		IIF(@hdStatus IS NOT NULL, @hdStatus, 		
		-- check leave app
		IIF(LApp.Status = 1, IIF(LType.IsPayable > 0, 'PL', 'UP'), 'A')
		)
	)
	 AS Status, 0 AS OverTime, @modifiedBy AS ModifiedBy, GETDATE() AS LastModified, 0 AS IsDeleted, EM.EmployeeID

	FROM Employees EM
	INNER JOIN Designations DS ON EM.DesignationID = DS.DesignationID
		INNER JOIN CardNoMappings CM ON EM.CardNo = CM.OriginalCardNo
	LEFT JOIN (
		SELECT ATT.CardNo, MIN(ATT.InOutTime) AS InTime 
		FROM Attendances ATT
		WHERE CAST(@date AS DATE) = CAST(ATT.InOutTime AS DATE)
		GROUP BY ATT.CardNo
	) ATT ON ATT.CardNo = CM.GeneratedCardNo

	LEFT JOIN (
		SELECT LApp.Status, LApp.EmployeeID, LApp.LeaveTypeID FROM LeaveApplications LApp
		WHERE CAST(@date AS DATE) >= CAST(LApp.FromDate AS DATE) AND CAST(@date AS DATE) <= CAST(LApp.ToDate AS DATE)	
	) LApp ON LApp.EmployeeID = EM.EmployeeID
	LEFT JOIN LeaveTypes Ltype ON LApp.LeaveTypeID = Ltype.LeaveTypeID
	LEFT JOIN WorkingShifts WS ON WS.Id = @shiftId

	INNER JOIN SecurityGuardRosters SGR ON EM.EmployeeID = SGR.EmployeeId
	WHERE (DS.DesignationName = 'Security Guard' OR DS.DesignationName = 'ASIC' OR DS.DesignationName = 'SIC')
	AND SGR.ShiftId = @shiftId AND EM.IsActive = 1 AND CAST(SGR.WorkingDate AS DATE) = CAST(@date AS DATE)

	ORDER BY CardNo
END
