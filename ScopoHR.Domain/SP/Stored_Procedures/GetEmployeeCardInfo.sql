USE [ScopoHR]
GO
/****** Object:  StoredProcedure [dbo].[GetEmployeeIdCardInfo]    Script Date: 05/20/2018 2:46:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetEmployeeIdCardInfo]	
	@cardNo VARCHAR(15) = ''
AS
BEGIN
	--DECLARE @CardNoVar NVARCHAR(100) = '১৩১৩১৮০'
	SET NOCOUNT ON;

	--SELECT '১৩১৩১৮০' AS CardNo, '' AS IssedDate, 'অরুণিমা গ্রুপ লিমিটেড, বাংলাদেশ' AS CompanyName, 'মোহাম্মাদ আলী' AS EmployeeName, 'সিনিঃসেলাই মেশিন অপারেটর' AS Designation, 'সেলাই' AS WorkingType, 'সুইংঃ০১' AS Department, '২০১৭-১০-০১' AS JoiningDate, '২০' AS Ticket
	--, '' AS Expiry, '' AS Job, '' AS BloodGroup, 'প্লট নং পুকুরপার, জিরাবো, আশুলিয়া, সাভার, ঢাকা।' AS CompanyAddress, 'মস্তফাপুর, সনাহার, দেবিগঞ্জ, পঞ্চগড়' AS PermanentAddress, '০৯৬১৩১১১০০০' AS Telephone, '০১৭৪৩৩০০৭৫১' AS EmergencyPhone, '১৩৩১৭৭১৩৪৭৬০০০১৩৩' AS NID
	--INTO #temp

	--SELECT * FROM #temp

	SELECT E.CardNo,'' AS IssuedDate, '' AS CompanyName, E.EmployeeName,DG.DesignationName AS Designation, DP.DepartmentName AS Department, E.JoinDate AS JoiningDate, E.TicketNo AS Ticket , '' AS WorkingType
	,'' AS Expiry,'' AS Job, E.BloodGroup, '' AS CompanyAddress, E.PermanentAddress, E.MobileNo,E.MobileNo AS EmergencyPhone, E.NID,P.Floor
	FROM Employees E
	INNER JOIN Designations DG ON E.DesignationID = DG.DesignationID
	INNER JOIN Departments DP ON E.DepartmentID = DP.DepartmentID
	INNER JOIN ProductionFloorLines P ON E.ProductionFloorLineID=P.ProductionFloorLineID
	WHERE E.CardNo=@cardNo

END