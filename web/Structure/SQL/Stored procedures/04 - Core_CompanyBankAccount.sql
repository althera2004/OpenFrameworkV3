/****** Object:  StoredProcedure [dbo].[Core_CompanyBankAccount_Activate]    Script Date: 03/07/2022 19:36:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_CompanyBankAccount_Activate]
	@Id bigint,
	@CompanyId bigint,
	@Active bit,
	@ApplicationUserId bigint
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE Core_CompanyBankAccount SET
		Active = @Active,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @Id
	AND CompanyId = @CompanyId

END
GO

/****** Object:  StoredProcedure [dbo].[Core_CompanyBankAccount_ByCompanyId]    Script Date: 03/07/2022 19:36:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_CompanyBankAccount_ByCompanyId]
	@CompanyId bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		B.Id,
		B.CompanyId,
		LEFT(B.IBAN,4)+'-'+RIGHT(LEFT(B.IBAN,8),4) +'-'+RIGHT(LEFT(B.IBAN,12),4) +'-'+RIGHT(LEFT(B.IBAN,16),4) +'-'+RIGHT(LEFT(B.IBAN,20),4) +'-'+RIGHT(LEFT(B.IBAN,24),4),
		ISNULL(B.Swift,''),
		ISNULL(B.BankName,''),
		ISNULL(B.Alias,''),
		B.Main,
		B.CreatedBy,
		ISNULL(CB.[Name],'') AS CreatedByName,
		ISNULL(CB.LastName,'') AS CreatedByLastName,
		ISNULL(CB.LastName2,'') AS CreatedByLastName2,
		B.CreatedOn,
		B.ModifiedBy,
		ISNULL(MB.[Name],'') AS ModifiedByName,
		ISNULL(MB.LastName,'') AS ModifiedLastName,
		ISNULL(MB.LastName2,'') AS ModifiedByLastName2,
		B.ModifiedOn,
		B.Active,
		ISNULL(B.ContractId,'') AS ContractId,
		ISNULL(B.PaymentType,'') AS PaymentType
	FROM Core_CompanyBankAccount B WITH(NOLOCK)
	INNER JOIN Core_Profile CB WITH(NOLOCK)
	ON	CB.ApplicationUserId = B.CreatedBy
	INNER JOIN Core_Profile MB WITH(NOLOCK)
	ON	MB.ApplicationUserId = B.ModifiedBy

	WHERE
		CompanyId = @CompanyId

END
GO

/****** Object:  StoredProcedure [dbo].[Core_CompanyBankAccount_ById]    Script Date: 03/07/2022 19:36:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_CompanyBankAccount_ById]
	@Id bigint,
	@CompanyId bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		B.Id,
		B.CompanyId,
		LEFT(B.IBAN,4)+'-'+RIGHT(LEFT(B.IBAN,8),4) +'-'+RIGHT(LEFT(B.IBAN,12),4) +'-'+RIGHT(LEFT(B.IBAN,16),4) + '-'+RIGHT(LEFT(B.IBAN,20),4) + '-'+RIGHT(LEFT(B.IBAN,24),4),
		ISNULL(B.Swift,''),
		ISNULL(B.BankName,''),
		ISNULL(B.Alias,''),
		B.Main,
		B.CreatedBy,
		ISNULL(CB.[Name],'') AS CreatedByName,
		ISNULL(CB.LastName,'') AS CreatedByLastName,
		ISNULL(CB.LastName2,'') AS CreatedByLastName2,
		B.CreatedOn,
		B.ModifiedBy,
		ISNULL(MB.[Name],'') AS ModifiedByName,
		ISNULL(MB.LastName,'') AS ModifiedLastName,
		ISNULL(MB.LastName2,'') AS ModifiedByLastName2,
		B.ModifiedOn,
		B.Active,
		ISNULL(B.ContractId,'') AS ContractId,
		ISNULL(B.PaymentType,'') AS PaymentType
	FROM Core_CompanyBankAccount B WITH(NOLOCK)
	INNER JOIN Core_Profile CB WITH(NOLOCK)
	ON	CB.ApplicationUserId = B.CreatedBy
	INNER JOIN Core_Profile MB WITH(NOLOCK)
	ON	MB.ApplicationUserId = B.ModifiedBy

	WHERE
		Id = @Id
	AND CompanyId = @CompanyId

END
GO

/****** Object:  StoredProcedure [dbo].[Core_CompanyBankAccount_Insert]    Script Date: 03/07/2022 19:36:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_CompanyBankAccount_Insert]
	@Id bigint output,
	@CompanyId bigint,
	@IBAN nchar(40),
	@Swift nchar(40),
	@BankName nvarchar(50),
	@Alias nvarchar(50),
	@ContractId nvarchar(50),
	@PaymentType nchar(4),
	@Main bit,
	@ApplicationUserId bigint
AS
BEGIN
	SET NOCOUNT ON;

	IF @Main = 1
	BEGIN
		UPDATE Core_CompanyBankAccount SET
			Main = 0
		WHERE
			CompanyId = @CompanyId
	END

    INSERT INTO [dbo].[Core_CompanyBankAccount]
	(
		[CompanyId],
		[IBAN],
		[Swift],
		[BankName],
		[Alias],
		[ContractId],
		[PaymentType],
		[Main],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn],
		[Active]
	)
	VALUES
	(
		@CompanyId,
		@IBAN,
		@Swift,
		@BankName,
		@Alias,
		@ContractId,
		@PaymentType,
		@Main,
		@ApplicationUserId,
		GETDATE(),
		@ApplicationUserId,
		GETDATE(),
		1
	)

	SET @Id = @@IDENTITY

END
GO

/****** Object:  StoredProcedure [dbo].[Core_CompanyBankAccount_SetMain]    Script Date: 03/07/2022 19:36:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_CompanyBankAccount_SetMain]
	@Id bigint,
	@CompanyId bigint,
	@ApplicationUserId bigint
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE Core_CompanyBankAccount SET
		Main = 0
	WHERE
		CompanyId = @CompanyId

    UPDATE Core_CompanyBankAccount SET
		Main = 1,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @Id
	AND CompanyId = @CompanyId

END
GO

/****** Object:  StoredProcedure [dbo].[Core_CompanyBankAccount_Update]    Script Date: 03/07/2022 19:36:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Core_CompanyBankAccount_Update]
	@Id bigint,
	@CompanyId bigint,
	@IBAN nchar(40),
	@Swift nchar(40),
	@BankName nvarchar(50),
	@Alias nvarchar(50),
	@ContractId nvarchar(50),
	@PaymentType nchar(4),
	@Main bit,
	@ApplicationUserId bigint
AS
BEGIN
	SET NOCOUNT ON;

	IF @Main = 1
	BEGIN
		UPDATE Core_CompanyBankAccount SET
			Main = 0
		WHERE
			CompanyId = @CompanyId
	END

    UPDATE [dbo].[Core_CompanyBankAccount] SET
		IBAN = @IBAN,
		Swift = @Swift,
		BankName = @BankName,
		Alias = @Alias,
		ContractId = @ContractId,
		PaymentType = @PaymentType,
		Main = @Main,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @Id
	AND CompanyId = @CompanyId

END
GO

