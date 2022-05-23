--IF EXISTS(SELECT * FROM sys.objects WHERE name='GetDropedOutEmployeeList')
--BEGIN
--	DROP PROC GetDropedOutEmployeeList
--END
--GO


CREATE PROCEDURE [dbo].[GetDropedOutEmployeeList]
	@fromDate DATETIME,	
	@toDate DATETIME,
	@floor VARCHAR(20) = '',
	@shiftId VARCHAR(10) = ''
AS
BEGIN
	SET NOCOUNT ON;	
	SELECT E.EmployeeName, E.EmployeeID, E.CardNo, E.JoinDate, D.DesignationName AS Designation, I.ApplicableDate AS DropOutDate,
	IIF(I.InActiveType=1,'Dismissed',IIF(I.InActiveType=2,'Resigned','Drop Out')) AS Remarks
	FROM Employees E
    INNER JOIN InactiveEmployees I ON E.EmployeeID=I.EmployeeID
	LEFT JOIN Designations D ON E.DesignationID = D.DesignationID
    INNER JOIN ProductionFloorLines F ON E.ProductionFloorLineID = F.ProductionFloorLineID	
	WHERE CAST(I.ApplicableDate AS DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)
	AND F.Floor=IIF(@floor = '', F.Floor, @floor)
	AND E.ShiftId = @shiftID
END

--EXEC [dbo].[GetDropOutList] '2017-08-16'
--EXEC GetDropedOutEmployeeList '11/2/2017','','1'
--EXEC GetDropedOutEmployeeList '12/31/2017 12:00:00 AM','','1'
