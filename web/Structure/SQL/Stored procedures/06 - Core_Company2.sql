SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_CompanyConfig_Get]
	@CompanyId bigint
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		CCC.CompanyId,
		CCC.DocumentHistory,
		CCC.DocumentDelete,
		CCC.DocumentTemporalAlive,
		CCC.MassDownload,
		CCC.AvailableDocuments,
		CCC.AvailableImages,
		CCC.DiskQuote,
		CCC.FeatureSticky,
		CCC.FeatureCustomAlerts,
		CCC.FeatureUserLock,
		CCC.FeatureFollowing,
		CCC.FeatureCustomLayout,
		CCC.OnPremise,
		CCC.SAT,
		CCC.CustomConfig
	FROM Core_Company_Config CCC WITH(NOLOCK)
	WHERE
		CCC.CompanyId = @CompanyId

END
GO

/****** Object:  StoredProcedure [dbo].[Core_MailBox_ByCompanyId]    Script Date: 15/12/2022 16:15:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_MailBox_ByCompanyId]
	@CompanyId bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		M.Id,
		M.CompanyId,
		M.Code,
		M.MailAddress,
		M.[Server],
		M.MailBoxType,
		M.ReadPort,
		M.SendPort,
		M.MailUser,
		M.MailPassword,
		ISNULL(M.[Description],'') AS Description,
		M.CreatedBy,
		ISNULL(CB.[Name],'') AS CreatedByFirtName,
		ISNULL(CB.LastName,'') AS CreatedByLastName,
		ISNULL(CB.LastName2,'') AS CreatedByLastName2,
		M.CreatedOn,
		M.ModifiedBy,
		ISNULL(MB.[Name],'') AS ModifiedByFirtName,
		ISNULL(MB.LastName,'') AS ModifiedByLastName,
		ISNULL(MB.LastName2,'') AS ModifiedByLastName2,
		M.ModifiedOn,
		M.Active,
		ISNULL(M.SenderName,'') AS SenderName,
		M.SSL,
		M.Main

	FROM Core_MailBox M WITH(NOLOCK)
	INNER JOIN Core_Profile CB WITH(NOLOCK)
	ON	CB.ApplicationUserId = M.CreatedBy
	INNER JOIN Core_Profile MB WITH(NOLOCK)
	ON	MB.ApplicationUserId = M.ModifiedBy

	WHERE
		M.CompanyId = @CompanyId
END
GO

/****** Object:  StoredProcedure [dbo].[Core_MailBox_Insert]    Script Date: 15/12/2022 16:15:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_MailBox_Insert]
	@Id bigint output,
	@CompanyId bigint,
    @SenderName nvarchar(100),
    @Main bit,
    @Code nchar(20),
    @MailAddress nvarchar(150),
    @Server nchar(100),
    @MailBoxType nvarchar(10),
    @ReadPort int,
    @SendPort int,
    @SSL bit,
    @MailUser nvarchar(100),
    @MailPassword nvarchar(50),
    @Description nvarchar(100),
	@ApplicationUserId bigint
AS
BEGIN
	SET NOCOUNT ON;

    
	INSERT INTO [dbo].[Core_MailBox]
           ([CompanyId]
           ,[SenderName]
           ,[Main]
           ,[Code]
           ,[MailAddress]
           ,[Server]
           ,[MailBoxType]
           ,[ReadPort]
           ,[SendPort]
           ,[SSL]
           ,[MailUser]
           ,[MailPassword]
           ,[Description]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
	 (
		@CompanyId,
        @SenderName,
        @Main,
        @Code,
        @MailAddress,
        @Server,
        @MailBoxType,
        @ReadPort,
        @SendPort,
        @SSL,
        @MailUser,
        @MailPassword,
        @Description,
        @ApplicationUserId,
        GETUTCDATE(),
        @ApplicationUserId,
        GETUTCDATE(),
        1
	)
	
	SET @Id = @@IDENTITY

END
GO

/****** Object:  StoredProcedure [dbo].[Core_MailxBox_Insert]    Script Date: 15/12/2022 16:15:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_MailxBox_Insert]
	@Id bigint output,
	@CompanyId bigint,
    @SenderName nvarchar(100),
    @Main bit,
    @Code nchar(20),
    @MailAddress nvarchar(150),
    @Server nchar(100),
    @MailBoxType nvarchar(10),
    @ReadPort int,
    @SendPort int,
    @SSL bit,
    @MailUser nvarchar(100),
    @MailPassword nvarchar(50),
    @Description nvarchar(100),
	@ApplicationUserId bigint
AS
BEGIN
	SET NOCOUNT ON;

    
	INSERT INTO [dbo].[Core_MailBox]
           ([CompanyId]
           ,[SenderName]
           ,[Main]
           ,[Code]
           ,[MailAddress]
           ,[Server]
           ,[MailBoxType]
           ,[ReadPort]
           ,[SendPort]
           ,[SSL]
           ,[MailUser]
           ,[MailPassword]
           ,[Description]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
	 (
		@CompanyId,
        @SenderName,
        @Main,
        @Code,
        @MailAddress,
        @Server,
        @MailBoxType,
        @ReadPort,
        @SendPort,
        @SSL,
        @MailUser,
        @MailPassword,
        @Description,
        @ApplicationUserId,
        GETUTCDATE(),
        @ApplicationUserId,
        GETUTCDATE(),
        1
	)
	
	SET @Id = @@IDENTITY

END
GO

