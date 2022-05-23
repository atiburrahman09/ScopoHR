--IF EXISTS(SELECT * FROM sys.objects WHERE name='GetNightBillData')
--BEGIN
--	DROP PROC GetNightBillData
--END
--GO



CREATE PROCEDURE [dbo].[GetNightBillData]
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

SELECT A.CardNo, A.EmployeeName, D.DesignationName AS Designation, B.JoinDate, COUNT(*) AS TiffinDays, 40 AS TiffinRate, COUNT(*) * 40 AS PaymentAmount, '' Signature
FROM #Data A
INNER JOIN Employees B ON A.CardNo = B.CardNo
LEFT JOIN Designations D ON B.DesignationID = D.DesignationID
WHERE 
A.CardNo NOT LIKE '14%' 
AND B.EmployeeType = 1
--AND A.TotalMinutes >= 14 * 60
AND A.OutTime >= '23:25'
GROUP BY A.CardNo, A.EmployeeName, D.DesignationName, B.JoinDate
--ORDER BY A.CardNo

UNION ALL

SELECT A.CardNo, A.EmployeeName, D.DesignationName AS Designation, B.JoinDate, COUNT(*) AS TiffinDays, CAST(DNB.NightBillAmount AS INT) AS TiffinRate, CAST(COUNT(*) * DNB.NightBillAmount AS INT) AS PaymentAmount, '' Signature
FROM #Data A
INNER JOIN Employees B ON A.CardNo = B.CardNo
LEFT JOIN Designations D ON B.DesignationID = D.DesignationID
LEFT JOIN [Dept.WiseNightBill] DNB ON D.DesignationName=DNB.Designation
WHERE 
A.CardNo NOT LIKE '14%' 
AND B.EmployeeType = 2
--AND A.TotalMinutes >= 14 * 60
AND A.OutTime >= '22:25'
GROUP BY A.CardNo, A.EmployeeName, D.DesignationName, B.JoinDate,DNB.NightBillAmount

DROP TABLE #Data
END

--EXEC GetNightBillData '2018-02-01', '2018-02-28','', '1','1','',''