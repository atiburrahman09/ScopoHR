USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetLeaveApprovalData]    Script Date: 7/12/2018 7:07:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetLeaveApprovalData] (@leaveApplicationID int)
AS 
BEGIN
	SELECT E.EmployeeName,E.CardNo,D.DesignationName AS Designation, Dept.DepartmentName AS Department, l.ApprovalDate,DATEADD(DAY,1,CAST(l.ToDate AS date)) As JoinDate, DATEDIFF(D,CAST(l.ToDate As date),CAST(l.FromDate As date)) + 2 AS LeaveDays 
	FROM LeaveApplications l
	INNER JOIN Employees E ON l.EmployeeID= E.EmployeeID
	INNER JOIN Designations D ON E.DesignationID=D.DesignationID
	INNER JOIN Departments Dept ON E.DepartmentID=Dept.DepartmentID
	WHERE l.LeaveApplicationID=@leaveApplicationID
END

--EXEC GetLeaveApprovalData '65935'
