
/****** Object:  Table [dbo].[Grant_Item]    Script Date: 10/11/2022 15:45:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Security_Grants](
	[CompanyId] [bigint] NOT NULL,
	[SecurityGroupId] [bigint] NOT NULL,
	[ApplicationUserId] [bigint] NOT NULL,
	[ItemId] [bigint] NOT NULL,
	[ItemName] [nvarchar](50) NOT NULL,
	[Grants] [nvarchar](15) NULL,
 CONSTRAINT [PK_Grant_Item] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[SecurityGroupId] ASC,
	[ApplicationUserId] ASC,
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


