--IF EXISTS(SELECT * FROM SYS.OBJECTS WHERE NAME = 'GetOvertimeReport')
--BEGIN
--	DROP PROCEDURE GetOvertimeReport
--END
--GO



CREATE PROCEDURE [dbo].[GetOvertimeReport]
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
		[EmployeeID] INT NULL,
		[CardNo] [varchar](20) NULL,
		[EmployeeName] [varchar](200) NULL,
		[InTime] [varchar](20) NULL,
		[InTimeDate][DateTime] NULL,
		[OutTime] [varchar](20) NULL,
		[OutTimeDate][DateTime] NULL,
		[Status] VARCHAR(10),
		[TotalMinutes] [int] NULL,
		[OTMinutes] [int] NULL,
		[LunchTime] [int] NULL
	)

	INSERT INTO #TempAttendance
	EXEC GetAttendanceData @fromDate, @toDate, @floor, @branchID, @shiftID, @cardNo


	SELECT TA.CardNo AS OriginalCardNo, TA.EmployeeName, TA.InTime, TA.OutTime, TA.OTMinutes,D.DepartmentName,TA.[Date] 
	FROM #TempAttendance TA
	INNER JOIN Employees E ON TA.CardNo=E.CardNo
	INNER JOIN Departments D ON E.DepartmentID=D.DepartmentID
	WHERE TA.OTMinutes IS NOT NULL AND TA.OTMinutes <> 0
	GROUP BY TA.CardNo, TA.EmployeeName, TA.InTime, TA.OutTime, TA.OTMinutes,D.DepartmentName,TA.[Date] 
	ORDER BY TA.CardNo,TA.[Date]
END

--EXEC GetOvertimeReport '2017-11-01', '2017-11-30','', '1','',''