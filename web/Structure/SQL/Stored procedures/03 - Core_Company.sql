SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_Company_Activate]
	@Id bigint,
	@Name nvarchar(50),
	@Code nvarchar(15),
	@LOPD bit,
	@SubscriptionStart datetime,
	@SubscriptionEnd datetime,
	@ContactPerson bigint,
	@ApplicationUserId bigint
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Core_Company] SET
		[Active] = 1,
        [ModifiedBy] = @ApplicationUserId,
        [ModifiedOn] = GETDATE()
	WHERE
		[Id] = @Id

END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Juan Castilla - jcastilla@openframework.es
-- Create date: 2019-07-06
-- Description:	Gets companies accesibles by user
-- =============================================
CREATE PROCEDURE [dbo].[Core_Company_All]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		C.Id,
		C.[Name],
		C.Code,
		C.LOPD,
		C.ContactPerson,
		ISNULL(CP.[Name], '') AS ContactPersonName,
		ISNULL(CP.LastName,'') AS ContactPersonLastName,
		ISNULL(CP.LastName2,'') AS ContactPersonLastName,
		ISNULL(U.Email,'') AS ConstactPersonEmail,
		C.SubscriptionStart,
		C.SubscriptionEnd,
		C.CreatedBy,
		ISNULL(CB.[Name],'') AS CreatedByName,
		ISNULL(CB.LastName,'') AS CreatedByLastName,
		ISNULL(CB.LastName2,'') AS CreatedByLastName2,
		C.CreatedOn,
		C.ModifiedBy,
		ISNULL(MB.[Name],'') AS ModifiedByName,
		ISNULL(MB.LastName,'') AS ModifiedByLastName,
		ISNULL(MB.LastName2,'') AS ModifiedByLastName2,
		C.ModifiedOn,
		C.Active,
		C.DefaultLanguage

	FROM Core_Company C WITH(NOLOCK)
	INNER JOIN Core_User U WITH(NOLOCK)
	ON	U.Id = C.ContactPerson
	INNER JOIN Core_Profile CP WITH(NOLOCK)
	ON	CP.ApplicationUserId = C.ContactPerson
	INNER JOIN Core_Profile CB WITH(NOLOCK)
	ON	CB.ApplicationUserId = C.CreatedBy
	INNER JOIN Core_Profile MB WITH(NOLOCK)
	ON	MB.ApplicationUserId = C.ModifiedBy

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
CREATE PROCEDURE [dbo].[Core_Company_ById]
	@CompanyId bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		C.Id,
		C.[Name],
		C.Code,
		C.LOPD,
		C.ContactPerson,
		ISNULL(CP.[Name], '') AS ContactPersonName,
		ISNULL(CP.LastName,'') AS ContactPersonLastName,
		ISNULL(CP.LastName2,'') AS ContactPersonLastName,
		ISNULL(U.Email,'') AS ConstactPersonEmail,
		C.SubscriptionStart,
		C.SubscriptionEnd,
		C.CreatedBy,
		ISNULL(CB.[Name],'') AS CreatedByName,
		ISNULL(CB.LastName,'') AS CreatedByLastName,
		ISNULL(CB.LastName2,'') AS CreatedByLastName2,
		C.CreatedOn,
		C.ModifiedBy,
		ISNULL(MB.[Name],'') AS ModifiedByName,
		ISNULL(MB.LastName,'') AS ModifiedByLastName,
		ISNULL(MB.LastName2,'') AS ModifiedByLastName2,
		C.ModifiedOn,
		C.Active,
		C.DefaultLanguage,
		CA.Id,
		CA.LocationWayTypeId,
		ISNULL(CA.LocationAddress,''),
		ISNULL(CA.LocationAddressComplement,''),
		CA.LocationPostalCodeId,
		ISNULL(C.RazonSocial,''),
		ISNULL(C.CIF,'') AS CIF,
		ISNULL(C.Email,'') AS Email,
		ISNULL(C.Web,'') AS Web,
		ISNULL(C.Phone,'') AS Phone,
		ISNULL(C.Fax,'') AS Fax,
		ISNULL(C.Features,'') AS Features

	FROM Core_Company C WITH(NOLOCK)
	INNER JOIN Core_User U WITH(NOLOCK)
	ON	U.Id = C.ContactPerson
	INNER JOIN Core_Profile CP WITH(NOLOCK)
	ON	CP.ApplicationUserId = C.ContactPerson
	INNER JOIN Core_Profile CB WITH(NOLOCK)
	ON	CB.ApplicationUserId = C.CreatedBy
	INNER JOIN Core_Profile MB WITH(NOLOCK)
	ON	MB.ApplicationUserId = C.ModifiedBy
	LEFT JOIN Core_Language L WITH(NOLOCK)
	ON	L.Id = C.DefaultLanguage
	LEFT JOIN Core_CompanyAddress CA WITH(NOLOCK)
	ON	CA.CompanyId = C.Id
	AND CA.Main = 1

	WHERE
		C.Id = @CompanyId

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
CREATE PROCEDURE [dbo].[Core_Company_Inactivate]
	@Id bigint,
	@Name nvarchar(50),
	@Code nvarchar(15),
	@LOPD bit,
	@SubscriptionStart datetime,
	@SubscriptionEnd datetime,
	@ContactPerson bigint,
	@ApplicationUserId bigint
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE [dbo].[Core_Company] SET
		[Active] = 0,
        [ModifiedBy] = @ApplicationUserId,
        [ModifiedOn] = GETDATE()
	WHERE
		[Id] = @Id

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
CREATE PROCEDURE [dbo].[Core_Company_Insert] 
	@Id bigint output,
	@Name nvarchar(50),
	@Code nvarchar(15),
	@LOPD bit,
	@SubscriptionStart datetime,
	@SubscriptionEnd datetime,
	@ContactPerson bigint,
	@ApplicationUserId bigint
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO [dbo].[Core_Company]
           ([Name]
           ,[Code]
           ,[LOPD]
           ,[SubscriptionStart]
           ,[SubscriptionEnd]
           ,[ContactPerson]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
           (@Name
           ,@Code
           ,@LOPD
           ,@SubscriptionStart
           ,@SubscriptionEnd
           ,@ContactPerson
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

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
CREATE PROCEDURE [dbo].[Core_Company_Update]
	@Id bigint,
	@Name nvarchar(50),
	@Code nvarchar(15),
	@LOPD bit,
	@SubscriptionStart datetime,
	@SubscriptionEnd datetime,
	@ContactPerson bigint,
	@ApplicationUserId bigint
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE [dbo].[Core_Company] SET
		[Name] = @Name,
        [Code] = @Code,
        [LOPD] = @LOPD,
        [SubscriptionStart] = @SubscriptionStart,
        [SubscriptionEnd] = @SubscriptionEnd,
        [ContactPerson] = @ContactPerson,
        [ModifiedBy] = @ApplicationUserId,
        [ModifiedOn] = GETDATE()
	WHERE
		[Id] = @Id

END


GO


