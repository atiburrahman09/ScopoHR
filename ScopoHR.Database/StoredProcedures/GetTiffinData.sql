--IF EXISTS(SELECT * FROM sys.objects WHERE name='GetTiffinBillData')
--BEGIN
--	DROP PROC GetTiffinBillData
--END
--GO

CREATE PROCEDURE [dbo].[GetTiffinBillData]
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

DELETE FROM #Data WHERE CardNo IN ('4217000','4217001', '4217002', '4217015')
DELETE FROM #Data WHERE CardNo IN ('888266','888314', '888339', '888417', '888822')
DELETE FROM #Data WHERE CardNo IN ('3188006','3188014', '3188028', '3188065', '3188168', '3188221')
DELETE FROM #Data WHERE CardNo IN ('3288157','3288177', '3288196', '3288221', '3288230', '3288343')
DELETE FROM #Data WHERE CardNo IN ('3388170','3388207', '3388285', '3388286')
DELETE FROM #Data WHERE CardNo IN ('1166142')
DELETE FROM #Data WHERE CardNo IN ('3313148')

SELECT A.CardNo, A.EmployeeName, D.DesignationName AS Designation, B.JoinDate, COUNT(*) AS TiffinDays, 20 AS TiffinRate, COUNT(*) * 20 AS PaymentAmount, '' Signature
FROM #Data A
INNER JOIN Employees B ON A.CardNo = B.CardNo
LEFT JOIN Designations D ON B.DesignationID = D.DesignationID
WHERE 
A.CardNo NOT LIKE '14%' 
AND SUBSTRING(REVERSE(A.CardNo), 5, 1) <> '0'
AND A.TotalMinutes >= 11 * 60
GROUP BY A.CardNo, A.EmployeeName, D.DesignationName, B.JoinDate
ORDER BY A.CardNo

DROP TABLE #Data
END

--EXEC GetSalarySheet 11, '3rd Way -4', 1, '', '', ''