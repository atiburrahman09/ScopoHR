--IF EXISTS(SELECT * FROM sys.objects WHERE name='GetComparetiveStatement')
--BEGIN
--	DROP PROC GetComparetiveStatement
--END
--GO
CREATE PROCEDURE [dbo].[GetComparetiveStatement]
	@firstMonth INT,
	@secondMonth INT,
	@floor VARCHAR(200)='',
	@branchID INT,
	@shiftID VARCHAR(10) = '',
	@firstYear INT,
	@secondYear INT
	
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @tempNo varchar(10);
	IF(@floor = '3rd Way -1')
	BEGIN
		SELECT @tempNo='31';
	END

	IF(@floor = '3rd Way -2')
	BEGIN
		SELECT @tempNo='32';
	END

	IF(@floor = '3rd Way -3')
	BEGIN
		SELECT @tempNo='33';
	END

	IF(@floor = '3rd Way -4')
	BEGIN
		SELECT @tempNo='8';
	END
	
	IF(@floor = 'Knit -A')
	BEGIN
		SELECT @tempNo='13';
	END
	
	IF(@floor = 'Knit -B')
	BEGIN
		SELECT @tempNo='11';
	END

	IF(@floor = 'All Common Office')
	BEGIN
		SELECT @tempNo='17';
	END
	IF(@floor = '3rd Way Common')
	BEGIN
		SELECT @tempNo='30';
	END

	IF(@floor = '3rd Way Finishing')
	BEGIN
		SELECT @tempNo='35';
	END

	IF(@floor = 'BDC')
	BEGIN
		SELECT @tempNo='34';
	END

	IF(@floor = 'Night Shit All Common')
	BEGIN
		SELECT @tempNo='42';
	END

	IF(@floor = 'Night Shit All Common')
	BEGIN
		SELECT @tempNo='42';
	END

	IF(@floor = '3rd Way Cutting Night Shift')
	BEGIN
		SELECT @tempNo='42';
	END

	IF(@floor = 'ASWL Common')
	BEGIN
		SELECT @tempNo='40';
	END

	IF(@floor = 'ASWL Night Shift')
	BEGIN
		SELECT @tempNo='41';
	END

  CREATE TABLE #TEMP
	(
		Code Nvarchar(10),
		Bonus decimal(18,2),
		Hours decimal(18,2),
		Salary_Wages decimal(18,2),
		OTAmount decimal(18,2),
		TotalPay decimal(18,2),		
		Section Nvarchar(Max),
		Month INT,
		Year INT
	)

		INSERT INTO #TEMP 
			SELECT LEFT(M.CardNo,4),SUM(M.AttendanceBonus) AS Bonus, SUM(M.OTH) / 60 AS Hours, SUM(PayableWages) AS Salary_Wages, SUM(OTTaka) AS OTAmount, SUM(TotalPay) AS TotalPay,
			CASE 
				WHEN M.CardNo >= CONCAT(@tempNo,'01000') AND M.CardNo <= CONCAT(@tempNo,'01999') THEN 'Factory Management' 
				WHEN M.CardNo >= CONCAT(@tempNo,'02000') AND M.CardNo <= CONCAT(@tempNo,'02099') THEN 'Engineering/Data Entry'
				WHEN M.CardNo >= CONCAT(@tempNo,'02100') AND M.CardNo <= CONCAT(@tempNo,'02199') THEN 'Factory Management'
				WHEN M.CardNo >= CONCAT(@tempNo,'02200') AND M.CardNo <= CONCAT(@tempNo,'02399') THEN 'Production Staff Swing'
				WHEN M.CardNo >= CONCAT(@tempNo,'02400') AND M.CardNo <= CONCAT(@tempNo,'02499') THEN 'Production Staff Cutting'
				WHEN M.CardNo >= CONCAT(@tempNo,'02500') AND M.CardNo <= CONCAT(@tempNo,'02599') THEN 'Production Staff Finishing'
				WHEN M.CardNo >= CONCAT(@tempNo,'02600') AND M.CardNo <= CONCAT(@tempNo,'02699') THEN 'Store' 
				WHEN M.CardNo >= CONCAT(@tempNo,'02700') AND M.CardNo <= CONCAT(@tempNo,'02799') THEN 'Q.C' 
				WHEN M.CardNo >= CONCAT(@tempNo,'02800') AND M.CardNo <= CONCAT(@tempNo,'02899') THEN 'Production Staff Swing' 
				WHEN M.CardNo >= CONCAT(@tempNo,'03100') AND M.CardNo <= CONCAT(@tempNo,'03499') THEN 'Production Staff Swing'
				WHEN M.CardNo >= CONCAT(@tempNo,'03500') AND M.CardNo <= CONCAT(@tempNo,'03599') THEN 'Production Staff Cutting'
				WHEN M.CardNo >= CONCAT(@tempNo,'03600') AND M.CardNo <= CONCAT(@tempNo,'03699') THEN 'Production Staff Finishing'
				WHEN M.CardNo >= CONCAT(@tempNo,'03800') AND M.CardNo <= CONCAT(@tempNo,'03999') THEN 'Production Staff Cutting'
				WHEN M.CardNo >= CONCAT(@tempNo,'04100') AND M.CardNo <= CONCAT(@tempNo,'04999') THEN 'Sample Machinist'
				WHEN M.CardNo >= CONCAT(@tempNo,'05199') AND M.CardNo <= CONCAT(@tempNo,'05499') THEN 'Mechanical' 
				WHEN M.CardNo >= CONCAT(@tempNo,'05500') AND M.CardNo <= CONCAT(@tempNo,'05899') THEN 'Electrical'				
				WHEN M.CardNo >= CONCAT(@tempNo,'06100') AND M.CardNo <= CONCAT(@tempNo,'06199') THEN 'Administration (Pure)'
				WHEN M.CardNo >= CONCAT(@tempNo,'06200') AND M.CardNo <= CONCAT(@tempNo,'06299') THEN 'Welfare_Medical'   
				WHEN M.CardNo >= CONCAT(@tempNo,'06300') AND M.CardNo <= CONCAT(@tempNo,'06399') THEN 'Compliance'
				WHEN M.CardNo >= CONCAT(@tempNo,'06400') AND M.CardNo <= CONCAT(@tempNo,'06499') THEN 'IT' 
				WHEN M.CardNo >= CONCAT(@tempNo,'06500') AND M.CardNo <= CONCAT(@tempNo,'06599') THEN 'Time Keeper' 
				WHEN M.CardNo >= CONCAT(@tempNo,'06600') AND M.CardNo <= CONCAT(@tempNo,'06699') THEN 'Store'
				WHEN M.CardNo >= CONCAT(@tempNo,'06700') AND M.CardNo <= CONCAT(@tempNo,'06799') THEN 'Driver'
				WHEN M.CardNo >= CONCAT(@tempNo,'06800') AND M.CardNo <= CONCAT(@tempNo,'06899') THEN 'Peon'
				WHEN M.CardNo >= CONCAT(@tempNo,'06900') AND M.CardNo <= CONCAT(@tempNo,'06999') THEN 'Loader/Cleaner'				
				WHEN M.CardNo >= CONCAT(@tempNo,'07000') AND M.CardNo <= CONCAT(@tempNo,'07999') THEN 'Accounts' 
				WHEN M.CardNo >= CONCAT(@tempNo,'08100') AND M.CardNo <= CONCAT(@tempNo,'08699') THEN 'Security'				
				WHEN M.CardNo >= CONCAT(@tempNo,'08700') AND M.CardNo <= CONCAT(@tempNo,'08899') THEN 'Checker'		
				WHEN M.CardNo >= CONCAT(@tempNo,'08900') AND M.CardNo <= CONCAT(@tempNo,'08999') THEN 'Compliance'	
				WHEN M.CardNo >= CONCAT(@tempNo,'09000') AND M.CardNo <= CONCAT(@tempNo,'09099') THEN 'Factory Management'	
				WHEN M.CardNo >= CONCAT(@tempNo,'09100') AND M.CardNo <= CONCAT(@tempNo,'09799') THEN 'Merchandising'
				WHEN M.CardNo >= CONCAT(@tempNo,'09800') AND M.CardNo <= CONCAT(@tempNo,'09999') THEN 'Commercial'
				WHEN M.CardNo >= CONCAT(@tempNo,'11000') AND M.CardNo <= CONCAT(@tempNo,'32999') THEN 'Operator'
				WHEN M.CardNo >= CONCAT(@tempNo,'33000') AND M.CardNo <= CONCAT(@tempNo,'54999') THEN 'Asst Swing Machine Operator'
				WHEN M.CardNo >= CONCAT(@tempNo,'55000') AND M.CardNo <= CONCAT(@tempNo,'65999') THEN 'Iron Man (S)'
				WHEN M.CardNo >= CONCAT(@tempNo,'66000') AND M.CardNo <= CONCAT(@tempNo,'66999') THEN 'Iron Man (F)'
				WHEN M.CardNo >= CONCAT(@tempNo,'67000') AND M.CardNo <= CONCAT(@tempNo,'76999') THEN 'Packer, Folder And Finishing Asst.'
				WHEN M.CardNo >= CONCAT(@tempNo,'77000') AND M.CardNo <= CONCAT(@tempNo,'87999') THEN 'Asst.Cutter'
				WHEN M.CardNo >= CONCAT(@tempNo,'97000') AND M.CardNo <= CONCAT(@tempNo,'99999') THEN 'Quality(C)'
				WHEN M.CardNo >= CONCAT(@tempNo,'88000') AND M.CardNo <= CONCAT(@tempNo,'92999') THEN 'Quality(S)'
				WHEN M.CardNo >= CONCAT(@tempNo,'93000') AND M.CardNo <= CONCAT(@tempNo,'96999') THEN 'Quality(F)'

				ELSE 'Other' END AS Section, M.Month,M.Year
		FROM MonthlySalaries M 
		LEFT JOIN Employees E ON M.CardNo = E.CardNo
		LEFT JOIN ProductionFloorLines P on E.ProductionFloorLineID = P.ProductionFloorLineID
		WHERE ((M.Month = @firstMonth AND M.Year=@firstYear) OR (M.Month=@secondMonth AND M.Year=@secondYear))
		AND P.Floor=IIF(@floor = '', P.Floor, @floor)
		AND E.ShiftId = IIF(@shiftID = '', E.ShiftID, @shiftID)
		--AND E.IsActive=1
		GROUP BY M.Month,M.CardNo,M.Year
		
		--select * from #TEMP
		SELECT Code,Section,SUM(Bonus) AS Bonus,Sum(Hours) AS Hours,SUM(Salary_Wages) AS Salary_Wages,SUM(OTAmount) AS OTAmount,Sum(TotalPay) AS TotalPay,Month, Count(*) AS TotalPerson,Year
			From #TEMP
			GROUP BY Section,Month,Code,Year
				ORDER BY YEAR ASC
			

	DROP TABLE #Temp
END

--EXEC GetComparetiveStatement 12,1,'3rd Way -1','1','1'

--SELECT *From MonthlySalaries