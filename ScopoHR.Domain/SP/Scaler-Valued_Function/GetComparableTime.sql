USE [ScopoHR]
GO
/****** Object:  UserDefinedFunction [dbo].[GetComparableTime]    Script Date: 05/20/2018 2:51:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[GetComparableTime](@date DATETIME)
RETURNS VARCHAR(MAX)
AS 
BEGIN
	DECLARE @res AS VARCHAR(MAX) = (
		SELECT     
		RIGHT('0' + CONVERT([varchar](2), DATEPART(HOUR, @date)), 2) + ':' +
		RIGHT('0' + CONVERT([varchar](2), DATEPART(MINUTE, @date)), 2)
		)
	RETURN @res;
END;
