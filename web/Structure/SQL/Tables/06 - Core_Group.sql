SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Core_Group](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](150) NULL,
	[Core] [bit] NULL,
	[RemindAlert] [bit] NULL,
	[BillingAccess] [bit] NULL,
	[MainUserId] [bigint] NULL,
	[Deletable] [bit] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
	[CompanyId] [bigint] NULL,
 CONSTRAINT [PK_Core_Group] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO