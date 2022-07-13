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

ALTER TABLE [dbo].[Core_CompanyAddress]  WITH CHECK ADD  CONSTRAINT [FK_Core_CompanyAddress_Core_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Core_Company] ([Id])
GO

ALTER TABLE [dbo].[Core_CompanyAddress] CHECK CONSTRAINT [FK_Core_CompanyAddress_Core_Company]
GO

ALTER TABLE [dbo].[Core_CompanyBankAccount]  WITH CHECK ADD  CONSTRAINT [FK_Core_CompanyBankAccount_Core_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Core_Company] ([Id])
GO

ALTER TABLE [dbo].[Core_CompanyBankAccount] CHECK CONSTRAINT [FK_Core_CompanyBankAccount_Core_Company]
GO

ALTER TABLE [dbo].[Core_CompanyConfig]  WITH CHECK ADD  CONSTRAINT [FK_Core_CompanyConfig_Core_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Core_Company] ([Id])
GO

ALTER TABLE [dbo].[Core_CompanyConfig] CHECK CONSTRAINT [FK_Core_CompanyConfig_Core_Company]
GO

ALTER TABLE [dbo].[Core_CompanyContactPerson]  WITH CHECK ADD  CONSTRAINT [FK_Core_CompanyContactPerson_Core_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Core_Company] ([Id])
GO

ALTER TABLE [dbo].[Core_CompanyContactPerson] CHECK CONSTRAINT [FK_Core_CompanyContactPerson_Core_Company]
GO