--IF EXISTS(SELECT * FROM Sys.objects WHERE name = 'SyncOutTime')
--BEGIN
--	DROP PROCEDURE SyncOutTime
--END
--GO


CREATE PROC [dbo].[SyncOutTime] @branchId INT, @date DATETIME, @shiftId VARCHAR(1), @modifiedBy VARCHAR(MAX)
AS
IF(@shiftId = '1' OR @shiftId = '2')
	BEGIN
		IF(@shiftId = '1')
		BEGIN
			UPDATE DTT SET DTT.OutTime = ATT.OutTime, 
			DTT.OverTime = IIF( CEILING(CONVERT(FLOAT, DATEDIFF(MINUTE, DTT.InTime, ATT.OutTime))/CONVERT(FLOAT, 60)) - 1- 8 > 0,
			CEILING(CONVERT(FLOAT, DATEDIFF(MINUTE, DTT.InTime, ATT.OutTime))/CONVERT(FLOAT, 60) - 1 - 8), 0
			 )
			 FROM DailyAttendances DTT
			 INNER JOIN Employees EM ON DTt.CardNo = EM.CardNo
			INNER JOIN CardNoMappings CM ON DTT.CardNo = CM.OriginalCardNo
			LEFT JOIN (
					SELECT ATT.CardNo, IIF(MAX(ATT.InOutTime) = MIN(ATT.InOutTime), NULL, MAX(ATT.InOutTime)) AS OutTime 
					FROM Attendances ATT
					WHERE CAST(@date AS DATE) = CAST(ATT.InOutTime AS DATE)
					GROUP BY ATT.CardNo
				) ATT ON CM.GeneratedCardNo = ATT.CardNo
			WHERE CAST(DTT.Date AS DATE) = CAST(@date AS DATE) AND EM.ShiftId = @shiftId AND EM.BranchID = @branchId
		END
		ELSE
		BEGIN
			UPDATE DTT SET DTT.OutTime = ATT.OutTime, 
			DTT.OverTime = IIF( CEILING(CONVERT(FLOAT, DATEDIFF(MINUTE, DTT.InTime, ATT.OutTime))/CONVERT(FLOAT, 60)) - 1- 8 > 0,
			CEILING(CONVERT(FLOAT, DATEDIFF(MINUTE, DTT.InTime, ATT.OutTime))/CONVERT(FLOAT, 60) - 1 - 8), 0
			 )
			 FROM DailyAttendances DTT
			 INNER JOIN Employees EM ON DTt.CardNo = EM.CardNo
			INNER JOIN CardNoMappings CM ON DTT.CardNo = CM.OriginalCardNo
			LEFT JOIN (
					SELECT ATT.CardNo, IIF(MAX(ATT.InOutTime) = MIN(ATT.InOutTime), NULL, MAX(ATT.InOutTime)) AS OutTime 
					FROM Attendances ATT
					WHERE CAST(@date AS DATE) = CAST(ATT.InOutTime AS DATE)
					GROUP BY ATT.CardNo
				) ATT ON CM.GeneratedCardNo = ATT.CardNo
			WHERE CAST(DTT.Date AS DATE) = CAST(DATEADD(DAY, -1, @date) AS DATE) AND EM.ShiftId = @shiftId AND EM.BranchID = @branchId
		END
	END 
ELSE
	BEGIN 
	IF(@shiftId = '5')
		BEGIN
		UPDATE DTT SET DTT.OutTime = ATT.OutTime, 
		DTT.OverTime = IIF( CEILING(CONVERT(FLOAT, DATEDIFF(MINUTE, DTT.InTime, ATT.OutTime))/CONVERT(FLOAT, 60)) - 1- 8 > 0,
		CEILING(CONVERT(FLOAT, DATEDIFF(MINUTE, DTT.InTime, ATT.OutTime))/CONVERT(FLOAT, 60) - 1 - 8), 0
		 )
		 FROM DailyAttendances DTT
		INNER JOIN Employees EM ON DTt.CardNo = EM.CardNo
		INNER JOIN SecurityGuardRosters SGR ON Em.EmployeeID = SGR.EmployeeId
		INNER JOIN CardNoMappings CM ON DTT.CardNo = CM.OriginalCardNo
		LEFT JOIN (
				SELECT ATT.CardNo, IIF(MAX(ATT.InOutTime) = MIN(ATT.InOutTime), NULL, MAX(ATT.InOutTime)) AS OutTime 
				FROM Attendances ATT
				WHERE CAST(@date AS DATE) = CAST(ATT.InOutTime AS DATE)
				GROUP BY ATT.CardNo
			) ATT ON CM.GeneratedCardNo = ATT.CardNo
		WHERE CAST(DTT.Date AS DATE) = CAST(DATEADD(DAY, -1, @date) AS DATE) AND SGR.ShiftId = @shiftId AND CAST(SGR.WorkingDate AS DATE) = CAST(DATEADD(DAY, -1, @date) AS DATE)
		AND EM.BranchID = @branchId
		END

	ELSE

	BEGIN
		UPDATE DTT SET DTT.OutTime = ATT.OutTime, 
		DTT.OverTime = IIF( CEILING(CONVERT(FLOAT, DATEDIFF(MINUTE, DTT.InTime, ATT.OutTime))/CONVERT(FLOAT, 60)) - 1- 8 > 0,
		CEILING(CONVERT(FLOAT, DATEDIFF(MINUTE, DTT.InTime, ATT.OutTime))/CONVERT(FLOAT, 60) - 1 - 8), 0
		 )
		 FROM DailyAttendances DTT
		 INNER JOIN Employees EM ON DTT.CardNo = EM.CardNo
		 INNER JOIN SecurityGuardRosters SGR ON EM.EmployeeID = SGR.EmployeeId
		INNER JOIN CardNoMappings CM ON DTT.CardNo = CM.OriginalCardNo
		LEFT JOIN (
				SELECT ATT.CardNo, IIF(MAX(ATT.InOutTime) = MIN(ATT.InOutTime), NULL, MAX(ATT.InOutTime)) AS OutTime 
				FROM Attendances ATT
				WHERE CAST(@date AS DATE) = CAST(ATT.InOutTime AS DATE)
				GROUP BY ATT.CardNo
			) ATT ON CM.GeneratedCardNo = ATT.CardNo
		WHERE CAST(DTT.Date AS DATE) = CAST(@date AS DATE) AND CAST(SGR.WorkingDate AS DATE) = CAST(@date AS DATE) AND SGR.ShiftId = @shiftId
		AND EM.BranchID = @branchId
	END
END

