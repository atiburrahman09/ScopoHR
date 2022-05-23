--IF EXISTS(SELECT * FROM sys.objects WHERE name='GetDropedNewJoinEmployeeInfoList')
--BEGIN
--	DROP PROC GetDropedNewJoinEmployeeInfoList
--END
--GO



CREATE PROCEDURE [dbo].[GetDropedNewJoinEmployeeInfoList]
	@fromDate DATETIME,	
	@toDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;	

	--CREATE TABLE #Temp
	--(
	--	Salary INT,
	--	DesignationName NVARCHAR(MAX),
	--	CardNo NVARCHAR(MAX),
	--	Floor NVARCHAR(MAX),
	--	Type INT
	--)


	
	SELECT * INTO #Temp
	FROM 
	(
		--####################################### DROPED OUT Employees TYPE 1 ##############################################
		SELECT SUM(CAST(SM.Amount AS INT)) AS Salary, D.DesignationName,E.CardNo,F.Floor, 1 AS Type,
		CASE 
				WHEN RIGHT(E.CardNo,5) >= '01000' AND RIGHT(E.CardNo,5) <= '01999' THEN 'Factory Management' 
				WHEN RIGHT(E.CardNo,5) >= '02000' AND RIGHT(E.CardNo,5) <= '02099' THEN 'Engineering/Data Entry'
				WHEN RIGHT(E.CardNo,5) >= '02100' AND RIGHT(E.CardNo,5) <= '02199' THEN 'Factory Management'
				WHEN RIGHT(E.CardNo,5) >= '02200' AND RIGHT(E.CardNo,5) <= '02399' THEN 'Production Staff Swing'
				WHEN RIGHT(E.CardNo,5) >= '02400' AND RIGHT(E.CardNo,5) <= '02499' THEN 'Production Staff Cutting'
				WHEN RIGHT(E.CardNo,5) >= '02500' AND RIGHT(E.CardNo,5) <= '02599' THEN 'Production Staff Finishing'
				WHEN RIGHT(E.CardNo,5) >= '02600' AND RIGHT(E.CardNo,5) <= '02699' THEN 'Store' 
				WHEN RIGHT(E.CardNo,5) >= '02700' AND RIGHT(E.CardNo,5) <= '02799' THEN 'Q.C' 
				WHEN RIGHT(E.CardNo,5) >= '02800' AND RIGHT(E.CardNo,5) <= '02899' THEN 'Production Staff Swing' 
				WHEN RIGHT(E.CardNo,5) >= '03100' AND RIGHT(E.CardNo,5) <= '03499' THEN 'Production Staff Swing'
				WHEN RIGHT(E.CardNo,5) >= '03500' AND RIGHT(E.CardNo,5) <= '03599' THEN 'Production Staff Cutting'
				WHEN RIGHT(E.CardNo,5) >= '03600' AND RIGHT(E.CardNo,5) <= '03699' THEN 'Production Staff Finishing'
				WHEN RIGHT(E.CardNo,5) >= '03800' AND RIGHT(E.CardNo,5) <= '03999' THEN 'Production Staff Cutting'
				WHEN RIGHT(E.CardNo,5) >= '04100' AND RIGHT(E.CardNo,5) <= '04999' THEN 'Sample Machinist'
				WHEN RIGHT(E.CardNo,5) >= '05199' AND RIGHT(E.CardNo,5) <= '05499' THEN 'Mechanical' 
				WHEN RIGHT(E.CardNo,5) >= '05500' AND RIGHT(E.CardNo,5) <= '05899' THEN 'Electrical'				
				WHEN RIGHT(E.CardNo,5) >= '06100' AND RIGHT(E.CardNo,5) <= '06199' THEN 'Administration (Pure)'
				WHEN RIGHT(E.CardNo,5) >= '06200' AND RIGHT(E.CardNo,5) <= '06299' THEN 'Welfare_Medical'   
				WHEN RIGHT(E.CardNo,5) >= '06300' AND RIGHT(E.CardNo,5) <= '06399' THEN 'Compliance'
				WHEN RIGHT(E.CardNo,5) >= '06400' AND RIGHT(E.CardNo,5) <= '06499' THEN 'IT' 
				WHEN RIGHT(E.CardNo,5) >= '06500' AND RIGHT(E.CardNo,5) <= '06599' THEN 'Time Keeper' 
				WHEN RIGHT(E.CardNo,5) >= '06600' AND RIGHT(E.CardNo,5) <= '06699' THEN 'Store'
				WHEN RIGHT(E.CardNo,5) >= '06700' AND RIGHT(E.CardNo,5) <= '06799' THEN 'Driver'
				WHEN RIGHT(E.CardNo,5) >= '06800' AND RIGHT(E.CardNo,5) <= '06899' THEN 'Peon'
				WHEN RIGHT(E.CardNo,5) >= '06900' AND RIGHT(E.CardNo,5) <= '06999' THEN 'Loader/Cleaner'				
				WHEN RIGHT(E.CardNo,5) >= '07000' AND RIGHT(E.CardNo,5) <= '07999' THEN 'Accounts' 
				WHEN RIGHT(E.CardNo,5) >= '08100' AND RIGHT(E.CardNo,5) <= '08699' THEN 'Security'				
				WHEN RIGHT(E.CardNo,5) >= '08700' AND RIGHT(E.CardNo,5) <= '08899' THEN 'Checker'		
				WHEN RIGHT(E.CardNo,5) >= '08900' AND RIGHT(E.CardNo,5) <= '08999' THEN 'Compliance'	
				WHEN RIGHT(E.CardNo,5) >= '09000' AND RIGHT(E.CardNo,5) <= '09099' THEN 'Factory Management'	
				WHEN RIGHT(E.CardNo,5) >= '09100' AND RIGHT(E.CardNo,5) <= '09799' THEN 'Merchandising'
				WHEN RIGHT(E.CardNo,5) >= '09800' AND RIGHT(E.CardNo,5) <= '09999' THEN 'Commercial'
				WHEN RIGHT(E.CardNo,5) >= '11000' AND RIGHT(E.CardNo,5) <= '32999' THEN 'Operator'
				WHEN RIGHT(E.CardNo,5) >= '33000' AND RIGHT(E.CardNo,5) <= '54999' THEN 'Asst Swing Machine Operator'
				WHEN RIGHT(E.CardNo,5) >= '55000' AND RIGHT(E.CardNo,5) <= '65999' THEN 'Iron Man (S)'
				WHEN RIGHT(E.CardNo,5) >= '66000' AND RIGHT(E.CardNo,5) <= '66999' THEN 'Iron Man (F)'
				WHEN RIGHT(E.CardNo,5) >= '67000' AND RIGHT(E.CardNo,5) <= '76999' THEN 'Packer, Folder And Finishing Asst.'
				WHEN RIGHT(E.CardNo,5) >= '77000' AND RIGHT(E.CardNo,5) <= '87999' THEN 'Asst.Cutter'
				WHEN RIGHT(E.CardNo,5) >= '97000' AND RIGHT(E.CardNo,5) <= '99999' THEN 'Quality(C)'
				WHEN RIGHT(E.CardNo,5) >= '88000' AND RIGHT(E.CardNo,5) <= '92999' THEN 'Quality(S)'
				WHEN RIGHT(E.CardNo,5) >= '93000' AND RIGHT(E.CardNo,5) <= '96999' THEN 'Quality(F)'

				ELSE 'Other' END AS Section,
				CASE 
				WHEN RIGHT(E.CardNo,5) >= '01000' AND RIGHT(E.CardNo,5) <= '01999' THEN '1' 
				WHEN RIGHT(E.CardNo,5) >= '02000' AND RIGHT(E.CardNo,5) <= '02099' THEN '2'
				WHEN RIGHT(E.CardNo,5) >= '02100' AND RIGHT(E.CardNo,5) <= '02199' THEN '1'
				WHEN RIGHT(E.CardNo,5) >= '02200' AND RIGHT(E.CardNo,5) <= '02399' THEN '3'
				WHEN RIGHT(E.CardNo,5) >= '02400' AND RIGHT(E.CardNo,5) <= '02499' THEN '4'
				WHEN RIGHT(E.CardNo,5) >= '02500' AND RIGHT(E.CardNo,5) <= '02599' THEN '5'
				WHEN RIGHT(E.CardNo,5) >= '02600' AND RIGHT(E.CardNo,5) <= '02699' THEN '6' 
				WHEN RIGHT(E.CardNo,5) >= '02700' AND RIGHT(E.CardNo,5) <= '02799' THEN '7' 
				WHEN RIGHT(E.CardNo,5) >= '02800' AND RIGHT(E.CardNo,5) <= '02899' THEN '3' 
				WHEN RIGHT(E.CardNo,5) >= '03100' AND RIGHT(E.CardNo,5) <= '03499' THEN '3'
				WHEN RIGHT(E.CardNo,5) >= '03500' AND RIGHT(E.CardNo,5) <= '03599' THEN '4'
				WHEN RIGHT(E.CardNo,5) >= '03600' AND RIGHT(E.CardNo,5) <= '03699' THEN '5'
				WHEN RIGHT(E.CardNo,5) >= '03800' AND RIGHT(E.CardNo,5) <= '03999' THEN '4'
				WHEN RIGHT(E.CardNo,5) >= '04100' AND RIGHT(E.CardNo,5) <= '04999' THEN '8'
				WHEN RIGHT(E.CardNo,5) >= '05199' AND RIGHT(E.CardNo,5) <= '05499' THEN '9' 
				WHEN RIGHT(E.CardNo,5) >= '05500' AND RIGHT(E.CardNo,5) <= '05899' THEN '10'
				WHEN RIGHT(E.CardNo,5) >= '06100' AND RIGHT(E.CardNo,5) <= '06199' THEN '11'
				WHEN RIGHT(E.CardNo,5) >= '06200' AND RIGHT(E.CardNo,5) <= '06299' THEN '12'
				WHEN RIGHT(E.CardNo,5) >= '06300' AND RIGHT(E.CardNo,5) <= '06399' THEN '13'
				WHEN RIGHT(E.CardNo,5) >= '06400' AND RIGHT(E.CardNo,5) <= '06499' THEN '14'
				WHEN RIGHT(E.CardNo,5) >= '06500' AND RIGHT(E.CardNo,5) <= '06599' THEN '15'
				WHEN RIGHT(E.CardNo,5) >= '06600' AND RIGHT(E.CardNo,5) <= '06699' THEN '16'
				WHEN RIGHT(E.CardNo,5) >= '06700' AND RIGHT(E.CardNo,5) <= '06799' THEN '17'
				WHEN RIGHT(E.CardNo,5) >= '06800' AND RIGHT(E.CardNo,5) <= '06899' THEN '18'
				WHEN RIGHT(E.CardNo,5) >= '06900' AND RIGHT(E.CardNo,5) <= '06999' THEN '19'
				WHEN RIGHT(E.CardNo,5) >= '07000' AND RIGHT(E.CardNo,5) <= '07999' THEN '20'
				WHEN RIGHT(E.CardNo,5) >= '08100' AND RIGHT(E.CardNo,5) <= '08699' THEN '21'
				WHEN RIGHT(E.CardNo,5) >= '08700' AND RIGHT(E.CardNo,5) <= '08899' THEN '22'
				WHEN RIGHT(E.CardNo,5) >= '08900' AND RIGHT(E.CardNo,5) <= '08999' THEN '23'
				WHEN RIGHT(E.CardNo,5) >= '09000' AND RIGHT(E.CardNo,5) <= '09099' THEN '1'
				WHEN RIGHT(E.CardNo,5) >= '09100' AND RIGHT(E.CardNo,5) <= '09799' THEN '24'
				WHEN RIGHT(E.CardNo,5) >= '09800' AND RIGHT(E.CardNo,5) <= '09999' THEN '25'
				WHEN RIGHT(E.CardNo,5) >= '11000' AND RIGHT(E.CardNo,5) <= '32999' THEN '26'
				WHEN RIGHT(E.CardNo,5) >= '33000' AND RIGHT(E.CardNo,5) <= '54999' THEN '27'
				WHEN RIGHT(E.CardNo,5) >= '55000' AND RIGHT(E.CardNo,5) <= '65999' THEN '28'
				WHEN RIGHT(E.CardNo,5) >= '66000' AND RIGHT(E.CardNo,5) <= '66999' THEN '29'
				WHEN RIGHT(E.CardNo,5) >= '67000' AND RIGHT(E.CardNo,5) <= '76999' THEN '30'
				WHEN RIGHT(E.CardNo,5) >= '77000' AND RIGHT(E.CardNo,5) <= '87999' THEN '31'
				WHEN RIGHT(E.CardNo,5) >= '97000' AND RIGHT(E.CardNo,5) <= '99999' THEN '32'
				WHEN RIGHT(E.CardNo,5) >= '88000' AND RIGHT(E.CardNo,5) <= '92999' THEN '33'
				WHEN RIGHT(E.CardNo,5) >= '93000' AND RIGHT(E.CardNo,5) <= '96999' THEN '34'

				ELSE '35' END AS Code
		FROM Employees E
		INNER JOIN InactiveEmployees I ON E.EmployeeID=I.EmployeeID
		LEFT JOIN Designations D ON E.DesignationID = D.DesignationID
		INNER JOIN ProductionFloorLines F ON E.ProductionFloorLineID = F.ProductionFloorLineID
		LEFT JOIN SalaryMappings SM ON E.EmployeeID=SM.EmployeeID	
		WHERE CAST(I.ApplicableDate AS DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)	
		GROUP BY D.DesignationName,F.Floor,E.CardNo

		UNION ALL

		--####################################### NEW JOIN Employees TYPE 2 ##############################################
		SELECT SUM(CAST(SM.Amount AS INT)) AS Salary,Desg.DesignationName,E.CardNo,P.Floor,2 AS Type,
			CASE 
				WHEN RIGHT(E.CardNo,5) >= '01000' AND RIGHT(E.CardNo,5) <= '01999' THEN 'Factory Management' 
				WHEN RIGHT(E.CardNo,5) >= '02000' AND RIGHT(E.CardNo,5) <= '02099' THEN 'Engineering/Data Entry'
				WHEN RIGHT(E.CardNo,5) >= '02100' AND RIGHT(E.CardNo,5) <= '02199' THEN 'Factory Management'
				WHEN RIGHT(E.CardNo,5) >= '02200' AND RIGHT(E.CardNo,5) <= '02399' THEN 'Production Staff Swing'
				WHEN RIGHT(E.CardNo,5) >= '02400' AND RIGHT(E.CardNo,5) <= '02499' THEN 'Production Staff Cutting'
				WHEN RIGHT(E.CardNo,5) >= '02500' AND RIGHT(E.CardNo,5) <= '02599' THEN 'Production Staff Finishing'
				WHEN RIGHT(E.CardNo,5) >= '02600' AND RIGHT(E.CardNo,5) <= '02699' THEN 'Store' 
				WHEN RIGHT(E.CardNo,5) >= '02700' AND RIGHT(E.CardNo,5) <= '02799' THEN 'Q.C' 
				WHEN RIGHT(E.CardNo,5) >= '02800' AND RIGHT(E.CardNo,5) <= '02899' THEN 'Production Staff Swing' 
				WHEN RIGHT(E.CardNo,5) >= '03100' AND RIGHT(E.CardNo,5) <= '03499' THEN 'Production Staff Swing'
				WHEN RIGHT(E.CardNo,5) >= '03500' AND RIGHT(E.CardNo,5) <= '03599' THEN 'Production Staff Cutting'
				WHEN RIGHT(E.CardNo,5) >= '03600' AND RIGHT(E.CardNo,5) <= '03699' THEN 'Production Staff Finishing'
				WHEN RIGHT(E.CardNo,5) >= '03800' AND RIGHT(E.CardNo,5) <= '03999' THEN 'Production Staff Cutting'
				WHEN RIGHT(E.CardNo,5) >= '04100' AND RIGHT(E.CardNo,5) <= '04999' THEN 'Sample Machinist'
				WHEN RIGHT(E.CardNo,5) >= '05199' AND RIGHT(E.CardNo,5) <= '05499' THEN 'Mechanical' 
				WHEN RIGHT(E.CardNo,5) >= '05500' AND RIGHT(E.CardNo,5) <= '05899' THEN 'Electrical'				
				WHEN RIGHT(E.CardNo,5) >= '06100' AND RIGHT(E.CardNo,5) <= '06199' THEN 'Administration (Pure)'
				WHEN RIGHT(E.CardNo,5) >= '06200' AND RIGHT(E.CardNo,5) <= '06299' THEN 'Welfare_Medical'   
				WHEN RIGHT(E.CardNo,5) >= '06300' AND RIGHT(E.CardNo,5) <= '06399' THEN 'Compliance'
				WHEN RIGHT(E.CardNo,5) >= '06400' AND RIGHT(E.CardNo,5) <= '06499' THEN 'IT' 
				WHEN RIGHT(E.CardNo,5) >= '06500' AND RIGHT(E.CardNo,5) <= '06599' THEN 'Time Keeper' 
				WHEN RIGHT(E.CardNo,5) >= '06600' AND RIGHT(E.CardNo,5) <= '06699' THEN 'Store'
				WHEN RIGHT(E.CardNo,5) >= '06700' AND RIGHT(E.CardNo,5) <= '06799' THEN 'Driver'
				WHEN RIGHT(E.CardNo,5) >= '06800' AND RIGHT(E.CardNo,5) <= '06899' THEN 'Peon'
				WHEN RIGHT(E.CardNo,5) >= '06900' AND RIGHT(E.CardNo,5) <= '06999' THEN 'Loader/Cleaner'				
				WHEN RIGHT(E.CardNo,5) >= '07000' AND RIGHT(E.CardNo,5) <= '07999' THEN 'Accounts' 
				WHEN RIGHT(E.CardNo,5) >= '08100' AND RIGHT(E.CardNo,5) <= '08699' THEN 'Security'				
				WHEN RIGHT(E.CardNo,5) >= '08700' AND RIGHT(E.CardNo,5) <= '08899' THEN 'Checker'		
				WHEN RIGHT(E.CardNo,5) >= '08900' AND RIGHT(E.CardNo,5) <= '08999' THEN 'Compliance'	
				WHEN RIGHT(E.CardNo,5) >= '09000' AND RIGHT(E.CardNo,5) <= '09099' THEN 'Factory Management'	
				WHEN RIGHT(E.CardNo,5) >= '09100' AND RIGHT(E.CardNo,5) <= '09799' THEN 'Merchandising'
				WHEN RIGHT(E.CardNo,5) >= '09800' AND RIGHT(E.CardNo,5) <= '09999' THEN 'Commercial'
				WHEN RIGHT(E.CardNo,5) >= '11000' AND RIGHT(E.CardNo,5) <= '32999' THEN 'Operator'
				WHEN RIGHT(E.CardNo,5) >= '33000' AND RIGHT(E.CardNo,5) <= '54999' THEN 'Asst Swing Machine Operator'
				WHEN RIGHT(E.CardNo,5) >= '55000' AND RIGHT(E.CardNo,5) <= '65999' THEN 'Iron Man (S)'
				WHEN RIGHT(E.CardNo,5) >= '66000' AND RIGHT(E.CardNo,5) <= '66999' THEN 'Iron Man (F)'
				WHEN RIGHT(E.CardNo,5) >= '67000' AND RIGHT(E.CardNo,5) <= '76999' THEN 'Packer, Folder And Finishing Asst.'
				WHEN RIGHT(E.CardNo,5) >= '77000' AND RIGHT(E.CardNo,5) <= '87999' THEN 'Asst.Cutter'
				WHEN RIGHT(E.CardNo,5) >= '97000' AND RIGHT(E.CardNo,5) <= '99999' THEN 'Quality(C)'
				WHEN RIGHT(E.CardNo,5) >= '88000' AND RIGHT(E.CardNo,5) <= '92999' THEN 'Quality(S)'
				WHEN RIGHT(E.CardNo,5) >= '93000' AND RIGHT(E.CardNo,5) <= '96999' THEN 'Quality(F)'

				ELSE 'Other' END AS Section,
				CASE 
				WHEN RIGHT(E.CardNo,5) >= '01000' AND RIGHT(E.CardNo,5) <= '01999' THEN '1' 
				WHEN RIGHT(E.CardNo,5) >= '02000' AND RIGHT(E.CardNo,5) <= '02099' THEN '2'
				WHEN RIGHT(E.CardNo,5) >= '02100' AND RIGHT(E.CardNo,5) <= '02199' THEN '1'
				WHEN RIGHT(E.CardNo,5) >= '02200' AND RIGHT(E.CardNo,5) <= '02399' THEN '3'
				WHEN RIGHT(E.CardNo,5) >= '02400' AND RIGHT(E.CardNo,5) <= '02499' THEN '4'
				WHEN RIGHT(E.CardNo,5) >= '02500' AND RIGHT(E.CardNo,5) <= '02599' THEN '5'
				WHEN RIGHT(E.CardNo,5) >= '02600' AND RIGHT(E.CardNo,5) <= '02699' THEN '6' 
				WHEN RIGHT(E.CardNo,5) >= '02700' AND RIGHT(E.CardNo,5) <= '02799' THEN '7' 
				WHEN RIGHT(E.CardNo,5) >= '02800' AND RIGHT(E.CardNo,5) <= '02899' THEN '3' 
				WHEN RIGHT(E.CardNo,5) >= '03100' AND RIGHT(E.CardNo,5) <= '03499' THEN '3'
				WHEN RIGHT(E.CardNo,5) >= '03500' AND RIGHT(E.CardNo,5) <= '03599' THEN '4'
				WHEN RIGHT(E.CardNo,5) >= '03600' AND RIGHT(E.CardNo,5) <= '03699' THEN '5'
				WHEN RIGHT(E.CardNo,5) >= '03800' AND RIGHT(E.CardNo,5) <= '03999' THEN '4'
				WHEN RIGHT(E.CardNo,5) >= '04100' AND RIGHT(E.CardNo,5) <= '04999' THEN '8'
				WHEN RIGHT(E.CardNo,5) >= '05199' AND RIGHT(E.CardNo,5) <= '05499' THEN '9' 
				WHEN RIGHT(E.CardNo,5) >= '05500' AND RIGHT(E.CardNo,5) <= '05899' THEN '10'
				WHEN RIGHT(E.CardNo,5) >= '06100' AND RIGHT(E.CardNo,5) <= '06199' THEN '11'
				WHEN RIGHT(E.CardNo,5) >= '06200' AND RIGHT(E.CardNo,5) <= '06299' THEN '12'
				WHEN RIGHT(E.CardNo,5) >= '06300' AND RIGHT(E.CardNo,5) <= '06399' THEN '13'
				WHEN RIGHT(E.CardNo,5) >= '06400' AND RIGHT(E.CardNo,5) <= '06499' THEN '14'
				WHEN RIGHT(E.CardNo,5) >= '06500' AND RIGHT(E.CardNo,5) <= '06599' THEN '15'
				WHEN RIGHT(E.CardNo,5) >= '06600' AND RIGHT(E.CardNo,5) <= '06699' THEN '16'
				WHEN RIGHT(E.CardNo,5) >= '06700' AND RIGHT(E.CardNo,5) <= '06799' THEN '17'
				WHEN RIGHT(E.CardNo,5) >= '06800' AND RIGHT(E.CardNo,5) <= '06899' THEN '18'
				WHEN RIGHT(E.CardNo,5) >= '06900' AND RIGHT(E.CardNo,5) <= '06999' THEN '19'
				WHEN RIGHT(E.CardNo,5) >= '07000' AND RIGHT(E.CardNo,5) <= '07999' THEN '20'
				WHEN RIGHT(E.CardNo,5) >= '08100' AND RIGHT(E.CardNo,5) <= '08699' THEN '21'
				WHEN RIGHT(E.CardNo,5) >= '08700' AND RIGHT(E.CardNo,5) <= '08899' THEN '22'
				WHEN RIGHT(E.CardNo,5) >= '08900' AND RIGHT(E.CardNo,5) <= '08999' THEN '23'
				WHEN RIGHT(E.CardNo,5) >= '09000' AND RIGHT(E.CardNo,5) <= '09099' THEN '1'
				WHEN RIGHT(E.CardNo,5) >= '09100' AND RIGHT(E.CardNo,5) <= '09799' THEN '24'
				WHEN RIGHT(E.CardNo,5) >= '09800' AND RIGHT(E.CardNo,5) <= '09999' THEN '25'
				WHEN RIGHT(E.CardNo,5) >= '11000' AND RIGHT(E.CardNo,5) <= '32999' THEN '26'
				WHEN RIGHT(E.CardNo,5) >= '33000' AND RIGHT(E.CardNo,5) <= '54999' THEN '27'
				WHEN RIGHT(E.CardNo,5) >= '55000' AND RIGHT(E.CardNo,5) <= '65999' THEN '28'
				WHEN RIGHT(E.CardNo,5) >= '66000' AND RIGHT(E.CardNo,5) <= '66999' THEN '29'
				WHEN RIGHT(E.CardNo,5) >= '67000' AND RIGHT(E.CardNo,5) <= '76999' THEN '30'
				WHEN RIGHT(E.CardNo,5) >= '77000' AND RIGHT(E.CardNo,5) <= '87999' THEN '31'
				WHEN RIGHT(E.CardNo,5) >= '97000' AND RIGHT(E.CardNo,5) <= '99999' THEN '32'
				WHEN RIGHT(E.CardNo,5) >= '88000' AND RIGHT(E.CardNo,5) <= '92999' THEN '33'
				WHEN RIGHT(E.CardNo,5) >= '93000' AND RIGHT(E.CardNo,5) <= '96999' THEN '34'

				ELSE '35' END AS Code
		FROM Employees E
		INNER JOIN Designations Desg ON E.DesignationID= Desg.DesignationID
		INNER JOIN ProductionFloorLines P ON E.ProductionFloorLineID=P.ProductionFloorLineID 
		LEFT JOIN SalaryMappings SM ON E.EmployeeID=SM.EmployeeID
		WHERE 
		--E.IsActive=1
		E.CardNo NOT LIKE '14%' 
		AND CAST(E.JoinDate as DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)
		GROUP BY Desg.DesignationName,P.Floor,E.CardNo
		)  AS T

		--DROP TABLE #Temp
		--SELECT * FROM #Temp
		SELECT SUM(Salary) AS Salary,Count(*) AS Total,DesignationName,Floor,Type,CAST(Code AS INT) AS Code,Section
		FROM #Temp
		--WHERE Floor='3rd Way -1'
		GROUP BY Floor,Code,Section,Type,DesignationName
		ORDER BY Floor,Code

END

--EXEC [dbo].[GetDropOutList] '2017-08-16'
--EXEC GetDropedOutEmployeeList '11/2/2017','','1'
--EXEC GetDropedNewJoinEmployeeInfoList '01/01/2018','01/25/2018'

--SELECT SUM(Amount) FROM SalaryMappings where EmployeeID=42099

--SELECT * FROM Employees where EmployeeName='MD SHAMIM PERVEZ'

