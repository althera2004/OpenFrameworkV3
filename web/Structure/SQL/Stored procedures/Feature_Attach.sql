/****** Object:  StoredProcedure [dbo].[Feature_Attach_ById]    Script Date: 27/12/2022 15:55:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Feature_Attach_ById]
	@Id bigint
AS
BEGIN
	
	SELECT
		A.Id,
		A.CompanyId,
		A.FileName,
		A.Size,
		A.CreatedBy,
		ISNULL(CB.Name,''),
		ISNULL(CB.LastName,''),
		ISNULL(CB.LastName2,''),
		A.CreatedOn,
		A.ModifiedBy,
		ISNULL(MB.Name,''),
		ISNULL(MB.LastName,''),
		ISNULL(MB.LastName2,''),
		A.ModifiedOn,
		A.Active,
		A.ItemDefinitionId,
		A.ItemId
	FROM Feature_Attach A WITH(NOLOCK)
	INNER JOIN Core_Profile CB WITH(NOLOCK)
	ON	CB.ApplicationUserId = A.CreatedBy
	INNER JOIN Core_Profile MB WITH(NOLOCK)
	ON	MB.ApplicationUserId = A.ModifiedBy

	WHERE
		A.Id = @Id

END
GO

/****** Object:  StoredProcedure [dbo].[Feature_Attach_ByItemId]    Script Date: 27/12/2022 15:55:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Feature_Attach_ByItemId]
	@ItemDefinitionId bigint,
	@ItemId bigint
AS
BEGIN
	
	SELECT
		A.Id,
		A.CompanyId,
		A.FileName,
		A.Size,
		A.CreatedBy,
		ISNULL(CB.Name,''),
		ISNULL(CB.LastName,''),
		ISNULL(CB.LastName2,''),
		A.CreatedOn,
		A.ModifiedBy,
		ISNULL(MB.Name,''),
		ISNULL(MB.LastName,''),
		ISNULL(MB.LastName2,''),
		A.ModifiedOn,
		A.Active
	FROM Feature_Attach A WITH(NOLOCK)
	INNER JOIN Core_Profile CB WITH(NOLOCK)
	ON	CB.ApplicationUserId = A.CreatedBy
	INNER JOIN Core_Profile MB WITH(NOLOCK)
	ON	MB.ApplicationUserId = A.ModifiedBy

	WHERE
		A.ItemDefinitionId = @ItemDefinitionId
	AND A.ItemId = @ItemId
	AND A.Active = 1

END
GO

/****** Object:  StoredProcedure [dbo].[Feature_Attach_Inactivate]    Script Date: 27/12/2022 15:55:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Feature_Attach_Inactivate]
	@Id bigint,
	@ApplicationUserId bigint
AS
BEGIN
	
	UPDATE [dbo].[Feature_Attach] SET
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETUTCDATE(),
		Active = 0
	WHERE
		Id = @Id

END
GO

/****** Object:  StoredProcedure [dbo].[Feature_Attach_Insert]    Script Date: 27/12/2022 15:55:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Feature_Attach_Insert]
	@Id bigint output,
	@ItemDefinitionId bigint,
	@ItemId bigint,
	@FileName nvarchar(150),
	@Size bigint,
	@CompanyId bigint,
	@ApplicationUserId bigint
AS
BEGIN
	
	INSERT INTO [dbo].[Feature_Attach]
	(
		[CompanyId],
		[ItemDefinitionId],
		[ItemId],
		[FileName],
		[Size],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn],
		[Active]
	)
	VALUES
	(
		@CompanyId,
		@ItemDefinitionId,
		@ItemId,
		@FileName,
		@Size,
		@ApplicationUserId,
		GETUTCDATE(),
		@ApplicationUserId,
		GETUTCDATE(),
		1
	)

	SET @Id = @@IDENTITY

END
GO

