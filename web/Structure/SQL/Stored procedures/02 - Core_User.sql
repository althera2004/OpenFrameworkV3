SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_User_GetAll]
	@CompanyId bigint
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		U.Id,
		U.Email,
		U.Locked,
		U.Core,
		U.Language,
		L.Name,
		L.LocaleName,
		L.ISO,
		U.TechnicalUser,
		U.PrimaryUser,
		U.AdminUser,
		ISNULL(CP.Name,''),
		ISNULL(CP.LastName,''),
		ISNULL(CP.LastName2,''),
		ISNULL(CP.Phone,''),
		ISNULL(CP.Mobile,''),
		ISNULL(CP.PhoneEmergency,''),
		ISNULL(CP.Fax,''),
		ISNULL(CP.EmailAlternative,''),
		ISNULL(CP.Twitter,''),
		ISNULL(CP.LinkedIn,''),
		ISNULL(CP.Instagram,''),
		ISNULL(CP.Facebook,''),
		ISNULL(CP.Gender,0),
		CP.IdentificationCard,
		CP.IMEI,
		CP.BirthDate,
		CP.AddressWayType,
		CP.Address,
		CP.PostalCode,
		CP.City,
		CP.Province,
		CP.State,
		CP.Country,
		CP.Latitude,
		CP.Longitude,
		U.CreatedBy,
		ISNULL(CB.Name,''),
		ISNULL(CB.LastName,''),
		ISNULL(CB.LastName2,''),
		U.CreatedOn,
		U.ModifiedBy,
		ISNULL(MB.Name,''),
		ISNULL(MB.LastName,''),
		ISNULL(MB.LastName2,''),
		U.ModifiedOn,
		U.Active,
		U.[External],
		ISNULL(U.PinCode,''),
		ISNULL(CMS.CompanyId,@CompanyId),
		U.LastConnection,
		CP.Web,
		U.Corporative,
		ISNULL(CP.DataText1,''),
		ISNULL(CP.DataText2,''),
		ISNULL(CP.DataText3,''),
		ISNULL(CP.DataText4,'')
	FROM Core_User U WITH(NOLOCK)
	INNER JOIN Core_Profile CP WITH(NOLOCK)
	ON	CP.ApplicationUserId = U.Id
	INNER JOIN Core_Profile CB WITH(NOLOCK)
	ON	CB.ApplicationUserId = U.CreatedBy
	INNER JOIN Core_Profile MB WITH(NOLOCK)
	ON	MB.ApplicationUserId = U.ModifiedBy
	INNER JOIN Core_Language L WITH(NOLOCK)
	ON	L.Id = U.Language
	LEFT JOIN Core_CompanyMemberShip CMS WITH(NOLOCK)
	ON	CMS.UserId = U.Id
	AND	CMS.CompanyId = @CompanyId

	WHERE
		CMS.UserId IS NOT NULL
	OR	U.Core = 1

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
CREATE PROCEDURE [dbo].[Core_User_GetById]
	@ApplicationUserId bigint
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		U.Id,
		U.Email,
		U.Locked,
		U.Core,
		U.Language,
		L.Name,
		L.LocaleName,
		L.ISO,
		U.TechnicalUser,
		U.PrimaryUser,
		U.AdminUser,
		ISNULL(CP.Name,''),
		ISNULL(CP.LastName,''),
		ISNULL(CP.LastName2,''),
		ISNULL(CP.Phone,''),
		ISNULL(CP.Mobile,''),
		ISNULL(CP.PhoneEmergency,''),
		ISNULL(CP.Fax,''),
		ISNULL(CP.EmailAlternative,''),
		ISNULL(CP.Twitter,''),
		ISNULL(CP.LinkedIn,''),
		ISNULL(CP.Instagram,''),
		ISNULL(CP.Facebook,''),
		ISNULL(CP.Gender,0),
		CP.IdentificationCard,
		CP.IMEI,
		CP.BirthDate,
		CP.AddressWayType,
		CP.Address,
		CP.PostalCode,
		CP.City,
		CP.Province,
		CP.State,
		CP.Country,
		CP.Latitude,
		CP.Longitude,
		U.CreatedBy,
		ISNULL(CB.Name,''),
		ISNULL(CB.LastName,''),
		ISNULL(CB.LastName2,''),
		U.CreatedOn,
		U.ModifiedBy,
		ISNULL(MB.Name,''),
		ISNULL(MB.LastName,''),
		ISNULL(MB.LastName2,''),
		U.ModifiedOn,
		U.Active,
		U.[External],
		ISNULL(U.PinCode,''),
		ISNULL(CMS.CompanyId,-1),
		U.LastConnection,
		CP.Web,
		U.Corporative,
		ISNULL(CP.DataText1,''),
		ISNULL(CP.DataText2,''),
		ISNULL(CP.DataText3,''),
		ISNULL(CP.DataText4,''),
		U.[Password]
	FROM Core_User U WITH(NOLOCK)
	INNER JOIN Core_Profile CP WITH(NOLOCK)
	ON	CP.ApplicationUserId = U.Id
	INNER JOIN Core_Profile CB WITH(NOLOCK)
	ON	CB.ApplicationUserId = U.CreatedBy
	INNER JOIN Core_Profile MB WITH(NOLOCK)
	ON	MB.ApplicationUserId = U.ModifiedBy
	INNER JOIN Core_Language L WITH(NOLOCK)
	ON	L.Id = U.Language
	LEFT JOIN Core_CompanyMemberShip CMS WITH(NOLOCK)
	ON	CMS.UserId = U.Id

	WHERE
		U.Id = @ApplicationUserId
	AND
	(
		CMS.UserId IS NOT NULL
		OR	
		U.Core = 1
	)

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
CREATE PROCEDURE [dbo].[Core_User_GetCompanies]
	@UserId bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		C.CompanyId
	FROM Core_CompanyMemberShip C
	WHERE
		C.Active = 1
	AND C.UserId = @UserId

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
CREATE PROCEDURE [dbo].[Core_Profile_ByApplicationUserId]
	@ApplicationUserId bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		ApplicationUserId,
        Name,
		ISNULL(LastName,''),
		ISNULL(LastName2,''),
		ISNULL(Phone,''),
		ISNULL(Mobile,''),
		ISNULL(PhoneEmergency,''),
		ISNULL(Fax,''),
		ISNULL(EmailAlternative,''),
		ISNULL(Twitter,''),
		ISNULL(LinkedIn,''),
		ISNULL(Instagram,''),
		ISNULL(Facebook,''),
		ISNULL(Gender,-1),
		ISNULL(IdentificationCard,''),
		ISNULL(IMEI,''),
		BirthDate,
		ISNULL(AddressWayType,0),
		ISNULL(Address,''),
		ISNULL(PostalCode,''),
		ISNULL(City,''),
		ISNULL(Province,''),
		ISNULL(State,''),
		ISNULL(Country,''),
		ISNULL(Latitude,0),
		ISNULL(Longitude,0),
		ISNULL(Web,'')
	FROM Core_Profile CP WITH(NOLOCK)

	WHERE
		CP.ApplicationUserId = @ApplicationUserId
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_User_ChangePassword]
	@ApplicationUserId bigint,
	@Actual nvarchar(50),
	@NewPass nvarchar(50),
	@Result int output
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @Exists int

	SELECT 
		@Exists = COUNT(U.id)
	FROM
		Core_User U WITH(NOLOCK)
	WHERE
		U.Active = 1
	AND U.Id = @ApplicationUserId
	AND U.Pass = @Actual

	IF @Exists =1
		BEGIN

			UPDATE Core_User SET
				Pass = @NewPass,
				ModifiedBy = @ApplicationUserId,
				ModifiedOn = GETUTCDATE()
			WHERE
				Id = @ApplicationUserId

			SET @Result = 1
		END
	ELSE
		BEGIN
			SET @Result = -1
		END


