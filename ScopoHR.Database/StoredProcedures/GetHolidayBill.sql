--IF EXISTS(SELECT * FROM sys.objects WHERE name='GetHolidayBillData')
--BEGIN
--	DROP PROC GetHolidayBillData
--END
--GO

CREATE PROCEDURE [dbo].[GetHolidayBillData]
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
	
INSERT INTO #Data
EXEC GetAttendanceData @fromDate, @toDate, @floor, 1, @shiftID, ''

UPDATE A
SET A.OTMinutes = ISNULL(A.TotalMinutes, 0), Status = dbo.GetHolidayStatus(A.Date, A.EmployeeID, '')
FROM #Data A
WHERE dbo.GetHolidayStatus(A.Date, A.EmployeeID, @departmentID) IN ('WH', 'HD') 
AND A.Status IN ('P', 'D')

--select * from #Data
--where Status in ('HD','WH')
--RETURN ;



SELECT A.CardNo, A.EmployeeName, D.DesignationName AS Designation, B.JoinDate, COUNT(*) AS HolidayDays, 
CAST(DNB.HolidayAmount AS INT) AS HolidayRate,
CAST(COUNT(*) * DNB.HolidayAmount AS INT) AS PaymentAmount, '' Signature
INTO #HolidayData
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

INSERT INTO #HolidayData
SELECT A.CardNo, A.EmployeeName, D.DesignationName AS Designation, B.JoinDate, COUNT(*) AS HolidayDays, 
ROUND((Sm.Amount/104),0) AS HolidayRate,
CAST((SUM(TotalMinutes)/60) * ROUND((Sm.Amount/104),0) AS INT) AS PaymentAmount, '' Signature
--INTO #HolidayData
FROM #Data A
INNER JOIN Employees B ON A.CardNo = B.CardNo
LEFT JOIN Designations D ON B.DesignationID = D.DesignationID
LEFT JOIN [Dept.WiseNightBill] DNB ON D.DesignationName=DNB.Designation
LEFT JOIN SalaryMappings SM ON SM.EmployeeID=B.EmployeeID
WHERE 
A.CardNo NOT LIKE '14%' 
AND B.EmployeeType = 2
AND A.Status IN ('HD','WH')
AND DNB.HolidayAmount IS NULL
AND Sm.SalaryTypeID=1
GROUP BY A.CardNo, A.EmployeeName, D.DesignationName, B.JoinDate,DNB.HolidayAmount,Sm.Amount

SELECT * FROM #HolidayData

DROP TABLE #Data
DROP TABLE #HolidayData
END

--EXEC GetHolidayBillData '2018-03-01', '2018-03-31','', '1','1','',''