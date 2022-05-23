-- IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'SyncDailyAttendance')
--BEGIN
--	DROP PROC SyncDailyAttendance
--END
--GO
CREATE PROCEDURE SyncDailyAttendance @branchId INT, @date DATETIME, @modifiedBy VARCHAR(20)
AS
DECLARE @officeOutTime DATETIME = dbo.GetOfficeOutTime();
DECLARE @isHoliday BIT = dbo.IsHoliday(@date);
INSERT INTO DailyAttendances
([Date], CardNo, InTime, OutTime, [Status], OverTime, ModifiedBy, LastModified, IsDeleted)
SELECT CAST(@date AS DATETIME) AS [Date], EMP.CardNo, DTT.InTime, DTT.OutTime,
	IIF(@isHoliday > 0, 'H', 
		IIF(DTT.InTime IS NULL, 
			IIF(LApp.Status = 1, IIF(LType.IsPayable > 0, 'PL', 'UP'), 'A')
		, 
		  IIF( dbo.GetComparableTime(DTT.InTime)>= CAST('06:00' AS TIME) AND dbo.GetComparableTime(DTT.InTime) <= CAST('08:30' AS TIME), 'P', 
		  IIF(dbo.GetComparableTime(DTT.InTime)>= CAST('08:31' AS TIME) AND dbo.GetComparableTime(DTT.InTime) <= CAST('12:30' AS TIME), 'L', 
		  IIF(dbo.GetComparableTime(DTT.InTime) >= CAST('12:31' AS TIME), 'EL', 'A'
		  )
		))
	)) AS 'Status', 
	--IIF(DTT.OverTime !> 0 , 0 ,DTT.OverTime) AS OverTime, 
	IIF(DTT.OverTime IS NULL OR DTT.OverTime !> 0, 0, DTT.OverTime) AS OverTime,
	@modifiedBy AS ModifiedBy, GETDATE() AS LastModified, 0 AS IsDeleted  
	--FROM dbo.GetDailyAttendance(@branchId, @date) DTT
	FROM (
			SELECT EMP.CardNo,
		MIN(ATT.InOutTime) AS InTime, MAX(ATT.InOutTime) AS OutTime,
		CEILING(CONVERT(FLOAT,DATEDIFF(MINUTE,CAST(MIN(ATT.InOutTime) as datetime), CAST(MAX(ATT.InOutTime) as datetime)))/(CONVERT(FLOAT, 60))) - 1 - 8 AS OverTime		
		FROM Attendances ATT
	INNER JOIN CardNoMappings CM ON ATT.CardNo = CM.GeneratedCardNo
	INNER JOIN Employees EMP ON CM.OriginalCardNo = EMP.CardNo		
	WHERE EMP.BranchID = @branchId AND CAST(ATT.InOutTime AS DATE) = CAST(@date AS DATE)
	GROUP BY EMP.CardNo, CAST(ATT.InOutTime AS DATE)	
	) AS DTT 
RIGHT JOIN Employees EMP ON DTT.CardNo = EMP.CardNo
LEFT JOIN (
	SELECT * FROM LeaveApplications LApp
	WHERE CAST(@date AS DATE) >= CAST(LApp.FromDate AS DATE) AND CAST(@date AS DATE) <= CAST(LApp.ToDate AS DATE)
) LApp ON EMP.EmployeeID = LApp.EmployeeID
LEFT JOIN LeaveTypes LType ON LApp.LeaveTypeID = LType.LeaveTypeID
WHERE EMP.IsActive = 1
GO
