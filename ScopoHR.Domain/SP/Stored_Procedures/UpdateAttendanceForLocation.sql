USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[UpdateAttendanceForLocation]    Script Date: 7/19/2018 4:29:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[UpdateAttendanceForLocation]
	@locationID INT,
	@inTimeDate datetime,
	@inTime datetime,
	@modifiedBy nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT at.CardNo,CAST(InOutTime as DATE) as InOUtTime
	INTO #temp 
	FROM Attendances AT
	INNER JOIN CardNoMappings CM ON AT.CardNo=CM.GeneratedCardNo
	INNER JOIN WorkerBus WB ON CM.OriginalCardNo=WB.CardNo
	INNER JOIN Locations l ON WB.LocationID=l.LocationID
	where at.CardNo in 
	(
	SELECT CardNo FROM Attendances WHERE CAST(InOutTime AS DATE) = CAST(@inTimeDate AS DATE)
	)
	AND WB.LocationID=@locationID
	AND CAST(AT.InOutTime AS DATE) = CAST(@inTimeDate AS DATE)


	--SELECT * FROM #temp
	DELETE FROM Attendances
	WHERE CAST(InOutTime AS DATE) = CAST(@inTimeDate AS DATE)
	AND CardNo IN (SELECT CardNo FROM #temp)

	INSERT INTO Attendances
	SELECT CAST(CONCAT(CONCAT(InOutTime,' '), CONVERT(TIME,@inTime,108)) AS datetime2),'BUS Update',CardNo,@modifiedBy,GETDATE(),0
	FROM #temp

	DROP TABLE #temp
	
END

--EXEC UpdateAttendanceForLocation @locationID=4, @inTimeDate='6/5/2018 12:00:00 AM', @inTime='6/19/2018 8:50:00 AM', @modifiedBy=superuser
--EXEC UpdateAttendanceForLocation @locationID=4, @inTimeDate='6/11/2018 12:00:00 AM', @inTime='7/19/2018 8:20:00 AM', @modifiedBy='superuser'



