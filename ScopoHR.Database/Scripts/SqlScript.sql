--IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'GetOfficeInTime')
--BEGIN
--	DROP FUNCTION GetOfficeInTime
--END
--GO
CREATE FUNCTION GetOfficeInTime()
RETURNS TIME(7)
AS
BEGIN
	DECLARE @INTime AS TIME(7)=(SELECT TOP 1 CAST(INTime AS TIME) FROM OfficeTimings
	ORDER BY OfficeTimingId DESC);
	RETURN @INTime;
END;
GO
/*2. create function to get office out-time:*/
IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'GetOfficeOutTime')
BEGIN
	DROP FUNCTION GetOfficeOutTime
END
GO
CREATE FUNCTION GetOfficeOutTime()
RETURNS TIME(7)
AS
BEGIN
	DECLARE @outTime AS TIME(7)=(SELECT TOP 1 cast(OutTime AS TIME) FROM OfficeTimings
	ORDER BY OfficeTimingId DESC);
	RETURN @outTime;
END;
GO
-- Create function GetMonthlyOverTime
IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'GetMonthlyOverTime')
BEGIN
	DROP FUNCTION GetMonthlyOverTime
END
GO
CREATE FUNCTION GetMonthlyOverTime(@year INT, @month INT, @branchId INT)
RETURNS @table TABLE
(CardNo VARCHAR(20), OverTime INT)
AS
BEGIN
	INSERT INTO @table
	SELECT DTT.CardNo, SUM(DTT.OverTime) AS OverTime FROM DailyAttendances DTT
	WHERE DATEPART(YEAR, DTT.Date) = @year AND DATEPART(MONTH, DTT.Date) = @month
	GROUP BY DTT.CardNo
	RETURN
END;
GO
----[Test Start]
--SELECT * FROM dbo.GetMonthlyOverTime(2017, 3, 1) WHERE CardNo = '813868'
----[Test End]



-- Function to return daily attendance 
IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'GetDailyAttendance')
BEGIN
	DROP FUNCTION GetDailyAttendance
END
GO
CREATE FUNCTION GetDailyAttendance(@branchId INT, @date DATETIME)
RETURNS @attendance TABLE
(
	CardNo VARCHAR(20),
	InTime DATETIME,
	OutTime DATETIME	
)
AS
BEGIN	
	INSERT INTO @attendance
	SELECT ATT.CardNo ,
		MIN(ATT.InOutTime) AS InTime, MAX(ATT.InOutTime) AS OutTime FROM Attendances ATT
	INNER JOIN Employees EMP ON ATT.CardNo = EMP.CardNo		
	WHERE EMP.BranchID = @branchId AND CAST(ATT.InOutTime AS DATE) = CAST(@date AS DATE)
	GROUP BY ATT.CardNo, CAST(ATT.InOutTime AS DATE)	
RETURN
END;
GO
IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'GetComparableTime')
BEGIN
	DROP FUNCTION GetComparableTime
END
GO
CREATE FUNCTION GetComparableTime(@date DATETIME)
RETURNS VARCHAR(MAX)
AS 
BEGIN
	DECLARE @res AS VARCHAR(MAX) = (
		SELECT     
		RIGHT('0' + CONVERT([varchar](2), DATEPART(HOUR, @date)), 2) + ':' +
		RIGHT('0' + CONVERT([varchar](2), DATEPART(MINUTE, @date)), 2)
		)
	RETURN @res;
END;
GO

