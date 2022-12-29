SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_Company_SecurityConfig_Get]
	@CompanyId bigint
AS
BEGIN
	SET NOCOUNT ON;
   
	SELECT
		[CompanyId],
		[IPAccess],
		[PasswordComplexity],
		CAST(CASE WHEN [PasswordCaducity] IS NULL THEN 0 ELSE [PasswordCaducity] END AS bit),
		ISNULL([PasswordCaducityDays], 0),
		CAST(CASE WHEN [PasswordRepeat] IS NULL THEN 0 ELSE  [PasswordRepeat] END AS bit),
		[Traceability],
		[GrantPermission],
		[FailedAttempts],
		[MinimumPasswordLength],
		[GroupUserMain]
	
	FROM Core_Company_SecurityConfig WITH(NOLOCK)
	WHERE
		[CompanyId] = @CompanyId
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_Company_SecurityConfig_Save]
	@CompanyId bigint,
    @IPAccess bit,
    @PasswordComplexity int,
	@PasswordCaducity bit,
	@PasswordCaducityDays int,
	@PasswordRepeat bit,
    @Traceability int,
    @GrantPermission int,
    @FailedAttempts int,
    @MinimumPasswordLength int,
    @GroupUserMain bit,
	@ApplicationUserId bigint
AS
BEGIN
	SET NOCOUNT ON;
   
	INSERT INTO [dbo].[Core_Company_SecurityConfig]
	(
		[CompanyId],
		[IPAccess],
		[PasswordComplexity],
		[PasswordCaducity],
		[PasswordCaducityDays],
		[PasswordRepeat],
		[Traceability],
		[GrantPermission],
		[FailedAttempts],
		[MinimumPasswordLength],
		[GroupUserMain]
	)
	VALUES
	(
		@CompanyId,
		@IPAccess,
		@PasswordComplexity,
		@PasswordCaducity,
		@PasswordCaducityDays,
		@PasswordRepeat,
		@Traceability,
		@GrantPermission,
		@FailedAttempts,
		@MinimumPasswordLength,
		@GroupUserMain
	)

	
   
	INSERT INTO [dbo].[Core_Company_SecurityConfigTrace]
	(
		[CompanyId],
		[IPAccess],
		[PasswordComplexity],
		[Traceability],
		[GrantPermission],
		[FailedAttempts],
		[MinimumPasswordLength],
		[GroupUserMain],
		[ModifiedBy],
		[ModifiedOn]
	)
	VALUES
	(
		@CompanyId,
		@IPAccess,
		@PasswordComplexity,
		@Traceability,
		@GrantPermission,
		@FailedAttempts,
		@MinimumPasswordLength,
		@GroupUserMain,
		@ApplicationUserId,
		GETDATE()
	)
END
GO

