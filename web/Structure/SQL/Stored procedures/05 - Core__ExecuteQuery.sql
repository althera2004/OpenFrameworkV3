SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[Core_ExecuteQuery]
	 @statement nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	EXECUTE sp_executesql @statement
END
GO

