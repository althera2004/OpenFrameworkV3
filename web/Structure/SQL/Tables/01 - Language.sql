SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Core_Language](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[LocaleName] [nvarchar](50) NOT NULL,
	[ISO] [nvarchar](5) NOT NULL,
	[RightToLeft] [bit] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Core_Language] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


INSERT INTO [dbo].[Core_Language] ([Name] ,[LocaleName] ,[ISO] ,[RightToLeft] ,[CreatedBy] ,[CreatedOn] ,[ModifiedBy] ,[ModifiedOn] ,[Active]) VALUES ('Catal�' ,'Catal�' ,'ca-es' ,0 ,1 ,GETUTCDATE() ,1 ,GETUTCDATE() ,1)
INSERT INTO [dbo].[Core_Language] ([Name] ,[LocaleName] ,[ISO] ,[RightToLeft] ,[CreatedBy] ,[CreatedOn] ,[ModifiedBy] ,[ModifiedOn] ,[Active]) VALUES ('Castell�' ,'Castellano' ,'es-es' ,0 ,1 ,GETUTCDATE() ,1 ,GETUTCDATE() ,1)
GO

