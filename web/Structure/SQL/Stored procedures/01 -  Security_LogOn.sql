SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Security_LogOn]
	@UserName nvarchar(50),
	@Password nvarchar(50),
	@Result int output,
	@Locked bit output,
	@Corporative bit output,
	@Multicompany bit output,
	@CompanyId bigint output
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		@Result = U.Id,
		@Locked = U.Locked,
		@Corporative = U.Corporative,
		@Multicompany = CASE WHEN U.Core = 1 THEN 1 ELSE ISNULL(COUNT(CCMS.CompanyId),0) END,
		@CompanyId = ISNULL(MIN(CCMS.CompanyId),0)
	FROM Core_User U WITH(NOLOCK)
	LEFT JOIN Core_CompanyMemberShip CCMS WITH(NOLOCK)
	ON	CCMS.Active = 1
	AND	CCMS.UserId = U.Id

	WHERE
		U.Active = 1
	AND U.Email = @UserName
	AND U.Pass = @Password

	GROUP BY U.Id, U.Locked, U.Corporative, U.Core

	IF @@ROWCOUNT = 0
		BEGIN
			SET @Result = -1
			SET @Corporative = 0
			SET @Locked = 0
			SET @Multicompany = 0
			SET @CompanyId = 0
		END

END
