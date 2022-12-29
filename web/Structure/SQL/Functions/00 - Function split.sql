USE [openf_support]
GO

/****** Object:  UserDefinedFunction [dbo].[fn_split]    Script Date: 27/12/2022 13:51:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE function [dbo].[fn_split](
	@str varchar(8000),
	@spliter char(1)
)
RETURNS @returnTable TABLE (item varchar(8000))
AS
BEGIN
	DECLARE @spliterIndex int
	SELECT @str = @str + @spliter

	WHILE len(@str) > 0
	BEGIN
		SELECT @spliterIndex = charindex(@spliter,@str)
		IF @spliterIndex = 1
			INSERT @returnTable (item) VALUES (null)
		ELSE
			INSERT @returnTable (item) VALUES (substring(@str, 1, @spliterIndex-1))

		SELECT @str = substring(@str, @spliterIndex+1, len(@str)-@spliterIndex)
	END
	RETURN
END
GO

