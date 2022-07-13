SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Core_Company](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](15) NOT NULL,
	[LOPD] [bit] NOT NULL,
	[SubscriptionStart] [datetime] NOT NULL,
	[SubscriptionEnd] [datetime] NULL,
	[ContactPerson] [bigint] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
	[RazonSocial] [nvarchar](150) NULL,
	[Logo] [nvarchar](150) NULL,
	[CIF] [nchar](15) NULL,
	[ApplicationUserId] [bigint] NULL,
	[DefaultLanguage] [bigint] NULL,
	[Email] [nvarchar](150) NULL,
	[Web] [nvarchar](150) NULL,
	[Phone] [nchar](15) NULL,
	[Fax] [nchar](15) NULL,
	[ExtraData1] [nvarchar](50) NULL,
	[ExtraData2] [nvarchar](50) NULL,
	[Features] [nchar](200) NULL,
 CONSTRAINT [PK_Core_Company] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO