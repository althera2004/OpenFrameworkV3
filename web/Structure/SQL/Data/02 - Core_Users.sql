INSERT INTO [dbo].[Core_User]([Email],[Password],[FailedSignIn],[MustResetPassword],[Locked],[Core],[Corporative],[OpenFrameworkId],[ShowHelp],[Language],[PrimaryUser],[AdminUser],[TechnicalUser],[External],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn],[Active])VALUES ('info@openframework.cat','P@ssw0rd',0,0,0,1,0,1,0,1,1,1,1,0,1,GETDATE(),1,GETDATE(),1)
INSERT INTO [dbo].[Core_User]([Email],[Password],[FailedSignIn],[MustResetPassword],[Locked],[Core],[Corporative],[OpenFrameworkId],[ShowHelp],[Language],[PrimaryUser],[AdminUser],[TechnicalUser],[External],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn],[Active])VALUES ('jcastilla@openframework.cat','P@ssw0rd',0,0,0,1,0,1,0,1,1,1,1,0,1,GETDATE(),1,GETDATE(),1)

INSERT INTO [dbo].[Core_Profile]([ApplicationUserId],[Name],[LastName],[LastName2],[Gender],[IdentificationCard]) VALUES (1,'Open','Framework','',0,'')
INSERT INTO [dbo].[Core_Profile]([ApplicationUserId],[Name],[LastName],[LastName2],[Gender],[IdentificationCard]) VALUES (2,'Juan','Castilla','Calderón',0,'')

