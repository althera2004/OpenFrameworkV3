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