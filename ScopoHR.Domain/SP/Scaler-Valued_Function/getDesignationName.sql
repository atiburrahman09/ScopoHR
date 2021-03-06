USE [ScopoHR]
GO
/****** Object:  UserDefinedFunction [dbo].[getDesignationName]    Script Date: 05/20/2018 2:51:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[getDesignationName](@designationID INT)
RETURNS VARCHAR(MAX)
AS
BEGIN	
	DECLARE @status VARCHAR(MAX);
	
	SET @status = (SELECT DesignationName FROM Designations WHERE DesignationID=@designationID) 
	
RETURN @status;
END

