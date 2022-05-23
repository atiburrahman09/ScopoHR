
--INSERT INTO LeaveApplications
SELECT 3, EmployeeID,'2018-06-19','2018-06-19',0,'System','','2018-06-01','Eid_Ul_Fitre','System',GETDATE(),0
FROM Employees
WHERE EmployeeID Not IN (
SELECT EmployeeID FROM LeaveApplications
WHERE LeaveTypeID=4
AND CAST(ToDate AS DATE) > '2018-06-14'
)
AND DepartmentID <> 6
AND IsActive=1

