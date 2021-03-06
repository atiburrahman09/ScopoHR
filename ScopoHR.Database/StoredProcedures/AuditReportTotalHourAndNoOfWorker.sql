--USE [ScopoHR]
--GO
--/****** Object:  StoredProcedure [dbo].[AuditReportTotalHourAndNoOfWorker]    Script Date: 11/28/2017 12:37:15 PM ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO


CREATE PROCEDURE [dbo].[AuditReportTotalHourAndNoOfWorker]
(
	@fromDate DATETIME,
	@toDate DATETIME, 
	@branchId INT,
	@shiftId as int, 
	@floor as nvarchar(max)=''
)
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

	--########################### Update Leave #################################
	UPDATE A
	SET A.OTMinutes = ISNULL(A.TotalMinutes, 0), Status = dbo.GetHolidayStatus(A.Date, A.EmployeeID, '')
	FROM #Data A
	WHERE dbo.GetHolidayStatus(A.Date, A.EmployeeID, '') IN ('WH', 'HD') 
	AND A.Status IN ('P', 'D')

	UPDATE A
	SET A.Status  = LT.ShortName, A.TotalMinutes = 0, A.OTMinutes = 0
	FROM #Data A
	LEFT JOIN LeaveApplications LA ON A.EmployeeID = LA.EmployeeID
	INNER JOIN LeaveTypes LT ON LA.LeaveTypeID = LT.LeaveTypeID
	WHERE 
	A.Status IN ('P', 'D') AND LA.IsDeleted <> 1
	AND A.Date BETWEEN LA.FromDate AND LA.ToDate



	CREATE TABLE #temp
	(
		Shift  nvarchar(50),
		Designation nvarchar (50),
		TotalHours float null,		
		Section nvarchar(max),
		CardNo nvarchar(max)
	)


	--IF(@floor = '3rd Way -1')
	--BEGIN
	--	SELECT @tempNo='31';
	--END

	--IF(@floor = '3rd Way -2')
	--BEGIN
	--	SELECT @tempNo='32';
	--END

	--IF(@floor = '3rd Way -3')
	--BEGIN
	--	SELECT @tempNo='33';
	--END

	--IF(@floor = '3rd Way -4')
	--BEGIN
	--	SELECT @tempNo='8';
	--END
	
	--IF(@floor = 'Knit -A')
	--BEGIN
	--	SELECT @tempNo='13';
	--END
	
	--IF(@floor = 'Knit -B')
	--BEGIN
	--	SELECT @tempNo='11';
	--END

	--IF(@floor = 'All Common Office')
	--BEGIN
	--	SELECT @tempNo='17';
	--END
	--IF(@floor = '3rd Way Common')
	--BEGIN
	--	SELECT @tempNo='30';
	--END

	--IF(@floor = '3rd Way Finishing')
	--BEGIN
	--	SELECT @tempNo='35';
	--END

	--IF(@floor = 'BDC')
	--BEGIN
	--	SELECT @tempNo='34';
	--END

	--IF(@floor = 'Night Shit All Common')
	--BEGIN
	--	SELECT @tempNo='42';
	--END

	--IF(@floor = 'Night Shit All Common')
	--BEGIN
	--	SELECT @tempNo='42';
	--END

	--IF(@floor = '3rd Way Cutting Night Shift')
	--BEGIN
	--	SELECT @tempNo='42';
	--END

	--IF(@floor = 'ASWL Common')
	--BEGIN
	--	SELECT @tempNo='40';
	--END

	--IF(@floor = 'ASWL Night Shift')
	--BEGIN
	--	SELECT @tempNo='41';
	--END
	--select @tempNo
	INSERT INTO #temp
	SELECT W.Name AS Shift, D.DesignationName Designation, ROUND(SUM(CAST(A.TotalMinutes AS float)) / CAST(60 AS float),2) TotalHours, 
	CASE 
				WHEN RIGHT(A.CardNo,5) >= '01000' AND RIGHT(A.CardNo,5) <= '01399' THEN 'Office'
				WHEN RIGHT(A.CardNo,5) >= '02000' AND RIGHT(A.CardNo,5) <= '02099' THEN 'Office'  
				WHEN RIGHT(A.CardNo,5) >= '06000' AND RIGHT(A.CardNo,5) <= '06599' THEN 'Office'
				WHEN RIGHT(A.CardNo,5) >= '06700' AND RIGHT(A.CardNo,5) <= '06999' THEN 'Office'
				WHEN RIGHT(A.CardNo,5) >= '07000' AND RIGHT(A.CardNo,5) <= '07999' THEN 'Office' 
				WHEN RIGHT(A.CardNo,5) >= '08000' AND RIGHT(A.CardNo,5) <= '08999' THEN 'Office'  
				WHEN RIGHT(A.CardNo,5) >= '09000' AND RIGHT(A.CardNo,5) <= '09999' THEN 'Office' 
				WHEN RIGHT(A.CardNo,5) >= '01900' AND RIGHT(A.CardNo,5) <= '01999' THEN 'Finishing' 
				WHEN RIGHT(A.CardNo,5) >= '02500' AND RIGHT(A.CardNo,5) <= '02599' THEN 'Finishing'
				WHEN RIGHT(A.CardNo,5) >= '03600' AND RIGHT(A.CardNo,5) <= '03699' THEN 'Finishing'
				WHEN RIGHT(A.CardNo,5) >= '66000' AND RIGHT(A.CardNo,5) <= '76999' THEN 'Finishing'
				WHEN RIGHT(A.CardNo,5) >= '93000' AND RIGHT(A.CardNo,5) <= '96999' THEN 'Finishing'
				WHEN RIGHT(A.CardNo,5) >= '04100' AND RIGHT(A.CardNo,5) <= '04999' THEN 'Sewing'
				WHEN RIGHT(A.CardNo,5) >= '02100' AND RIGHT(A.CardNo,5) <= '02399' THEN 'Sewing'
				WHEN RIGHT(A.CardNo,5) >= '02700' AND RIGHT(A.CardNo,5) <= '02899' THEN 'Sewing'
				WHEN RIGHT(A.CardNo,5) >= '03100' AND RIGHT(A.CardNo,5) <= '03499' THEN 'Sewing'
				WHEN RIGHT(A.CardNo,5) >= '05000' AND RIGHT(A.CardNo,5) <= '05999' THEN 'Sewing'
				WHEN RIGHT(A.CardNo,5) >= '11000' AND RIGHT(A.CardNo,5) <= '54999' THEN 'Sewing'
				WHEN RIGHT(A.CardNo,5) >= '55000' AND RIGHT(A.CardNo,5) <= '65999' THEN 'Sewing'
				WHEN RIGHT(A.CardNo,5) >= '88000' AND RIGHT(A.CardNo,5) <= '92999' THEN 'Sewing'
				WHEN RIGHT(A.CardNo,5) >= '02400' AND RIGHT(A.CardNo,5) <= '02499' THEN 'Cutting'
				WHEN RIGHT(A.CardNo,5) >= '03500' AND RIGHT(A.CardNo,5) <= '03599' THEN 'Cutting'
				WHEN RIGHT(A.CardNo,5) >= '03700' AND RIGHT(A.CardNo,5) <= '03999' THEN 'Cutting'
				WHEN RIGHT(A.CardNo,5) >= '77000' AND RIGHT(A.CardNo,5) <= '87999' THEN 'Cutting'
				WHEN RIGHT(A.CardNo,5) >= '97000' AND RIGHT(A.CardNo,5) <= '99999' THEN 'Cutting'
				WHEN RIGHT(A.CardNo,5) >= '02600' AND RIGHT(A.CardNo,5) <= '02699' THEN 'Store'
				WHEN RIGHT(A.CardNo,5) >= '06600' AND RIGHT(A.CardNo,5) <= '06699' THEN 'Store'
				ELSE 'Other' END AS Section,A.CardNo
	FROM #Data A
	INNER JOIN Employees E ON A.CardNo = E.CardNo
	INNER JOIN Designations D ON E.DesignationID = D.DesignationID
	INNER JOIN WorkingShifts W ON E.ShiftId = W.Id
	WHERE 
	E.DepartmentID <> 6 
	AND E.ProductionFloorLineID <> 110
	AND A.TotalMinutes IS NOT NULL AND A.TotalMinutes <> 0
	AND A.CardNo NOT LIKE ('14%')
	AND A.CardNo NOT LIKE ('44%')
	GROUP BY W.Name, D.DesignationName,A.CardNo
	ORDER BY  RIGHT(A.CardNo,5)

	--SELECT * FROM #temp
	

	SELECT  Shift,Section,Designation,SUM(TotalHours) As TotalHours,
	CASE 
		WHEN (TotalHours) <= 40 THEN 40
		WHEN (TotalHours) BETWEEN 40 AND 48 THEN 48
		WHEN (TotalHours) BETWEEN 48 AND 60 THEN 60
		WHEN (TotalHours) BETWEEN 60 AND 72 THEN 72
		WHEN (TotalHours) > 72 THEN 84
		END AS TypeOfHours, 
	COUNT(*) AS NoOfWorkers	
	FROM #temp
	GROUP BY Shift,Section,Designation,TotalHours
	ORDER BY Section,Designation

	DROP TABLE #Data
	DROP TABLE #temp
END

--EXEC AuditReportTotalHourAndNoOfWorker '01/06/2018', '01/11/2018','1', '1',''
