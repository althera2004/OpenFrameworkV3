SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Core_Profile](
	[ApplicationUserId] [bigint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NULL,
	[LastName2] [nvarchar](50) NULL,
	[Phone] [nvarchar](20) NULL,
	[Mobile] [nvarchar](20) NULL,
	[PhoneEmergency] [nvarchar](20) NULL,
	[Fax] [nvarchar](20) NULL,
	[EmailAlternative] [nvarchar](100) NULL,
	[Twiter] [nvarchar](50) NULL,
	[LinkedIn] [nvarchar](50) NULL,
	[Instagram] [nvarchar](50) NULL,
	[Facebook] [nvarchar](50) NULL,
	[Gender] [int] NULL,
	[IdentificationCard] [nvarchar](15) NULL,
	[IMEI] [nvarchar](20) NULL,
	[BirthDate] [datetime] NULL,
	[AddressWayType] [int] NULL,
	[Address] [nvarchar](100) NULL,
	[PostalCode] [nvarchar](10) NULL,
	[City] [nvarchar](50) NULL,
	[Province] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[Latitude] [decimal](18, 8) NULL,
	[Longitude] [decimal](18, 8) NULL,
	[Web] [nvarchar](300) NULL,
	[DataText1] [nvarchar](50) NULL,
	[DataText2] [nvarchar](50) NULL,
	[DataText3] [nvarchar](50) NULL,
	[DataText4] [nvarchar](50) NULL,
 CONSTRAINT [PK_Core_Profile] PRIMARY KEY CLUSTERED 
(
	[ApplicationUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO