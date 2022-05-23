--IF EXISTS(SELECT * FROM Sys.objects WHERE name = 'GetBankSalaryPad')
--BEGIN
--	DROP PROCEDURE GetBankSalaryPad
--END

--GO


CREATE PROCEDURE [dbo].[GetBankSalaryPad]
	@month INT,
	@bankName nvarchar(max),
	@company nvarchar (max),
	@year INT
AS
BEGIN
	SET NOCOUNT ON;

	IF(@company = 'BDC')
		BEGIN
			SELECT E.EmployeeName, E.CardNo,D.DesignationName, CAST(CAST(E.JoinDate AS DATE) AS VARCHAR(200)) AS JoiningDate,
		M.PayableWages AS Gross,
		ISNULL(M.Advance, 0) AS Deduction,
		BS.AccountNo,
		ROUND(M.TotalPay - ISNULL(M.Advance, 0), 0) AS TotalPay,
		BS.BankName,Bs.Company,M.Month,M.Year
		
		From MonthlySalaries M
		INNER JOIN Employees E ON M.EmployeeId = E.EmployeeID
		LEFT JOIN Designations D ON E.DesignationID = D.DesignationID
		LEFT JOIN ProductionFloorLines P on E.ProductionFloorLineID = P.ProductionFloorLineID
		LEFT JOIN InactiveEmployees IE ON E.EmployeeID = IE.EmployeeID
		INNER JOIN BankSalary BS ON E.CardNo = BS.CardNo
		WHERE M.Month = @month
		AND M.Year=@year
		AND BS.BankName=@bankName
		AND p.ProductionFloorLineID=106
		--AND BS.Company=@company
		--AND E.DepartmentID <> 6
		AND IE.EmployeeID IS NULL
		ORDER BY E.CardNo 
		END
	ELSE
		BEGIN
			SELECT E.EmployeeName, E.CardNo,D.DesignationName, CAST(CAST(E.JoinDate AS DATE) AS VARCHAR(200)) AS JoiningDate,
		M.PayableWages AS Gross,
		ISNULL(M.Advance, 0) AS Deduction,
		BS.AccountNo,
		ROUND(M.TotalPay - ISNULL(M.Advance, 0), 0) AS TotalPay,
		BS.BankName,Bs.Company,M.Month,M.Year
		
		From MonthlySalaries M
		INNER JOIN Employees E ON M.EmployeeId = E.EmployeeID
		LEFT JOIN Designations D ON E.DesignationID = D.DesignationID
		LEFT JOIN ProductionFloorLines P on E.ProductionFloorLineID = P.ProductionFloorLineID
		LEFT JOIN InactiveEmployees IE ON E.EmployeeID = IE.EmployeeID
		INNER JOIN BankSalary BS ON E.CardNo = BS.CardNo
		WHERE M.Month = @month
		AND M.Year=@year
		AND BS.BankName=@bankName
		AND BS.Company=@company
		--AND E.DepartmentID <> 6
		AND IE.EmployeeID IS NULL
		ORDER BY E.CardNo 
		END

	
END

--EXEC GetBankSalaryPad 12