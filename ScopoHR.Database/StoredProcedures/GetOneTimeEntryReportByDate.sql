--IF EXISTS(SELECT * FROM Sys.objects WHERE name = 'GetOneTimeEntryReportByDate')
--BEGIN
--	DROP PROCEDURE GetOneTimeEntryReportByDate
--END
--GO


CREATE PROCEDURE [dbo].[GetOneTimeEntryReportByDate]
	@FromDate as DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	SELECT D.CardNo, E.EmployeeName as NameOfEmployee , Desg.DesignationName as Designation , 
	CONVERT(VARCHAR(5), CAST(D.InTime as TIME))
	AS InTime , CONVERT(VARCHAR(5), CAST(D.OutTime as TIME)) AS OutTime, @FromDate as [Date]
	FROM DailyAttendances D 
	INNER JOIN Employees E ON D.CardNo=E.CardNo
	INNER JOIN Designations Desg ON  E.DesignationID = Desg.DesignationID
	Where CAST (D.Date as DATE)=CAST(@FromDate as DATE)
	AND D.InTime is not null
	AND D.OutTime is null

END