---- [Test Start]
--DECLARE @branchId INT = 1, @date DATETIME = CAST('2017-02-27 8:31:19.000' AS DATETIME);
--DECLARE @officeOutTime DATETIME = dbo.GetOfficeOutTime();
--DECLARE @isHoliday BIT = dbo.IsHoliday(@date) ,  @modifiedBy VARCHAR(20);
--SELECT CAST(@date AS DATETIME) AS [Date], EMP.CardNo, DTT.InTime, DTT.OutTime,
--	IIF(@isHoliday > 0, 'H', 
--		IIF(DTT.InTime IS NULL, 
--			IIF(LApp.Status = 1, IIF(LType.IsPayable > 0, 'PL', 'UP'), 'A')
--		, 
--		  IIF( dbo.GetComparableTime(@date)>= CAST('06:00' AS TIME) AND dbo.GetComparableTime(@date) <= CAST('08:30' AS TIME), 'P', 
--		  IIF(dbo.GetComparableTime(@date)>= CAST('08:31' AS TIME) AND dbo.GetComparableTime(@date) <= CAST('12:30' AS TIME), 'L', 
--		  IIF(dbo.GetComparableTime(@date) >= CAST('12:31' AS TIME), 'EL', 'A'
--		  )
--		))
--	)) AS 'Status', 
--	dbo.CalculateOverTime(DTT.OutTime, @officeOutTime) AS OverTime, @modifiedBy AS ModifiedBy, GETDATE() AS LastModified, 0 AS IsDeleted  
--	FROM dbo.GetDailyAttendance(@branchId, @date) DTT
--RIGHT JOIN Employees EMP ON DTT.CardNo = EMP.CardNo
--LEFT JOIN (
--	SELECT * FROM LeaveApplications LApp
--	WHERE CAST(@date AS DATE) >= CAST(LApp.FromDate AS DATE) AND CAST(@date AS DATE) <= CAST(LApp.ToDate AS DATE)
--) LApp ON EMP.EmployeeID = LApp.EmployeeID
--LEFT JOIN LeaveTypes LType ON LApp.LeaveTypeID = LType.LeaveTypeID
--WHERE EMP.IsActive = 1
---- [Test End]

-- Sync Daily Attendance 
IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'SyncDailyAttendance')
BEGIN
	DROP PROC SyncDailyAttendance
END
GO
CREATE PROCEDURE SyncDailyAttendance @branchId INT, @date DATETIME, @modifiedBy VARCHAR(20)
AS
DECLARE @officeOutTime DATETIME = dbo.GetOfficeOutTime();
DECLARE @isHoliday BIT = dbo.IsHoliday(@date);
INSERT INTO DailyAttendances
([Date], CardNo, InTime, OutTime, [Status], OverTime, ModifiedBy, LastModified, IsDeleted)
SELECT CAST(@date AS DATETIME) AS [Date], EMP.CardNo, DTT.InTime, DTT.OutTime,
	IIF(@isHoliday > 0, 'H', 
		IIF(DTT.InTime IS NULL, 
			IIF(LApp.Status = 1, IIF(LType.IsPayable > 0, 'PL', 'UP'), 'A')
		, 
		  IIF(dbo.GetComparableTime(DTT.InTime) >= CAST('06:00' AS TIME) AND dbo.GetComparableTime(DTT.InTime) <= CAST('08:30' AS TIME), 'P', 
		  IIF(dbo.GetComparableTime(DTT.InTime) >= CAST('08:31' AS TIME) AND dbo.GetComparableTime(DTT.InTime) <= CAST('12:30' AS TIME), 'L', 
		  IIF(dbo.GetComparableTime(DTT.InTime) >= CAST('12:31' AS TIME), 'EL', 'P'
		  )
		))
	)) AS 'Status', 
	dbo.CalculateOverTime(DTT.OutTime, @officeOutTime) AS OverTime, @modifiedBy AS ModifiedBy, GETDATE() AS LastModified, 0 AS IsDeleted
	FROM dbo.GetDailyAttendance(@branchId, @date) DTT
RIGHT JOIN Employees EMP ON DTT.CardNo = EMP.CardNo 
LEFT JOIN (
	SELECT * FROM LeaveApplications LApp
	WHERE CAST(@date AS DATE) >= CAST(LApp.FromDate AS DATE) AND CAST(@date AS DATE) <= CAST(LApp.ToDate AS DATE)
) LApp ON EMP.EmployeeID = LApp.EmployeeID
LEFT JOIN LeaveTypes LType ON LApp.LeaveTypeID = LType.LeaveTypeID
WHERE EMP.IsActive = 1
GO

----[Test Start]
----	DECLARE @date DATETIME = CAST('2017-04-07' AS DATETIME)
----	EXEC SyncDailyAttendance 1, @date, 'System'
----[Test End]

-- Create function CalculateOverTime(@employeeOutTime, @officeOutTime)
IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'CalculateOverTime')
BEGIN
	DROP FUNCTION CalculateOverTime
