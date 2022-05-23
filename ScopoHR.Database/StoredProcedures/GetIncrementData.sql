--IF EXISTS(SELECT * FROM sys.objects WHERE name='GetIncrementData')
--BEGIN
--	DROP PROC GetIncrementData
--END
--GO

ALTER PROCEDURE [dbo].[GetIncrementData]
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

	CREATE TABLE #temp(
	CardNo nvarchar(max),
	EmployeeName nvarchar(max),
	Designation nvarchar(max),
	JoiningDate date ,
	IncrementAmount decimal(18,2),
	Gross decimal(18,2),
	Remarks nvarchar(max),
	PreviousIncrementDate date,
	PreviousIncrementAmount decimal(18,2)
	)


	INSERT INTO #temp
	SELECT S.CardNo,S.EmployeeName,S.Designation, S.JoiningDate,S.IncrementAmount,S.Gross,S.Remarks,MAX(SI.IncrementDate) AS PreviousIncrementDate,MAX(SI.IncrementAmount) AS PreviousIncrementAmount FROM 
	(SELECT S.EmployeeID,E.CardNo,E.EmployeeName,D.DesignationName AS Designation,E.JoinDate AS JoiningDate, ((SUM(SM.Amount) - 1100)*5) / 100 AS IncrementAmount, SUM(SM.Amount) AS Gross, '' AS Remarks	
	FROM Employees E
	LEFT JOIN SalaryIncrements S ON S.EmployeeID= E.EmployeeID
	INNER JOIN Designations D ON E.DesignationID = D.DesignationID
	INNER JOIN Departments DP ON E.DepartmentID = DP.DepartmentID
	INNER JOIN ProductionFloorLines P on E.ProductionFloorLineID = P.ProductionFloorLineID
	INNER JOIN SalaryMappings SM ON E.EmployeeID = SM.EmployeeID
	
	WHERE IIF(S.IncrementDate IS NOT NULL, 
	(SELECT MAX(IncrementDate) FROM SalaryIncrements WHERE EmployeeID=E.EmployeeID),CAST(E.JoinDate AS DATE))
	BETWEEN DATEADD(YEAR,-1,CAST(@fromDate AS DATE)) AND DATEADD(YEAR,-1,CAST(@toDate AS DATE))
	AND E.IsActive=1
	AND DP.DepartmentID <> 6
	AND P.Floor=IIF(@floor = '', P.Floor, @floor)
	AND E.ShiftId = IIF(@shiftID = '', E.ShiftID, @shiftID)
	GROUP BY S.EmployeeID, E.CardNo,E.EmployeeName,D.DesignationName,E.JoinDate,S.IncrementAmount,S.IncrementDate
	) AS S
	INNER JOIN SalaryIncrements SI ON SI.EmployeeID= S.EmployeeID
	GROUP BY  S.CardNo,S.EmployeeName,S.Designation, S.JoiningDate,S.IncrementAmount,S.Gross,S.Remarks


	SELECT CardNo,EmployeeName,Designation,JoiningDate,IncrementAmount,Gross,Remarks,PreviousIncrementDate,PreviousIncrementAmount FROM #temp  
	UNION ALL
	SELECT CardNo,EmployeeName,D.DesignationName AS Designation,JoinDate AS JoiningDate,((SUM(SM.Amount) - 1100)*5) / 100 AS IncrementAmount, SUM(SM.Amount) AS Gross, '' AS Remarks, NULL,0
	FROM Employees E
	LEFT JOIN Designations D ON E.DesignationID=D.DesignationID
	LEFT JOIN SalaryMappings SM ON E.EmployeeID = SM.EmployeeID
	LEFT JOIN Departments DP ON E.DepartmentID = DP.DepartmentID
	LEFT JOIN ProductionFloorLines P on E.ProductionFloorLineID = P.ProductionFloorLineID
	WHERE CAST(E.JoinDate as Date) BETWEEN DATEADD(YEAR,-1,CAST(@fromDate AS DATE)) AND DATEADD(YEAR,-1,CAST(@toDate AS DATE))
	AND E.IsActive=1
	AND DP.DepartmentID <> 6
	AND P.Floor=IIF(@floor = '', P.Floor, @floor)
	AND E.ShiftId = IIF(@shiftID = '', E.ShiftID, @shiftID)
	GROUP BY CardNo,EmployeeName,D.DesignationName,JoinDate


	DROP TABLE #temp


END

--EXEC GetIncrementData '2018/01/01', '2018/01/30','', '1','1','','0'
--EXEC GetIncrementData '12/1/2017','1/1/2018','3rd Way -1','1','1','','0'

--PRINT(CAST(DATEADD(YEAR,1,GetDate()) AS DATE) )

--SELECT (CAST(DATEADD(YEAR,1,IIF(CAST('2017-01-01' AS DATE)))  AS DATE))

--SELECT CAST(DATEADD(YEAR,1,'2017-01-01') AS DATE)