END


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Core_User_GetGrants]
	@ApplicationUserId bigint,
	@CompanyId bigint
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		ISNULL(G.SecurityGroupId,-1) AS GroupId,
		ISNULL(G.ApplicationUserId,-1) AS ApplicationUserId,
		ISNULL(G.ItemId,-1) AS ItemId,
		ISNULL(G.ItemName,'') AS ItemName,
		ISNULL(G.Grants,'') AS Grants
	FROM Security_Grants G WITH(NOLOCK)
	LEFT JOIN Core_Membership MS WITH(NOLOCK)
	ON	G.SecurityGroupId = MS.GroupId
	AND	MS.UserId = @ApplicationUserId
	AND MS.CompanyId = @CompanyId
	AND G.CompanyId = MS.CompanyId

	WHERE
		G.ApplicationUserId = @ApplicationUserId
	OR MS.UserId IS NOT NULL

	
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
CREATE PROCEDURE [dbo].[Core_Company_ByUserId]
	@ApplicationUserId bigint
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Core bit

	SELECT @Core = Core FROM Core_User WITH(NOLOCK) WHERE Id = @ApplicationUserId

	IF @Core = 1 
		BEGIN
			SELECT
				C.Id,
				C.[Name],
				ISNULL(C.Code,'') AS Code
			FROM Core_Company C WITH(NOLOCK)
			WHERE
				C.Active = 1

		END
	ELSE
		BEGIN
			SELECT 
				C.Id,
				C.[Name],
				ISNULL(C.Code,'') AS Code
			FROM Core_Company C WITH(NOLOCK)
			INNER JOIN Core_CompanyMemberShip CMS WITH(NOLOCK)
			ON	
				CMS.UserId = @ApplicationUserId
			AND	CMS.CompanyId = C.Id
			AND CMS.Active = 1
			AND C.Active = 1
		END
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- ALTERdate: <ALTERDate,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_SecurityGroup_GetById]
	@SecurityGroupId bigint,
	@CompanyId bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		G.Id,
		G.CompanyId,
		G.[Name] AS Name,
		ISNULL(G.[Description],'') AS Description,
		G.Deletable,
		G.Core,
		G.BillingAccess,
		G.RemindAlert,
		ISNULL(G.MainUserId,-1) AS MainUserId,
		G.CreatedBy,
		ISNULL(CB.[Name],'') AS CreatedByFirstName,
		ISNULL(CB.LastName,'') AS CreatedByLastName,
		ISNULL(CB.LastName2,'') AS CreatedByLastName2,
		G.CreatedOn,
		G.ModifiedBy,
		ISNULL(MB.[Name],'') AS ModifiedByName,
		ISNULL(MB.LastName,'') AS ModifiedByLastName,
		ISNULL(MB.LastName2,'') AS ModifiedByLastName2,
		G.ModifiedOn,
		G.Active
	FROM Core_Group G WITH(NOLOCK)
	LEFT JOIN Core_Profile CB WITH(NOLOCK)
	ON CB.ApplicationUserId = G.CreatedBy
	LEFT JOIN Core_Profile MB WITH(NOLOCK)
	ON MB.ApplicationUserId = G.ModifiedBy

	WHERE
		G.Active = 1
	AND G.Id = @SecurityGroupId
	AND
	(
		G.CompanyId = 0
		OR
		G.CompanyId = @CompanyId
	)
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Core_User_GetAllForList]
	@CompanyId bigint
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		U.Id,
		U.Email,
		U.Locked,
		U.Core,
		U.Language,
		L.Name,
		L.LocaleName,
		L.ISO,
		U.TechnicalUser,
		U.PrimaryUser,
		U.AdminUser,
		ISNULL(CP.Name,''),
		ISNULL(CP.LastName,''),
		ISNULL(CP.LastName2,''),
		U.Active,
		U.[External],
		U.Corporative
	FROM Core_User U WITH(NOLOCK)
	INNER JOIN Core_Profile CP WITH(NOLOCK)
	ON	CP.ApplicationUserId = U.Id
	INNER JOIN Core_Profile CB WITH(NOLOCK)
	ON	CB.ApplicationUserId = U.CreatedBy
	INNER JOIN Core_Profile MB WITH(NOLOCK)
	ON	MB.ApplicationUserId = U.ModifiedBy
	INNER JOIN Core_Language L WITH(NOLOCK)
	ON	L.Id = U.Language
	LEFT JOIN Core_CompanyMemberShip CMS WITH(NOLOCK)
	ON	CMS.UserId = U.Id
	AND	CMS.CompanyId = @CompanyId

	WHERE
		CMS.UserId IS NOT NULL
	OR	U.Core = 1

END