END
GO
CREATE FUNCTION CalculateOverTime(@employeeOutTime DATETIME, @officeOutTime DATETIME)
RETURNS INT
AS
BEGIN
DECLARE @hour INT = 
	(SELECT IIF(
	DATEDIFF(MINUTE, 
		CAST(DATEADD(MINUTE, 30, CAST(@officeOutTime AS TIME)) AS TIME),
		CAST(@employeeOutTime AS TIME)
	) >= 0,
	DATEDIFF(MINUTE,
		CAST(DATEADD(MINUTE, 30, CAST(@officeOutTime AS TIME)) AS TIME),
		CAST(@employeeOutTime AS TIME)
	), 0
))
RETURN CEILING(CAST(@hour AS FLOAT)/CAST(60 AS FLOAT));
END;
GO

-- Create function GetHolidayIdByDate(@date)
IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'GetHolidayIdByDate')
BEGIN
	DROP FUNCTION GetHolidayIdByDate
END
GO
CREATE FUNCTION GetHolidayIdByDate(@date DATETIME)
RETURNS INT
AS
BEGIN
	DECLARE @id INT = 
		(SELECT HLD.HolidayId FROM Holidays HLD
	WHERE CAST(@date AS DATE) >= CAST(HLD.FromDate AS DATE) AND CAST(@date AS DATE) <= CAST(HLD.ToDate AS DATE))
RETURN @id
END
GO
-- Create function IsHoliday(@date)
IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'IsHoliday')
BEGIN
	DROP FUNCTION IsHoliday
END
GO
CREATE FUNCTION IsHoliday(@date DATETIME)
RETURNS BIT
AS
BEGIN	
	DECLARE @isHoliday BIT = IIF(dbo.GetHolidayIdByDate(@date) > 0, 1, 0)
RETURN @isHoliday;
END
GO

------ [Test Start]
--SELECT dbo.IsHoliday(CAST('2017-04-06' AS DATE))
------ [Test End]

-- Insert monthly salary
IF EXISTS( SELECT * FROM Sys.objects WHERE name = 'CalculateMonthlySalary')
BEGIN
	DROP PROC CalculateMonthlySalary
END
GO
CREATE PROCEDURE CalculateMonthlySalary( @year INT,@month INT, @minutes INT, @branchId INT, @preparedBy VARCHAR(20))
AS
	INSERT INTO MonthlySalaries
	([Month],EmployeeId,IsTaken,PreparedBy,PreparedDate, [Year],ModifiedBy,LastModified,IsDeleted, BranchID,Amount, SalaryType)		
	SELECT  @month AS [Month], EMP.EmployeeID, 0 AS IsTaken, @preparedBy AS PreparedBy, GETDATE() AS PreparedBy,
		@year AS [Year], @preparedBy AS ModifiedBy,GETDATE() AS LastModified,
		0 AS IsDeleted, @branchId AS BranchID,
		CASE 
			WHEN STP.SalaryTypeID = 3
				THEN OVT.OverTime*SMP.Amount
			WHEN STP.SalaryTypeID = 1
				THEN CAST(SMP.Amount-(SUB.AbsentDays*SMP.Amount/26) AS FLOAT)
			WHEN STP.SalaryTypeID = 4 
				THEN IIF(SUB.AbsentDays > 0, SMP.Amount * 0, SMP.Amount)
			ELSE SMP.Amount
			END AS Amount,
		STP.SalaryTypeID AS SalaryType
	FROM dbo.GetMonthlyOverTime(2017, 03, 1) OVT
	RIGHT JOIN Employees EMP ON OVT.CardNo = EMP.CardNo	
	LEFT JOIN Designations DSG ON EMP.DesignationID = DSG.DesignationID
	LEFT JOIN SalaryMappings SMP ON EMP.EmployeeID = SMP.EmployeeID
	LEFT JOIN SalaryTypes STP ON SMP.SalaryTypeID = STP.SalaryTypeID
	LEFT JOIN (
		SELECT CardNo, COUNT(*) AS AbsentDays FROM DailyAttendances
		WHERE Status = 'UP' OR Status = 'A' AND DATEPART(YEAR, Date) = 2017 AND DATEPART(MONTH, Date) = 3
		GROUP BY CardNo
	) SUB ON EMP.CardNo = SUB.CardNo
GO

