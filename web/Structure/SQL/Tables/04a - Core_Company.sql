SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Core_CompanyAddress](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [bigint] NOT NULL,
	[LocationWayTypeId] [bigint] NULL,
	[LocationAddress] [nvarchar](150) NOT NULL,
	[LocationAddressComplement] [nvarchar](100) NULL,
	[LocationBloc] [nchar](10) NULL,
	[LocationStreetNumber] [nchar](10) NULL,
	[LocationStairs] [nchar](10) NULL,
	[LocationStage] [nchar](10) NULL,
	[LocationDoor] [nchar](10) NULL,
	[LocationPostalCodeId] [bigint] NULL,
	[Description] [nvarchar](50) NULL,
	[Main] [bit] NULL,
	[Billing] [bit] NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_Core_CompanyAddress] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Core_CompanyBankAccount](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [bigint] NOT NULL,
	[IBAN] [nchar](40) NULL,
	[Swift] [nchar](40) NULL,
	[BankName] [nvarchar](50) NULL,
	[PaymentType] [nchar](4) NULL,
	[ContractId] [nvarchar](50) NULL,
	[Alias] [nvarchar](50) NULL,
	[Main] [bit] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Core_CompanyBankAccount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Core_CompanyConfig](
	[CompanyId] [bigint] NULL,
	[Key] [nchar](20) NULL,
	[Value] [nvarchar](50) NULL
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Core_CompanyContactPerson](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [bigint] NOT NULL,
	[ApplicationUserId] [bigint] NULL,
	[Main] [bit] NULL,
	[ContractOwner] [bit] NULL,
	[Firma] [nvarchar](150) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[LastName2] [nvarchar](50) NULL,
	[NIF] [nchar](15) NULL,
	[Phone1] [nchar](30) NULL,
	[Phone2] [nchar](30) NULL,
	[EmergencyPhone] [nchar](30) NULL,
	[Email] [nvarchar](150) NULL,
	[AlternativeMail] [nvarchar](150) NULL,
	[JobPosition] [nvarchar](50) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Core_CompanyContactPerson] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Core_Company_Config]    Script Date: 15/12/2022 16:13:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Core_Company_Config](
	[CompanyId] [bigint] NOT NULL,
	[DocumentHistory] [bit] NOT NULL,
	[DocumentDelete] [int] NOT NULL,
	[DocumentTemporalAlive] [int] NOT NULL,
	[MassDownload] [bit] NOT NULL,
	[AvailableDocuments] [bigint] NOT NULL,
	[AvailableImages] [bigint] NOT NULL,
	[DiskQuote] [bigint] NOT NULL,
	[FeatureSticky] [bit] NOT NULL,
	[FeatureCustomAlerts] [bit] NOT NULL,
	[FeatureUserLock] [bit] NOT NULL,
	[FeatureFollowing] [bit] NOT NULL,
	[FeatureCustomLayout] [bit] NOT NULL,
	[OnPremise] [bit] NOT NULL,
	[SAT] [int] NOT NULL,
	[CustomConfig] [bit] NOT NULL
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Core_MailBox](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [bigint] NOT NULL,
	[SenderName] [nvarchar](100) NULL,
	[Main] [bit] NOT NULL,
	[Code] [nchar](20) NOT NULL,
	[MailAddress] [nvarchar](150) NOT NULL,
	[Server] [nchar](100) NOT NULL,
	[MailBoxType] [nvarchar](10) NOT NULL,
	[ReadPort] [int] NOT NULL,
	[SendPort] [int] NOT NULL,
	[SSL] [bit] NULL,
	[MailUser] [nvarchar](100) NOT NULL,
	[MailPassword] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Core_MailBox] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Core_Company_SecurityConfig](
	[CompanyId] [bigint] NULL,
	[IPAccess] [bit] NULL,
	[PasswordComplexity] [int] NULL,
	[Traceability] [int] NULL,
	[GrantPermission] [int] NULL,
	[FailedAttempts] [int] NULL,
	[MinimumPasswordLength] [int] NULL,
	[GroupUserMain] [bit] NULL,
	[FailedAttemptsMailNotification] [nvarchar](150) NULL,
	[MFA] [int] NULL,
	[CorporativeUsers] [bit] NULL,
	[FailedAttemptsSaveDays] [int] NULL,
	[PasswordCaducity] [bit] NULL,
	[PasswordCaducityDays] [int] NULL,
	[PasswordRepeat] [bit] NULL
) ON [PRIMARY]
GO

