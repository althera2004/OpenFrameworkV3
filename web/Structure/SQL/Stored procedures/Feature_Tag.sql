USE [openf_support]
GO

/****** Object:  StoredProcedure [dbo].[Feature_Tag_ById]    Script Date: 27/12/2022 13:53:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Feature_Tag_ById]
	@Id bigint,
	@CompanyId bigint
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		T.Id,
		T.CompanyId,
		T.Tags,
		T.ItemDefinitionId,
		T.Id,
		T.CreatedBy,
		ISNULL(CB.Name,''),
		ISNULL(CB.LastName,''),
		ISNULL(CB.LastName2,''),
		T.CreatedOn,
		T.ModifiedBy,
		ISNULL(MB.Name,''),
		ISNULL(MB.LastName,''),
		ISNULL(MB.LastName2,''),
		T.ModifiedOn,
		T.Active
	FROM Feature_Tag T WITH(NOLOCK)
	INNER JOIN Core_Profile CB WITH(NOLOCK)
	ON	CB.ApplicationUserId = T.CreatedBy
	INNER JOIN Core_Profile MB WITH(NOLOCK)
	ON	MB.ApplicationUserId = T.ModifiedBy

	WHERE
		Id = @Id
	AND CompanyId = @CompanyId

END
GO

/****** Object:  StoredProcedure [dbo].[Feature_Tag_ByItemId]    Script Date: 27/12/2022 13:53:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Feature_Tag_ByItemId]
	@ItemDefinitionId bigint,
	@ItemId bigint,
	@CompanyId bigint
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		T.Id,
		T.CompanyId,
		T.Tags,
		T.ItemDefinitionId,
		T.Id,
		T.CreatedBy,
		ISNULL(CB.Name,''),
		ISNULL(CB.LastName,''),
		ISNULL(CB.LastName2,''),
		T.CreatedOn,
		T.ModifiedBy,
		ISNULL(MB.Name,''),
		ISNULL(MB.LastName,''),
		ISNULL(MB.LastName2,''),
		T.ModifiedOn,
		T.Active
	FROM Feature_Tag T WITH(NOLOCK)
	INNER JOIN Core_Profile CB WITH(NOLOCK)
	ON	CB.ApplicationUserId = T.CreatedBy
	INNER JOIN Core_Profile MB WITH(NOLOCK)
	ON	MB.ApplicationUserId = T.ModifiedBy

	WHERE
		ItemDefinitionId = @ItemDefinitionId
	AND	ItemId = @ItemId
	AND CompanyId = @CompanyId
	AND Active = 1

END
GO

/****** Object:  StoredProcedure [dbo].[Feature_Tag_Insert]    Script Date: 27/12/2022 13:53:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Feature_Tag_Insert]
	@Id bigint output,
	@ItemDefintionId bigint,
	@ItemId bigint,
	@CompanyId bigint,
	@Tags nvarchar(2000),
	@ApplicationUserId bigint
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[Feature_Tag]
	(
		[CompanyId],
		[Tags],
		[ItemDefinitionId],
		[ItemId],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn],
		[Active]
	)
	VALUES
	(
		@CompanyId,
		@Tags,
		@ItemDefintionId,
		@ItemId,
		@ApplicationUserId,
		GETUTCDATE(),
		@ApplicationUserId,
		GETUTCDATE(),
		1
	)
    
	SET @Id = @@IDENTITY


END
GO

/****** Object:  StoredProcedure [dbo].[Feature_Tag_Update]    Script Date: 27/12/2022 13:53:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Feature_Tag_Update]
	@ItemDefintionId bigint,
	@ItemId bigint,
	@CompanyId bigint,
	@Tags nvarchar(2000),
	@ApplicationUserId bigint
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE Feature_Tag SET
		Tags = @Tags,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETUTCDATE()
	WHERE
		ItemId = @ItemId
	AND ItemDefinitionId = @ItemDefintionId
	AND CompanyId = @CompanyId

END
GO