----[Test Start]
--DECLARE @month INT = 3, @preparedBy VARCHAR(20) = 'PreparedBy', @year INT = 2017,  @branchId INT = 1
--	--INSERT INTO MonthlySalaries
--	--([Month],EmployeeId,IsTaken,PreparedBy,PreparedDate, [Year],ModifiedBy,LastModified,IsDeleted, BranchID,Amount, SalaryType)		
--	SELECT  @month AS [Month], EMP.EmployeeID, 0 AS IsTaken, @preparedBy AS PreparedBy, GETDATE() AS PreparedBy,
--		@year AS [Year], @preparedBy AS ModifiedBy,GETDATE() AS LastModified,
--		0 AS IsDeleted, @branchId AS BranchID,
--		CASE 
--			WHEN STP.SalaryTypeID = 3
--				THEN OVT.OverTime*SMP.Amount
--			WHEN STP.SalaryTypeID = 1
--				THEN CAST(SMP.Amount-(SUB.AbsentDays*SMP.Amount/26) AS FLOAT)
--			WHEN STP.SalaryTypeID = 4 
--				THEN IIF(SUB.AbsentDays > 0, SMP.Amount * 0, SMP.Amount)
--			ELSE SMP.Amount
--			END AS Amount,
--		STP.SalaryTypeID AS SalaryType
--	FROM dbo.GetMonthlyOverTime(2017, 03, 1) OVT
--	RIGHT JOIN Employees EMP ON OVT.CardNo = EMP.CardNo	
--	LEFT JOIN Designations DSG ON EMP.DesignationID = DSG.DesignationID
--	LEFT JOIN SalaryMappings SMP ON EMP.EmployeeID = SMP.EmployeeID
--	LEFT JOIN SalaryTypes STP ON SMP.SalaryTypeID = STP.SalaryTypeID
--	LEFT JOIN (
--		SELECT CardNo, COUNT(*) AS AbsentDays FROM DailyAttendances
--		WHERE Status = 'UP' OR Status = 'A' AND DATEPART(YEAR, Date) = @year AND DATEPART(MONTH, Date) = @month
--		GROUP BY CardNo
--	) SUB ON EMP.CardNo = SUB.CardNo	
--	WHERE Amount IS NOT NULL
----[Test End]







IF EXISTS(SELECT * FROM Sys.objects WHERE name = 'GetAttendanceReport')
BEGIN 
	DROP PROCEDURE GetAttendanceReport
END
GO

