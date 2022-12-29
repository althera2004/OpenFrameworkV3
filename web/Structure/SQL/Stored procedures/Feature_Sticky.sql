/****** Object:  StoredProcedure [dbo].[Feature_Sticky_Activate]    Script Date: 27/12/2022 15:54:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Feature_Sticky_Activate]
	@Id bigint,
	@CompanyId bigint,
	@ApplicationUserId bigint,
	@CreatedOn nvarchar(20)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Feature_Sticky] SET
	    Active = 1,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = convert(datetime, @CreatedOn, 101)
	WHERE
		Id = @Id
	AND CompanyId = @CompanyId
END
GO

/****** Object:  StoredProcedure [dbo].[Feature_Sticky_ByItemDefinitionId]    Script Date: 27/12/2022 15:54:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Feature_Sticky_ByItemDefinitionId]
	@ItemDefinitionId bigint,
	@CompanyId bigint
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		FN.Id,		
		FN.CompanyId,
		FN.ItemDefinitionId,
		FN.ItemId,
		FN.CreatedBy AS Author,
		ISNULL(FN.[Target],'-1') AS Target,
		ISNULL(FN.[Text],'') AS Text,
		FN.CreatedBy,
		ISNULL(CB.[Name],'') AS CreatedByName,
		ISNULL(CB.LastName,'') AS CreatedByLastName,
		ISNULL(CB.LastName2,'') AS CreatedByLastName2,
		FN.CreatedOn,
		FN.ModifiedBy,
		ISNULL(MB.[Name],'') AS ModifiedByName,
		ISNULL(MB.LastName,'') AS ModifiedByLastName,
		ISNULL(MB.LastName2,'') AS ModifiedByLastName2,
		FN.ModifiedOn,
		FN.Active

	FROM Feature_Sticky FN WITH(NOLOCK)
	INNER JOIN Core_Profile CB WITH(NOLOCK)
	ON	CB.ApplicationUserId = FN.CreatedBy
	INNER JOIN Core_Profile MB WITH(NOLOCK)
	ON	MB.ApplicationUserId = FN.ModifiedOn

	WHERE
		FN.CompanyId = @CompanyId
	AND FN.ItemDefinitionId = @ItemDefinitionId
	AND FN.Active = 1
END
GO

/****** Object:  StoredProcedure [dbo].[Feature_Sticky_GetByItemDefinitionId]    Script Date: 27/12/2022 15:54:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Feature_Sticky_GetByItemDefinitionId]
	@ItemDefinitionId bigint,
	@CompanyId bigint
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		FN.Id,		
		FN.CompanyId,
		FN.ItemDefinitionId,
		FN.ItemId,
		FN.CreatedBy AS Author,
		ISNULL(FN.[Target],'-1') AS Target,
		ISNULL(FN.[Text],'') AS Text,
		FN.CreatedBy,
		ISNULL(CB.[Name],'') AS CreatedByName,
		ISNULL(CB.LastName,'') AS CreatedByLastName,
		ISNULL(CB.LastName2,'') AS CreatedByLastName2,
		FN.CreatedOn,
		FN.ModifiedBy,
		ISNULL(MB.[Name],'') AS ModifiedByName,
		ISNULL(MB.LastName,'') AS ModifiedByLastName,
		ISNULL(MB.LastName2,'') AS ModifiedByLastName2,
		FN.ModifiedOn,
		FN.Active

	FROM Feature_Sticky FN WITH(NOLOCK)
	INNER JOIN Core_Profile CB WITH(NOLOCK)
	ON	CB.ApplicationUserId = FN.CreatedBy
	INNER JOIN Core_Profile MB WITH(NOLOCK)
	ON	MB.ApplicationUserId = FN.ModifiedOn

	WHERE
		FN.CompanyId = @CompanyId
	AND FN.ItemDefinitionId = @ItemDefinitionId
	AND FN.Active = 1
END
GO

/****** Object:  StoredProcedure [dbo].[Feature_Sticky_GetByItemId]    Script Date: 27/12/2022 15:54:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Feature_Sticky_GetByItemId]
	@ItemDefinitionId bigint,
	@ItemId bigint,
	@CompanyId bigint
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		FN.Id,		
		FN.CompanyId,
		FN.ItemDefinitionId,
		FN.ItemId,
		FN.CreatedBy AS Author,
		ISNULL(FN.[Target],'-1') AS Target,
		ISNULL(FN.[Text],'') AS Text,
		FN.CreatedBy,
		ISNULL(CB.[Name],'') AS CreatedByName,
		ISNULL(CB.LastName,'') AS CreatedByLastName,
		ISNULL(CB.LastName2,'') AS CreatedByLastName2,
		FN.CreatedOn,
		FN.ModifiedBy,
		ISNULL(MB.[Name],'') AS ModifiedByName,
		ISNULL(MB.LastName,'') AS ModifiedByLastName,
		ISNULL(MB.LastName2,'') AS ModifiedByLastName2,
		FN.ModifiedOn,
		FN.Active

	FROM Feature_Sticky FN WITH(NOLOCK)
	INNER JOIN Core_Profile CB WITH(NOLOCK)
	ON	CB.ApplicationUserId = FN.CreatedBy
	INNER JOIN Core_Profile MB WITH(NOLOCK)
	ON	MB.ApplicationUserId = FN.ModifiedBy

	WHERE
		FN.CompanyId = @CompanyId
	AND FN.ItemDefinitionId = @ItemDefinitionId
	AND FN.ItemId = @ItemId
	AND FN.Active = 1

	ORDER BY
		FN.CreatedOn DESC
END
GO

/****** Object:  StoredProcedure [dbo].[Feature_Sticky_Inactivate]    Script Date: 27/12/2022 15:54:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Feature_Sticky_Inactivate]
	@Id bigint,
	@CompanyId bigint,
	@ApplicationUserId bigint
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Feature_Sticky] SET
	    Active = 0,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @Id
	AND CompanyId = @CompanyId
END
GO

/****** Object:  StoredProcedure [dbo].[Feature_Sticky_Insert]    Script Date: 27/12/2022 15:54:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Feature_Sticky_Insert]
	@Id bigint output,
	@CompanyId bigint,
	@Target nchar(100),
	@Text nvarchar(2000),
	@ItemDefinitionId bigint,
	@ItemId bigint,
	@ApplicationUserId bigint,
	@CreatedOn datetime
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Feature_Sticky]
	(
		[CompanyId],
		[Target],
		[Text],
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
		@Target,
		@Text,
		@ItemDefinitionId,
		@ItemId,
		@ApplicationUserId,
		@CreatedOn,
		@ApplicationUserId,
		@CreatedOn,
		1
	)

	SET @Id = @@IDENTITY
END
GO

/****** Object:  StoredProcedure [dbo].[Feature_Sticky_Update]    Script Date: 27/12/2022 15:54:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Feature_Sticky_Update]
	@Id bigint,
	@CompanyId bigint,
	@Text nvarchar(2000),
	@ApplicationUserId bigint,
	@ModifiedOn datetime
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Feature_Sticky] SET
	    [Text] = @Text,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = @ModifiedOn
	WHERE
		Id = @Id
	AND CompanyId = @CompanyId
END
GO

