USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetNightBillData]    Script Date: 05/20/2018 2:48:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetNightBillData]
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
	END




--DELETE FROM #Data WHERE CardNo IN ('4217000','4217001', '4217002', '4217015')
--DELETE FROM #Data WHERE CardNo IN ('888266','888314', '888339', '888417', '888822')
--DELETE FROM #Data WHERE CardNo IN ('3188006','3188014', '3188028', '3188065', '3188168', '3188221')
--DELETE FROM #Data WHERE CardNo IN ('3288157','3288177', '3288196', '3288221', '3288230', '3288343')
--DELETE FROM #Data WHERE CardNo IN ('3388170','3388207', '3388285', '3388286')
--DELETE FROM #Data WHERE CardNo IN ('1166142')
--DELETE FROM #Data WHERE CardNo IN ('3313148')

SELECT A.CardNo, A.EmployeeName, D.DesignationName AS Designation, B.JoinDate, COUNT(*) AS TiffinDays, 40 AS TiffinRate, COUNT(*) * 40 AS PaymentAmount, '' Signature
FROM #Data A
INNER JOIN Employees B ON A.CardNo = B.CardNo AND B.ShiftId=1
LEFT JOIN Designations D ON B.DesignationID = D.DesignationID
WHERE 
A.CardNo NOT LIKE '14%' 
AND B.EmployeeType = 1
--AND B.ShiftId=1
--AND A.TotalMinutes >= 14 * 60
AND A.OutTime >= '23:25' OR (A.OutTime BETWEEN '00:00' AND '06:00')
GROUP BY A.CardNo, A.EmployeeName, D.DesignationName, B.JoinDate
--ORDER BY A.CardNo

UNION ALL

SELECT A.CardNo, A.EmployeeName, D.DesignationName AS Designation, B.JoinDate, COUNT(*) AS TiffinDays, CAST(DNB.NightBillAmount AS INT) AS TiffinRate, CAST(COUNT(*) * DNB.NightBillAmount AS INT) AS PaymentAmount, '' Signature
FROM #Data A
INNER JOIN Employees B ON A.CardNo = B.CardNo AND B.ShiftId=1
LEFT JOIN Designations D ON B.DesignationID = D.DesignationID
LEFT JOIN [Dept.WiseNightBill] DNB ON D.DesignationName=DNB.Designation
WHERE 
A.CardNo NOT LIKE '14%' 
AND B.EmployeeType = 2
--AND B.ShiftId=1
--AND A.TotalMinutes >= 14 * 60
AND A.OutTime >= '22:25'
GROUP BY A.CardNo, A.EmployeeName, D.DesignationName, B.JoinDate,DNB.NightBillAmount

DROP TABLE #Data
END

--EXEC GetNightBillData '2018-03-01', '2018-03-31','', '1','2','',''
