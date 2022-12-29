SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- ALTERdate: <ALTERDate,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_SecurityGroup_GetAll]
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
	AND
	(
		G.CompanyId = 0
		OR
		G.CompanyId = @CompanyId
	)
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Core_SecurityGroup_GetMembersByCompany]
	@CompanyId bigint,
	@SecurityGroupId bigint
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		M.GroupId,
		M.UserId,
		U.Active
	FROM Core_Membership M WITH(NOLOCK)
	INNER JOIN Core_User U WITH(NOLOCK)
	ON	U.Id = M.UserId

	WHERE
		M.GroupId = @SecurityGroupId
	AND M.CompanyId = @CompanyId
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
CREATE PROCEDURE [dbo].[Core_SecurityGroup_ByUserId]
	@SecurityUserId bigint,
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
	INNER JOIN Core_Membership MS WITH(NOLOCK)
	ON	G.Id = MS.GroupId
	AND	MS.UserId = @SecurityUserId
	AND MS.CompanyId = @CompanyId
	LEFT JOIN Core_Profile CB WITH(NOLOCK)
	ON CB.ApplicationUserId = G.CreatedBy
	LEFT JOIN Core_Profile MB WITH(NOLOCK)
	ON MB.ApplicationUserId = G.ModifiedBy

	WHERE
		G.Active = 1
	AND
	(
		G.CompanyId = 0
		OR
		G.CompanyId = @CompanyId
	)
END