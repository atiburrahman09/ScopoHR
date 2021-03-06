--IF EXISTS(SELECT * FROM Sys.objects WHERE name = 'GetLeaveDetailsByEmployee')
--BEGIN
--	DROP PROCEDURE GetLeaveDetailsByEmployee
--END
--GO



CREATE PROCEDURE [dbo].[GetLeaveDetailsByEmployee] (@cardNo as nvarchar(max))
AS 

SELECT E.CardNo, E.EmployeeName, D.DesignationName,L.FromDate,L.ToDate , DATEDIFF(DAY,Cast(L.FromDate as Date),Cast(L.ToDate as Date)) + 1 as LeaveDays, LT.LeaveTypeName
FROM Employees E
LEFT JOIN Designations D ON E.DesignationID=D.DesignationID
LEFT JOIN LeaveApplications L ON E.EmployeeID=L.EmployeeID
LEFT JOIN LeaveTypes LT ON L.LeaveTypeID=LT.LeaveTypeID

WHERE E.CardNo=@cardNo
