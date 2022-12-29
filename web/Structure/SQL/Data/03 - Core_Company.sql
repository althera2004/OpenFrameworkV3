




INSERT INTO [dbo].[Core_Company]([Name],[Code],[LOPD],[SubscriptionStart],[ContactPerson],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn],[Active],[RazonSocial],[CIF],[ApplicationUserId],[DefaultLanguage],[Email],[Web])VALUES('OpenFramework','OPEN',1,GETDATE(),1,1,GETDATE(),1,GETDATE(),1,'Open Framework','',1,1,'info@openframework.cat','https://www.openframwork.cat')

           
INSERT INTO [dbo].[Core_CompanyMemberShip]([UserId],[CompanyId],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn],[Active])VALUES(1,1,1,GETDATE(),1,GETDATE(),1)

