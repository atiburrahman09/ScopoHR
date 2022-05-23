IF EXISTS(SELECT * FROM sys.objects WHERE name='GetIncrementData')
BEGIN
	DROP PROC GetIncrementData
END
GO
CREATE PROCEDURE [dbo].[GetIncrementData]
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

	SELECT E.CardNo,E.EmployeeName,D.DesignationName AS Designation,E.JoinDate AS JoiningDate, SUM(SM.Amount) AS Gross, S.IncrementAmount, '' AS Remarks
	FROM SalaryIncrements S
	INNER JOIN Employees E ON S.EmployeeID= E.EmployeeID
	LEFT JOIN Designations D ON E.DesignationID = D.DesignationID
	LEFT JOIN ProductionFloorLines P on E.ProductionFloorLineID = P.ProductionFloorLineID
	LEFT JOIN SalaryMappings SM ON E.EmployeeID = SM.EmployeeID
	AND P.Floor=IIF(@floor = '', P.Floor, @floor)
	AND E.ShiftId = IIF(@shiftID = '', E.ShiftID, @shiftID)
	WHERE CAST(IncrementDate AS DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)
	GROUP BY E.CardNo,E.EmployeeName,D.DesignationName,E.JoinDate,S.IncrementAmount
	ORDER BY E.CardNo

END