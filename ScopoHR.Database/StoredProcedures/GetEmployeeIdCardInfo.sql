--IF EXISTS(SELECT * FROM sys.objects WHERE name='GetEmployeeIdCardInfo')
--BEGIN
--	DROP PROC GetEmployeeIdCardInfo
--END
--GO
CREATE PROCEDURE [dbo].[GetEmployeeIdCardInfo]	
	@cardNo VARCHAR(15) = ''
AS
BEGIN
	--DECLARE @CardNoVar NVARCHAR(100) = '১৩১৩১৮০'
	SET NOCOUNT ON;

	SELECT E.CardNo,'' AS IssuedDate, '' AS CompanyName, E.EmployeeName,DG.DesignationName AS Designation, DP.DepartmentName AS Department, E.JoinDate AS JoiningDate, E.TicketNo AS Ticket , '' AS WorkingType
	,'' AS Expiry,'' AS Job, E.BloodGroup, '' AS CompanyAddress, E.PermanentAddress, E.MobileNo,E.MobileNo AS EmergencyPhone, E.NID,P.Floor
	FROM Employees E
	INNER JOIN Designations DG ON E.DesignationID = DG.DesignationID
	INNER JOIN Departments DP ON E.DepartmentID = DP.DepartmentID
	INNER JOIN ProductionFloorLines P ON E.ProductionFloorLineID=P.ProductionFloorLineID
	WHERE E.CardNo=@cardNo

END