SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Core_CompanyMemberShip](
	[UserId] [bigint] NOT NULL,
	[CompanyId] [bigint] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[Active] [bit] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Core_CompanyMemberShip]  WITH CHECK ADD  CONSTRAINT [FK_Core_CompanyMemberShip_Core_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Core_Company] ([Id])
GO

ALTER TABLE [dbo].[Core_CompanyMemberShip] CHECK CONSTRAINT [FK_Core_CompanyMemberShip_Core_Company]
GO