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
		CP.Phone,
		CP.Mobile,
		CP.PhoneEmergency,
		CP.Fax,
		CP.EmailAlternative,
		CP.Twiter,
		CP.LinkedIn,
		CP.Instagram,
		CP.Facebook,
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
		CP.Phone,
		CP.Mobile,
		CP.PhoneEmergency,
		CP.Fax,
		CP.EmailAlternative,
		CP.Twiter,
		CP.LinkedIn,
		CP.Instagram,
		CP.Facebook,
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


