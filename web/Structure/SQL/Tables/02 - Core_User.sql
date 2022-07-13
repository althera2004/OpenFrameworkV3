SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Core_User](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PinCode] [nvarchar](4) NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[FailedSignIn] [int] NOT NULL,
	[MustResetPassword] [bit] NOT NULL,
	[Locked] [bit] NOT NULL,
	[Core] [bit] NULL,
	[Corporative] [bit] NULL,
	[OpenFrameworkId] [bigint] NULL,
	[ShowHelp] [bit] NULL,
	[ShortcutBlue] [bigint] NULL,
	[ShortcutGreen] [bigint] NULL,
	[ShortcutRed] [bigint] NULL,
	[ShortcutYellow] [bigint] NULL,
	[Language] [bigint] NULL,
	[PrimaryUser] [bit] NULL,
	[AdminUser] [bit] NULL,
	[TechnicalUser] [bit] NULL,
	[LastConnection] [datetime] NULL,
	[External] [bit] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Core_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO