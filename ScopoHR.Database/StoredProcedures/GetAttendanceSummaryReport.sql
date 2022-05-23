--IF EXISTS(SELECT * FROM sys.objects WHERE name = 'GetAttendanceSummary')
--BEGIN
--	DROP PROCEDURE GetAttendanceSummary
--END
--GO


CREATE PROCEDURE [dbo].[GetAttendanceSummary]
	@fromDate DATETIME,
	@floor VARCHAR(200) = '',
	@branchID INT,
	@shiftID VARCHAR(10) = ''
	
AS
BEGIN
	SET NOCOUNT ON;

	CREATE TABLE #Data
	(
		Day	nvarchar(50),
		Date	date,
		EmployeeID INT NULL,
		CardNo	nvarchar(200),
		EmployeeName	nvarchar(500),
		InTime	varchar(20),
		InTimeDate datetime NULL,
		OutTime	varchar(20),
		OutTimeDate datetime NULL,
		Status	varchar(5),
		TotalMinutes int,
		OTMinutes	int,
		LunchTime	int
	)
	
	INSERT INTO #Data
	EXEC GetAttendanceData @fromDate, @fromDate, @floor, @branchID, @shiftID, ''

	CREATE TABLE #Absent
	(
		EmployeeID INT,
		EmployeeName VARCHAR(500),
		CardNo VARCHAR(50),
		DesignationName VARCHAR(200),
		Floor VARCHAR(200),
		AbsentDate DATETIME,
		Status VARCHAR(10) NULL
	)

	INSERT INTO #Absent
	EXEC GetAbsentReport @fromDate, @fromDate, @floor, @branchID, @shiftID, '', 0

	
	
	INSERT INTO #Data
	SELECT DATENAME(DW,CAST(A.AbsentDate AS Date)) as [Day], 
	CAST(A.AbsentDate AS DATE) AS Date, A.EmployeeID,
	A.CardNo, A.EmployeeName, '' AS InTime, NULL AS InTimeDate, '' AS OutTime, NULL AS OutTimeDate, A.Status,
	NULL AS TotalMinutes, NULL AS OTMinutes, 0 AS LunchTime
	FROM #Absent A

	UPDATE A
	SET A.Status  = LT.ShortName, A.TotalMinutes = 0, A.OTMinutes = 0
	FROM #Data A
	LEFT JOIN LeaveApplications LA ON A.EmployeeID = LA.EmployeeID
	INNER JOIN LeaveTypes LT ON LA.LeaveTypeID = LT.LeaveTypeID
	WHERE 
	A.Status IN ('P', 'D') AND LA.IsDeleted <> 1
	AND A.Date BETWEEN LA.FromDate AND LA.ToDate

	UPDATE A
	SET A.Status = dbo.GetHolidayStatus(A.Date, A.EmployeeID, '')
	FROM #Data A
	WHERE dbo.GetHolidayStatus(A.Date, A.EmployeeID, '') IN ('WH', 'HD') 
	AND A.Status = 'WP'

	--SELECT Count(*) FROM #Data
	--SELECT DISTINCT STATUS FROM #Data

		CREATE TABLE #TEMP
			(
				Code Nvarchar(10),
				AbsentDays INT,
				Leave INT,
				Late INT,
				Total INT,
				NoLine INT,
				GrandTotal INT,
				MALE INT,
				FEMALE INT,
				Section Nvarchar(Max),
				Serial NVARCHAR (10)
			)
		IF(@floor IS NULL OR @floor='')
		BEGIN
			INSERT INTO #TEMP 
			SELECT Right(D.CardNo,5), COUNT(IIF(D.Status='A' OR D.Status='WP',1,NULL)) AS AbsentDays,
			COUNT(IIF(D.Status='CL' OR D.Status='ML' OR D.Status='MT' OR D.Status='EL' OR D.Status='FL',1,NULL)) AS Leave,
			COUNT(IIF(D.Status='D',1,NULL)) AS Late,
			COUNT(IIF(D.Status='P' OR D.Status='D',1,NULL)) AS Total,
			COUNT(IIF(D.Status='P' OR D.Status='D',1,NULL)) AS NoLine,
			(COUNT(IIF(D.Status='CL' OR D.Status='ML' OR D.Status='MT' OR D.Status='EL' OR D.Status='FL',1,NULL)) + COUNT(IIF(D.Status='D',1,NULL)) + COUNT(IIF(D.Status='P',1,NULL)) + COUNT(IIF(D.Status='A',1,NULL)) + COUNT(IIF(D.Status='WP',1,NULL)))   AS GrandTotal,
			COUNT(IIF(E.GenderID=1 ,1,NULL)) AS MALE,
			COUNT(IIF(E.GenderID=2 ,1,NULL)) AS FEMALE,
			CASE 
				WHEN Right(D.CardNo,5) >= '01000' AND Right(D.CardNo,5) <= '01999' THEN 'Factory Management' 
				WHEN Right(D.CardNo,5) >= '02000' AND Right(D.CardNo,5) <= '02099' THEN 'Engineering/Data Entry'
				WHEN Right(D.CardNo,5) >= '02100' AND Right(D.CardNo,5) <= '02199' THEN 'Factory Management'
				WHEN Right(D.CardNo,5) >= '02200' AND Right(D.CardNo,5) <= '02399' THEN 'Production Staff Swing'
				WHEN Right(D.CardNo,5) >= '02400' AND Right(D.CardNo,5) <= '02499' THEN 'Production Staff Cutting'
				WHEN Right(D.CardNo,5) >= '02500' AND Right(D.CardNo,5) <= '02599' THEN 'Production Staff Finishing'
				WHEN Right(D.CardNo,5) >= '02600' AND Right(D.CardNo,5) <= '02699' THEN 'Store' 
				WHEN Right(D.CardNo,5) >= '02700' AND Right(D.CardNo,5) <= '02799' THEN 'Q.C' 
				WHEN Right(D.CardNo,5) >= '02800' AND Right(D.CardNo,5) <= '02899' THEN 'Production Staff Swing' 
				WHEN Right(D.CardNo,5) >= '03100' AND Right(D.CardNo,5) <= '03499' THEN 'Production Staff Swing'
				WHEN Right(D.CardNo,5) >= '03500' AND Right(D.CardNo,5) <= '03599' THEN 'Production Staff Cutting'
				WHEN Right(D.CardNo,5) >= '03600' AND Right(D.CardNo,5) <= '03699' THEN 'Production Staff Finishing'
				WHEN Right(D.CardNo,5) >= '03800' AND Right(D.CardNo,5) <= '03999' THEN 'Production Staff Cutting'
				WHEN Right(D.CardNo,5) >= '04100' AND Right(D.CardNo,5) <= '04999' THEN 'Sample Machinist'
				WHEN Right(D.CardNo,5) >= '05199' AND Right(D.CardNo,5) <= '05499' THEN 'Mechanical' 
				WHEN Right(D.CardNo,5) >= '05500' AND Right(D.CardNo,5) <= '05899' THEN 'Electrical'				
				WHEN Right(D.CardNo,5) >= '06100' AND Right(D.CardNo,5) <= '06199' THEN 'Administration (Pure)'
				WHEN Right(D.CardNo,5) >= '06200' AND Right(D.CardNo,5) <= '06299' THEN 'Welfare_Medical'   
				WHEN Right(D.CardNo,5) >= '06300' AND Right(D.CardNo,5) <= '06399' THEN 'Compliance'
				WHEN Right(D.CardNo,5) >= '06400' AND Right(D.CardNo,5) <= '06499' THEN 'IT' 
				WHEN Right(D.CardNo,5) >= '06500' AND Right(D.CardNo,5) <= '06599' THEN 'Time Keeper' 
				WHEN Right(D.CardNo,5) >= '06600' AND Right(D.CardNo,5) <= '06699' THEN 'Store'
				WHEN Right(D.CardNo,5) >= '06700' AND Right(D.CardNo,5) <= '06799' THEN 'Driver'
				WHEN Right(D.CardNo,5) >= '06800' AND Right(D.CardNo,5) <= '06899' THEN 'Peon'
				WHEN Right(D.CardNo,5) >= '06900' AND Right(D.CardNo,5) <= '06999' THEN 'Loader/Cleaner'				
				WHEN Right(D.CardNo,5) >= '07000' AND Right(D.CardNo,5) <= '07999' THEN 'Accounts' 
				WHEN Right(D.CardNo,5) >= '08100' AND Right(D.CardNo,5) <= '08699' THEN 'Security'				
				WHEN Right(D.CardNo,5) >= '08700' AND Right(D.CardNo,5) <= '08899' THEN 'Checker'		
				WHEN Right(D.CardNo,5) >= '08900' AND Right(D.CardNo,5) <= '08999' THEN 'Compliance'	
				WHEN Right(D.CardNo,5) >= '09000' AND Right(D.CardNo,5) <= '09099' THEN 'Factory Management'	
				WHEN Right(D.CardNo,5) >= '09100' AND Right(D.CardNo,5) <= '09799' THEN 'Merchandising'
				WHEN Right(D.CardNo,5) >= '09800' AND Right(D.CardNo,5) <= '09999' THEN 'Commercial'
				WHEN Right(D.CardNo,5) >= '11000' AND Right(D.CardNo,5) <= '32999' THEN 'Operator'
				WHEN Right(D.CardNo,5) >= '33000' AND Right(D.CardNo,5) <= '54999' THEN 'Asst Swing Machine Operator'
				WHEN Right(D.CardNo,5) >= '55000' AND Right(D.CardNo,5) <= '65999' THEN 'Iron Man (S)'
				WHEN Right(D.CardNo,5) >= '66000' AND Right(D.CardNo,5) <= '66999' THEN 'Iron Man (F)'
				WHEN Right(D.CardNo,5) >= '67000' AND Right(D.CardNo,5) <= '76999' THEN 'Packer, Folder And Finishing Asst.'
				WHEN Right(D.CardNo,5) >= '77000' AND Right(D.CardNo,5) <= '87999' THEN 'Asst.Cutter'
				WHEN Right(D.CardNo,5) >= '97000' AND Right(D.CardNo,5) <= '99999' THEN 'Quality(C)'
				WHEN Right(D.CardNo,5) >= '88000' AND Right(D.CardNo,5) <= '92999' THEN 'Quality(S)'
				WHEN Right(D.CardNo,5) >= '93000' AND Right(D.CardNo,5) <= '96999' THEN 'Quality(F)'

				ELSE 'Other' END AS Section,

				CASE 
				WHEN Right(D.CardNo,5) >= '01000' AND Right(D.CardNo,5) <= '01999' THEN '1' 
				WHEN Right(D.CardNo,5) >= '02000' AND Right(D.CardNo,5) <= '02099' THEN '2'
				WHEN Right(D.CardNo,5) >= '02100' AND Right(D.CardNo,5) <= '02199' THEN '1'
				WHEN Right(D.CardNo,5) >= '02200' AND Right(D.CardNo,5) <= '02399' THEN '3'
				WHEN Right(D.CardNo,5) >= '02400' AND Right(D.CardNo,5) <= '02499' THEN '4'
				WHEN Right(D.CardNo,5) >= '02500' AND Right(D.CardNo,5) <= '02599' THEN '5'
				WHEN Right(D.CardNo,5) >= '02600' AND Right(D.CardNo,5) <= '02699' THEN '6' 
				WHEN Right(D.CardNo,5) >= '02700' AND Right(D.CardNo,5) <= '02799' THEN '7' 
				WHEN Right(D.CardNo,5) >= '02800' AND Right(D.CardNo,5) <= '02899' THEN '3' 
				WHEN Right(D.CardNo,5) >= '03100' AND Right(D.CardNo,5) <= '03499' THEN '3'
				WHEN Right(D.CardNo,5) >= '03500' AND Right(D.CardNo,5) <= '03599' THEN '4'
				WHEN Right(D.CardNo,5) >= '03600' AND Right(D.CardNo,5) <= '03699' THEN '5'
				WHEN Right(D.CardNo,5) >= '03800' AND Right(D.CardNo,5) <= '03999' THEN '4'
				WHEN Right(D.CardNo,5) >= '04100' AND Right(D.CardNo,5) <= '04999' THEN '8'
				WHEN Right(D.CardNo,5) >= '05199' AND Right(D.CardNo,5) <= '05499' THEN '9' 
				WHEN Right(D.CardNo,5) >= '05500' AND Right(D.CardNo,5) <= '05899' THEN '10'				
				WHEN Right(D.CardNo,5) >= '06100' AND Right(D.CardNo,5) <= '06199' THEN '11'
				WHEN Right(D.CardNo,5) >= '06200' AND Right(D.CardNo,5) <= '06299' THEN '12'   
				WHEN Right(D.CardNo,5) >= '06300' AND Right(D.CardNo,5) <= '06399' THEN '13'
				WHEN Right(D.CardNo,5) >= '06400' AND Right(D.CardNo,5) <= '06499' THEN '14' 
				WHEN Right(D.CardNo,5) >= '06500' AND Right(D.CardNo,5) <= '06599' THEN '15' 
				WHEN Right(D.CardNo,5) >= '06600' AND Right(D.CardNo,5) <= '06699' THEN '16'
				WHEN Right(D.CardNo,5) >= '06700' AND Right(D.CardNo,5) <= '06799' THEN '17'
				WHEN Right(D.CardNo,5) >= '06800' AND Right(D.CardNo,5) <= '06899' THEN '18'
				WHEN Right(D.CardNo,5) >= '06900' AND Right(D.CardNo,5) <= '06999' THEN '19'				
				WHEN Right(D.CardNo,5) >= '07000' AND Right(D.CardNo,5) <= '07999' THEN '20' 
				WHEN Right(D.CardNo,5) >= '08100' AND Right(D.CardNo,5) <= '08699' THEN '21'				
				WHEN Right(D.CardNo,5) >= '08700' AND Right(D.CardNo,5) <= '08899' THEN '22'		
				WHEN Right(D.CardNo,5) >= '08900' AND Right(D.CardNo,5) <= '08999' THEN '23'	
				WHEN Right(D.CardNo,5) >= '09000' AND Right(D.CardNo,5) <= '09099' THEN '1'	
				WHEN Right(D.CardNo,5) >= '09100' AND Right(D.CardNo,5) <= '09799' THEN '24'
				WHEN Right(D.CardNo,5) >= '09800' AND Right(D.CardNo,5) <= '09999' THEN '25'
				WHEN Right(D.CardNo,5) >= '11000' AND Right(D.CardNo,5) <= '32999' THEN '26'
				WHEN Right(D.CardNo,5) >= '33000' AND Right(D.CardNo,5) <= '54999' THEN '27'
				WHEN Right(D.CardNo,5) >= '55000' AND Right(D.CardNo,5) <= '65999' THEN '28'
				WHEN Right(D.CardNo,5) >= '66000' AND Right(D.CardNo,5) <= '66999' THEN '29'
				WHEN Right(D.CardNo,5) >= '67000' AND Right(D.CardNo,5) <= '76999' THEN '30'
				WHEN Right(D.CardNo,5) >= '77000' AND Right(D.CardNo,5) <= '87999' THEN '31'
				WHEN Right(D.CardNo,5) >= '97000' AND Right(D.CardNo,5) <= '99999' THEN '32'
				WHEN Right(D.CardNo,5) >= '88000' AND Right(D.CardNo,5) <= '92999' THEN '33'
				WHEN Right(D.CardNo,5) >= '93000' AND Right(D.CardNo,5) <= '96999' THEN '34'

				ELSE '35' END AS Serial
		FROM #Data D 
		INNER JOIN Employees E ON D.CardNo = E.CardNo
		WHERE E.CardNo NOT LIKE '14%'
		AND E.IsActive=1
		GROUP BY D.CardNo
		ORDER BY D.CardNo
		END

		ELSE
			BEGIN
			
			DECLARE @tempNo varchar(10);
			IF(@floor = '3rd Way -1')
			BEGIN
				SELECT @tempNo='31';
			END

			IF(@floor = '3rd Way -2')
			BEGIN
			SELECT @tempNo='32';
			END

			IF(@floor = '3rd Way -3')
			BEGIN
				SELECT @tempNo='33';
			END

			IF(@floor = '3rd Way -4')
			BEGIN
				SELECT @tempNo='8';
			END
	
			IF(@floor = 'Knit -A')
			BEGIN
				SELECT @tempNo='13';
			END
	
			IF(@floor = 'Knit -B')
			BEGIN
				SELECT @tempNo='11';
			END

			IF(@floor = 'All Common Office')
			BEGIN
			SELECT @tempNo='17';
			END
			IF(@floor = '3rd Way Common')
			BEGIN
			SELECT @tempNo='30';
			END

			IF(@floor = '3rd Way Finishing')
			BEGIN
			SELECT @tempNo='35';
			END

			IF(@floor = 'BDC')
			BEGIN
			SELECT @tempNo='34';
			END

			IF(@floor = 'Night Shit All Common')
			BEGIN
			SELECT @tempNo='42';
			END

			IF(@floor = 'Night Shit All Common')
			BEGIN
			SELECT @tempNo='42';
			END

			IF(@floor = '3rd Way Cutting Night Shift')
			BEGIN
			SELECT @tempNo='42';
			END

			IF(@floor = 'ASWL Common')
			BEGIN
			SELECT @tempNo='40';
			END

			IF(@floor = 'ASWL Night Shift')
			BEGIN
			SELECT @tempNo='41';
			END

					--SELECT * FROM #Data
		
			INSERT INTO #TEMP 
			SELECT Right(D.CardNo,5), COUNT(IIF(D.Status='A' OR D.Status='WP',1,NULL)) AS AbsentDays,
			COUNT(IIF(D.Status='CL' OR D.Status='ML' OR D.Status='MT' OR D.Status='EL' OR D.Status='FL',1,NULL)) AS Leave,
			COUNT(IIF(D.Status='D',1,NULL)) AS Late,
			COUNT(IIF(D.Status='P' OR D.Status='D',1,NULL)) AS Total,
			COUNT(IIF(D.Status='P' OR D.Status='D',1,NULL)) AS NoLine,
			(COUNT(IIF(D.Status='CL' OR D.Status='ML' OR D.Status='MT' OR D.Status='EL' OR D.Status='FL',1,NULL)) + COUNT(IIF(D.Status='D',1,NULL)) + COUNT(IIF(D.Status='P',1,NULL)) + COUNT(IIF(D.Status='A',1,NULL)) + COUNT(IIF(D.Status='WP',1,NULL)))   AS GrandTotal,
			COUNT(IIF(E.GenderID=1 ,1,NULL)) AS MALE,
			COUNT(IIF(E.GenderID=2 ,1,NULL)) AS FEMALE,
			CASE 
				WHEN D.CardNo >= CONCAT(@tempNo,'01000') AND D.CardNo <= CONCAT(@tempNo,'01999') THEN 'Factory Management' 
				WHEN D.CardNo >= CONCAT(@tempNo,'02000') AND D.CardNo <= CONCAT(@tempNo,'02099') THEN 'Engineering/Data Entry'
				WHEN D.CardNo >= CONCAT(@tempNo,'02100') AND D.CardNo <= CONCAT(@tempNo,'02199') THEN 'Factory Management'
				WHEN D.CardNo >= CONCAT(@tempNo,'02200') AND D.CardNo <= CONCAT(@tempNo,'02399') THEN 'Production Staff Swing'
				WHEN D.CardNo >= CONCAT(@tempNo,'02400') AND D.CardNo <= CONCAT(@tempNo,'02499') THEN 'Production Staff Cutting'
				WHEN D.CardNo >= CONCAT(@tempNo,'02500') AND D.CardNo <= CONCAT(@tempNo,'02599') THEN 'Production Staff Finishing'
				WHEN D.CardNo >= CONCAT(@tempNo,'02600') AND D.CardNo <= CONCAT(@tempNo,'02699') THEN 'Store' 
				WHEN D.CardNo >= CONCAT(@tempNo,'02700') AND D.CardNo <= CONCAT(@tempNo,'02799') THEN 'Q.C' 
				WHEN D.CardNo >= CONCAT(@tempNo,'02800') AND D.CardNo <= CONCAT(@tempNo,'02899') THEN 'Production Staff Swing' 
				WHEN D.CardNo >= CONCAT(@tempNo,'03100') AND D.CardNo <= CONCAT(@tempNo,'03499') THEN 'Production Staff Swing'
				WHEN D.CardNo >= CONCAT(@tempNo,'03500') AND D.CardNo <= CONCAT(@tempNo,'03599') THEN 'Production Staff Cutting'
				WHEN D.CardNo >= CONCAT(@tempNo,'03600') AND D.CardNo <= CONCAT(@tempNo,'03699') THEN 'Production Staff Finishing'
				WHEN D.CardNo >= CONCAT(@tempNo,'03800') AND D.CardNo <= CONCAT(@tempNo,'03999') THEN 'Production Staff Cutting'
				WHEN D.CardNo >= CONCAT(@tempNo,'04100') AND D.CardNo <= CONCAT(@tempNo,'04999') THEN 'Sample Machinist'
				WHEN D.CardNo >= CONCAT(@tempNo,'05199') AND D.CardNo <= CONCAT(@tempNo,'05499') THEN 'Mechanical' 
				WHEN D.CardNo >= CONCAT(@tempNo,'05500') AND D.CardNo <= CONCAT(@tempNo,'05899') THEN 'Electrical'				
				WHEN D.CardNo >= CONCAT(@tempNo,'06100') AND D.CardNo <= CONCAT(@tempNo,'06199') THEN 'Administration (Pure)'
				WHEN D.CardNo >= CONCAT(@tempNo,'06200') AND D.CardNo <= CONCAT(@tempNo,'06299') THEN 'Welfare_Medical'   
				WHEN D.CardNo >= CONCAT(@tempNo,'06300') AND D.CardNo <= CONCAT(@tempNo,'06399') THEN 'Compliance'
				WHEN D.CardNo >= CONCAT(@tempNo,'06400') AND D.CardNo <= CONCAT(@tempNo,'06499') THEN 'IT' 
				WHEN D.CardNo >= CONCAT(@tempNo,'06500') AND D.CardNo <= CONCAT(@tempNo,'06599') THEN 'Time Keeper' 
				WHEN D.CardNo >= CONCAT(@tempNo,'06600') AND D.CardNo <= CONCAT(@tempNo,'06699') THEN 'Store'
				WHEN D.CardNo >= CONCAT(@tempNo,'06700') AND D.CardNo <= CONCAT(@tempNo,'06799') THEN 'Driver'
				WHEN D.CardNo >= CONCAT(@tempNo,'06800') AND D.CardNo <= CONCAT(@tempNo,'06899') THEN 'Peon'
				WHEN D.CardNo >= CONCAT(@tempNo,'06900') AND D.CardNo <= CONCAT(@tempNo,'06999') THEN 'Loader/Cleaner'				
				WHEN D.CardNo >= CONCAT(@tempNo,'07000') AND D.CardNo <= CONCAT(@tempNo,'07999') THEN 'Accounts' 
				WHEN D.CardNo >= CONCAT(@tempNo,'08100') AND D.CardNo <= CONCAT(@tempNo,'08699') THEN 'Security'				
				WHEN D.CardNo >= CONCAT(@tempNo,'08700') AND D.CardNo <= CONCAT(@tempNo,'08899') THEN 'Checker'		
				WHEN D.CardNo >= CONCAT(@tempNo,'08900') AND D.CardNo <= CONCAT(@tempNo,'08999') THEN 'Compliance'	
				WHEN D.CardNo >= CONCAT(@tempNo,'09000') AND D.CardNo <= CONCAT(@tempNo,'09099') THEN 'Factory Management'	
				WHEN D.CardNo >= CONCAT(@tempNo,'09100') AND D.CardNo <= CONCAT(@tempNo,'09799') THEN 'Merchandising'
				WHEN D.CardNo >= CONCAT(@tempNo,'09800') AND D.CardNo <= CONCAT(@tempNo,'09999') THEN 'Commercial'
				WHEN D.CardNo >= CONCAT(@tempNo,'11000') AND D.CardNo <= CONCAT(@tempNo,'32999') THEN 'Operator'
				WHEN D.CardNo >= CONCAT(@tempNo,'33000') AND D.CardNo <= CONCAT(@tempNo,'54999') THEN 'Asst Swing Machine Operator'
				WHEN D.CardNo >= CONCAT(@tempNo,'55000') AND D.CardNo <= CONCAT(@tempNo,'65999') THEN 'Iron Man (S)'
				WHEN D.CardNo >= CONCAT(@tempNo,'66000') AND D.CardNo <= CONCAT(@tempNo,'66999') THEN 'Iron Man (F)'
				WHEN D.CardNo >= CONCAT(@tempNo,'67000') AND D.CardNo <= CONCAT(@tempNo,'76999') THEN 'Packer, Folder And Finishing Asst.'
				WHEN D.CardNo >= CONCAT(@tempNo,'77000') AND D.CardNo <= CONCAT(@tempNo,'87999') THEN 'Asst.Cutter'
				WHEN D.CardNo >= CONCAT(@tempNo,'97000') AND D.CardNo <= CONCAT(@tempNo,'99999') THEN 'Quality(C)'
				WHEN D.CardNo >= CONCAT(@tempNo,'88000') AND D.CardNo <= CONCAT(@tempNo,'92999') THEN 'Quality(S)'
				WHEN D.CardNo >= CONCAT(@tempNo,'93000') AND D.CardNo <= CONCAT(@tempNo,'96999') THEN 'Quality(F)'

				ELSE 'Other' END AS Section,

				CASE 
				WHEN D.CardNo >= CONCAT(@tempNo,'01000') AND D.CardNo <= CONCAT(@tempNo,'01999') THEN '1' 
				WHEN D.CardNo >= CONCAT(@tempNo,'02000') AND D.CardNo <= CONCAT(@tempNo,'02099') THEN '2'
				WHEN D.CardNo >= CONCAT(@tempNo,'02100') AND D.CardNo <= CONCAT(@tempNo,'02199') THEN '1'
				WHEN D.CardNo >= CONCAT(@tempNo,'02200') AND D.CardNo <= CONCAT(@tempNo,'02399') THEN '3'
				WHEN D.CardNo >= CONCAT(@tempNo,'02400') AND D.CardNo <= CONCAT(@tempNo,'02499') THEN '4'
				WHEN D.CardNo >= CONCAT(@tempNo,'02500') AND D.CardNo <= CONCAT(@tempNo,'02599') THEN '5'
				WHEN D.CardNo >= CONCAT(@tempNo,'02600') AND D.CardNo <= CONCAT(@tempNo,'02699') THEN '6' 
				WHEN D.CardNo >= CONCAT(@tempNo,'02700') AND D.CardNo <= CONCAT(@tempNo,'02799') THEN '7' 
				WHEN D.CardNo >= CONCAT(@tempNo,'02800') AND D.CardNo <= CONCAT(@tempNo,'02899') THEN '3' 
				WHEN D.CardNo >= CONCAT(@tempNo,'03100') AND D.CardNo <= CONCAT(@tempNo,'03499') THEN '3'
				WHEN D.CardNo >= CONCAT(@tempNo,'03500') AND D.CardNo <= CONCAT(@tempNo,'03599') THEN '4'
				WHEN D.CardNo >= CONCAT(@tempNo,'03600') AND D.CardNo <= CONCAT(@tempNo,'03699') THEN '5'
				WHEN D.CardNo >= CONCAT(@tempNo,'03800') AND D.CardNo <= CONCAT(@tempNo,'03999') THEN '4'
				WHEN D.CardNo >= CONCAT(@tempNo,'04100') AND D.CardNo <= CONCAT(@tempNo,'04999') THEN '8'
				WHEN D.CardNo >= CONCAT(@tempNo,'05199') AND D.CardNo <= CONCAT(@tempNo,'05499') THEN '9' 
				WHEN D.CardNo >= CONCAT(@tempNo,'05500') AND D.CardNo <= CONCAT(@tempNo,'05899') THEN '10'				
				WHEN D.CardNo >= CONCAT(@tempNo,'06100') AND D.CardNo <= CONCAT(@tempNo,'06199') THEN '11'
				WHEN D.CardNo >= CONCAT(@tempNo,'06200') AND D.CardNo <= CONCAT(@tempNo,'06299') THEN '12'   
				WHEN D.CardNo >= CONCAT(@tempNo,'06300') AND D.CardNo <= CONCAT(@tempNo,'06399') THEN '13'
				WHEN D.CardNo >= CONCAT(@tempNo,'06400') AND D.CardNo <= CONCAT(@tempNo,'06499') THEN '14' 
				WHEN D.CardNo >= CONCAT(@tempNo,'06500') AND D.CardNo <= CONCAT(@tempNo,'06599') THEN '15' 
				WHEN D.CardNo >= CONCAT(@tempNo,'06600') AND D.CardNo <= CONCAT(@tempNo,'06699') THEN '16'
				WHEN D.CardNo >= CONCAT(@tempNo,'06700') AND D.CardNo <= CONCAT(@tempNo,'06799') THEN '17'
				WHEN D.CardNo >= CONCAT(@tempNo,'06800') AND D.CardNo <= CONCAT(@tempNo,'06899') THEN '18'
				WHEN D.CardNo >= CONCAT(@tempNo,'06900') AND D.CardNo <= CONCAT(@tempNo,'06999') THEN '19'				
				WHEN D.CardNo >= CONCAT(@tempNo,'07000') AND D.CardNo <= CONCAT(@tempNo,'07999') THEN '20' 
				WHEN D.CardNo >= CONCAT(@tempNo,'08100') AND D.CardNo <= CONCAT(@tempNo,'08699') THEN '21'				
				WHEN D.CardNo >= CONCAT(@tempNo,'08700') AND D.CardNo <= CONCAT(@tempNo,'08899') THEN '22'		
				WHEN D.CardNo >= CONCAT(@tempNo,'08900') AND D.CardNo <= CONCAT(@tempNo,'08999') THEN '23'	
				WHEN D.CardNo >= CONCAT(@tempNo,'09000') AND D.CardNo <= CONCAT(@tempNo,'09099') THEN '1'	
				WHEN D.CardNo >= CONCAT(@tempNo,'09100') AND D.CardNo <= CONCAT(@tempNo,'09799') THEN '24'
				WHEN D.CardNo >= CONCAT(@tempNo,'09800') AND D.CardNo <= CONCAT(@tempNo,'09999') THEN '25'
				WHEN D.CardNo >= CONCAT(@tempNo,'11000') AND D.CardNo <= CONCAT(@tempNo,'32999') THEN '26'
				WHEN D.CardNo >= CONCAT(@tempNo,'33000') AND D.CardNo <= CONCAT(@tempNo,'54999') THEN '27'
				WHEN D.CardNo >= CONCAT(@tempNo,'55000') AND D.CardNo <= CONCAT(@tempNo,'65999') THEN '28'
				WHEN D.CardNo >= CONCAT(@tempNo,'66000') AND D.CardNo <= CONCAT(@tempNo,'66999') THEN '29'
				WHEN D.CardNo >= CONCAT(@tempNo,'67000') AND D.CardNo <= CONCAT(@tempNo,'76999') THEN '30'
				WHEN D.CardNo >= CONCAT(@tempNo,'77000') AND D.CardNo <= CONCAT(@tempNo,'87999') THEN '31'
				WHEN D.CardNo >= CONCAT(@tempNo,'97000') AND D.CardNo <= CONCAT(@tempNo,'99999') THEN '32'
				WHEN D.CardNo >= CONCAT(@tempNo,'88000') AND D.CardNo <= CONCAT(@tempNo,'92999') THEN '33'
				WHEN D.CardNo >= CONCAT(@tempNo,'93000') AND D.CardNo <= CONCAT(@tempNo,'96999') THEN '34'

				ELSE '35' END AS Serial
		FROM #Data D 
		INNER JOIN Employees E ON D.CardNo = E.CardNo
		WHERE E.CardNo NOT LIKE '14%'
		AND E.IsActive=1
		GROUP BY D.CardNo
		ORDER BY D.CardNo
		
	END
	
		--select * from #TEMP
		SELECT SUM(Total) AS TotalPresent,Sum(NoLine) AS NoLine,SUM(AbsentDays) AS AbsentDays,SUM(Leave) AS Leave,Sum(Late) AS Late,SUM(GrandTotal) AS GrandTotal,SUM(MALE) AS Male,SUM(FEMALE) As Female,Section, CAST(Serial AS INT) AS Code
		From #TEMP
		GROUP BY Serial,Section
		ORDER BY Serial


			

	DROP TABLE #Temp
	DROP TABLE #Data
	DROP TABLE #Absent



END

--EXEC GetAttendanceSummary '01/18/2018','','1','1'