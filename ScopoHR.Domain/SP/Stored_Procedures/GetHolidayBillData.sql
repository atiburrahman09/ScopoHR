USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetHolidayBillData]    Script Date: 05/20/2018 2:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetHolidayBillData]
	@fromDate datetime,
	@toDate datetime,
	@floor VARCHAR(200) = '',
	@branchID INT,
	@shiftID VARCHAR(10) = '',
	@cardNo VARCHAR(15) = '',
	@departmentID INT
AS
BEGIN
	SET NOCOUNT ON;

	CREATE TABLE #Data
(
	Day	nvarchar(50),
	Date	date,
	EmployeeID int,
	CardNo	nvarchar(200),
	EmployeeName	nvarchar(500),
	InTime	varchar(20),
	InTimeDate datetime,
	OutTime	varchar(20),
	OutTimeDate datetime,
	Status	varchar(5),
	TotalMinutes int,
	OTMinutes	int,
	LunchTime	int
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
	INSERT INTO #Data
		SELECT A.Day, A.Date,A.EmployeeID, A.CardNo, A.EmployeeName, A.InTime,A.InTimeDate, A.OutTime, A.OutTimeDate,A.Status, A.TotalMinutes AS WorkingHours, A.OTMinutes, A.LunchTime
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
		INSERT INTO #Data
		EXEC GetAttendanceData @fromDate, @toDate, @floor, 1, @shiftID, ''

		UPDATE A
		SET A.OTMinutes = ISNULL(A.TotalMinutes, 0), Status = dbo.GetHolidayStatus(A.Date, A.EmployeeID, '')
		FROM #Data A
		WHERE dbo.GetHolidayStatus(A.Date, A.EmployeeID, @departmentID) IN ('WH', 'HD') 
		AND A.Status IN ('P', 'D')
	END
	



SELECT A.CardNo, A.EmployeeName, D.DesignationName AS Designation, B.JoinDate, COUNT(*) AS HolidayDays, 
CAST(DNB.HolidayAmount AS INT) AS HolidayRate,
CAST(COUNT(*) * DNB.HolidayAmount AS INT) AS PaymentAmount, '' Signature
--INTO #HolidayData
FROM #Data A
INNER JOIN Employees B ON A.CardNo = B.CardNo
LEFT JOIN Designations D ON B.DesignationID = D.DesignationID
LEFT JOIN [Dept.WiseNightBill] DNB ON D.DesignationName=DNB.Designation
WHERE 
A.CardNo NOT LIKE '14%' 
AND B.EmployeeType = 2
AND A.Status IN ('HD','WH')
AND A.TotalMinutes >= 4 * 60
AND DNB.HolidayAmount IS NOT NULL
GROUP BY A.CardNo, A.EmployeeName, D.DesignationName, B.JoinDate,DNB.HolidayAmount

--INSERT INTO #HolidayData
--SELECT A.CardNo, A.EmployeeName, D.DesignationName AS Designation, B.JoinDate, COUNT(*) AS HolidayDays, 
--ROUND((Sm.Amount/104),0) AS HolidayRate,
--CAST((SUM(TotalMinutes)/60) * ROUND((Sm.Amount/104),0) AS INT) AS PaymentAmount, '' Signature
----INTO #HolidayData
--FROM #Data A
--INNER JOIN Employees B ON A.CardNo = B.CardNo
--LEFT JOIN Designations D ON B.DesignationID = D.DesignationID
--LEFT JOIN [Dept.WiseNightBill] DNB ON D.DesignationName=DNB.Designation
--LEFT JOIN SalaryMappings SM ON SM.EmployeeID=B.EmployeeID AND Sm.SalaryTypeID=1
--WHERE 
--A.CardNo NOT LIKE '14%' 
--AND B.EmployeeType = 2
--AND A.Status IN ('HD','WH')
--AND DNB.HolidayAmount IS NULL
--GROUP BY A.CardNo, A.EmployeeName, D.DesignationName, B.JoinDate,DNB.HolidayAmount,Sm.Amount

--SELECT * FROM #HolidayData

DROP TABLE #Data
--DROP TABLE #HolidayData
END

--EXEC GetHolidayBillData '2018-03-01', '2018-03-31','', '1','1','',''
