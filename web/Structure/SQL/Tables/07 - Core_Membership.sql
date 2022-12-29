SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Core_Membership](
	[GroupId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[CompanyId] [bigint] NULL,
	[Main] [bit] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Core_Membership]([GroupId] ,[UserId] ,[CompanyId] ,[Main] ,[CreatedBy] ,[CreatedOn] ,[ModifiedBy] ,[ModifiedOn] ,[Active]) VALUES (1 ,1 ,0 ,0 ,1 ,GETUTCDATE() ,1 ,GETUTCDATE(), 1)
INSERT INTO [dbo].[Core_Membership]([GroupId] ,[UserId] ,[CompanyId] ,[Main] ,[CreatedBy] ,[CreatedOn] ,[ModifiedBy] ,[ModifiedOn] ,[Active]) VALUES (2 ,1 ,0 ,0 ,1 ,GETUTCDATE() ,1 ,GETUTCDATE(), 1)
INSERT INTO [dbo].[Core_Membership]([GroupId] ,[UserId] ,[CompanyId] ,[Main] ,[CreatedBy] ,[CreatedOn] ,[ModifiedBy] ,[ModifiedOn] ,[Active]) VALUES (3 ,1 ,0 ,0 ,1 ,GETUTCDATE() ,1 ,GETUTCDATE(), 1)
GO