CREATE PROCEDURE GetAttendanceReport
    -- Add the parameters for the stored procedure here
    @fromDate datetime,
    @toDate datetime ,
    @floor nvarchar(10),
    @branchID int
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON; 
    IF(@floor = '')
        BEGIN
            
            WITH OverTime (CardNo, EmployeeName, CumulativeHour)
                AS
                -- Define the CTE query.
                (
                    SELECT A.CardNo, A.EmployeeName, SUM(A.CumulativeHour) as CumulativeHour
                    FROM
                    (
                    SELECT 
                        (CONVERT(float,datediff(minute,CAST(MIN(InOutTime) as time), CAST(MAX(InOutTime) as time)))/(60))  as CumulativeHour, 
                        At.CardNo,Emp.EmployeeName, Cast(InOutTime as date) AS DATE
                        FROM Attendances AT
                        LEFT JOIN Employees EMP on AT.CardNo = EMP.CardNo   
                        WHERE CAST(AT.InOutTime as Date) >=CAST(@fromDate as Date)  and CAST(AT.InOutTime as Date) <= @toDate
                        GROUP BY  AT.CardNo,EMP.EmployeeName, Cast(InOutTime as date)
                    ) AS A
                    GROUP BY A.CardNo, A.EmployeeName
                )
            -- Define the outer query referencing the CTE name.
                    SELECT AT.CardNo, EMP.DepartmentID , DEP.DepartmentName, EMP.EmployeeName, AT.ModifiedBy, round(CumulativeHour,2) as CumulativeHour,
                    CAST(MIN(InOutTime) as datetime) AS InTime,
                    CASE
                    WHEN CAST(MIN(InOutTime) as datetime) = CAST(MAX(InOutTime) as datetime) then NULL 
                    else CAST(MAX(InOutTime) as datetime) 
                    END AS OutTime ,
                    round(CONVERT(float,DATEDIFF(MINUTE,CAST(MIN(InOutTime) as datetime), CAST(MAX(InOutTime) as datetime)))/60,2) as TotalHour
                    FROM OverTime
                    INNER JOIN Attendances AT ON OverTime.CardNo=AT.CardNo
                    LEFT JOIN Employees EMP on AT.CardNo = EMP.CardNo
                    LEFT JOIN Departments DEP on EMP.DepartmentID = DEP.DepartmentID
                    WHERE CAST(AT.InOutTime as Date)=CAST(@toDate as Date)
                    GROUP BY  AT.CardNo, EMP.EmployeeName, EMP.DepartmentID , DEP.DepartmentName, AT.ModifiedBy, OverTime.CumulativeHour
        END
    ELSE
        BEGIN
        WITH OverTime (CardNo, EmployeeName, CumulativeHour)
                AS
                -- Define the CTE query.
                (
                    SELECT A.CardNo, A.EmployeeName, SUM(A.CumulativeHour) as CumulativeHour
                    FROM
                    (
                    SELECT 
                        (CONVERT(float,datediff(minute,CAST(MIN(InOutTime) as time), CAST(MAX(InOutTime) as time)))/(60))  as CumulativeHour, 
                        At.CardNo,Emp.EmployeeName, Cast(InOutTime as date) AS DATE
                        FROM Attendances AT
                        LEFT JOIN Employees EMP on AT.CardNo = EMP.CardNo   
                        WHERE CAST(AT.InOutTime as Date) >=CAST(@fromDate as Date)  and CAST(AT.InOutTime as Date) <= @toDate
                        GROUP BY  AT.CardNo,EMP.EmployeeName, Cast(InOutTime as date)
                    ) AS A
                    GROUP BY A.CardNo, A.EmployeeName
                )
            -- Define the outer query referencing the CTE name.
                    SELECT AT.CardNo , EMP.DepartmentID , DEP.DepartmentName, EMP.EmployeeName, AT.ModifiedBy, round(CumulativeHour,2) as CumulativeHour,
                    CAST(MIN(InOutTime) as datetime) AS InTime,
                    CASE
                    WHEN CAST(MIN(InOutTime) as datetime) = CAST(MAX(InOutTime) as datetime) then NULL 
                    else CAST(MAX(InOutTime) as datetime) 
                    END AS OutTime ,
                    round(CONVERT(float,DATEDIFF(MINUTE,CAST(MIN(InOutTime) as datetime), CAST(MAX(InOutTime) as datetime)))/60,2) as TotalHour
                    FROM OverTime
                    INNER JOIN Attendances AT ON OverTime.CardNo=AT.CardNo
                    LEFT JOIN Employees EMP on AT.CardNo = EMP.CardNo
                    LEFT JOIN Departments DEP on EMP.DepartmentID = DEP.DepartmentID
                    LEFT JOIN ProductionFloorLines P on EMP.ProductionFloorLineID = P.ProductionFloorLineID
                    WHERE CAST(AT.InOutTime as Date)=CAST(@toDate as Date)
                    AND P.[Floor] = @floor
                    GROUP BY  AT.CardNo, EMP.EmployeeName , EMP.DepartmentID , DEP.DepartmentName, AT.ModifiedBy, OverTime.CumulativeHour
    END   
END
GO


IF EXISTS(SELECT * FROM Sys.objects WHERE name = 'GetSalarySheet')
BEGIN
	DROP PROCEDURE GetSalarySheet
END
GO
CREATE PROCEDURE GetSalarySheet
    -- Add the parameters for the stored procedure here
    @Month INT,
    @Year nvarchar(10),
    @DepartmentID int,
    @Floor nvarchar(10),
    @BranchId int
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    SELECT EMP.EmployeeID, EMP.CardNo, EMP.EmployeeName, DEP.DepartmentName,SUM(SM.Amount) as Salary
    FROM Employees EMP
    INNER JOIN MonthlySalaries SM ON SM.EmployeeID = EMP.EmployeeID
    INNER JOIN Departments DEP ON EMP.DepartmentID = DEP.DepartmentID
	LEFT JOIN ProductionFloorLines PFL ON EMP.ProductionFloorLineID = PFL.ProductionFloorLineID
    where SM.[Month] = @Month
    AND SM.[Year] = @Year
    AND EMP.DepartmentID = IIF(@DepartmentID = 0, EMP.DepartmentID, @DepartmentID)
    AND PFL.Floor = IIF(@Floor = '', PFL.[Floor], @Floor)
    AND SM.BranchID = @BranchId
    GROUP By EMP.EmployeeID, EMP.CardNo, EMP.EmployeeName, DEP.DepartmentName
END
GO


