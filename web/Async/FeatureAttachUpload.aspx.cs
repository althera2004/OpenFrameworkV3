// --------------------------------
// <copyright file="FeatureAttachUpload.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFramework.Web.Async
{
    using System;
    using System.IO;
    using System.Web.UI;
    using OpenFrameworkV3;
    using OpenFrameworkV3.Feature;
    using OpenFrameworkV3.Tools;

    public partial class FeatureAttachUpload : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var instancenName = this.Request.Form["instanceName"] as string;
            var companyId = Convert.ToInt64(this.Request.Form["companyId"]);
            var applicationUserId = Convert.ToInt64(this.Request.Form["applicationUserId"]);
            var itemDefinitionId = Convert.ToInt64(this.Request.Form["itemDefinitionId"]);
            var itemId = Convert.ToInt64(this.Request.Form["itemId"]);
            var file = this.Request.Files[0];

            var attach = new Attach
            {
                Id = Constant.DefaultId,
                FileName = file.FileName,
                Size = Convert.ToInt64(file.ContentLength),
                Active = true,
                CompanyId = companyId,
                ItemDefinitionId = itemDefinitionId,
                ItemId = itemId             
            };

            var finalPath = attach.Path(instancenName);
            var path = finalPath;

            var version = 1;
            var name = Path.GetFileNameWithoutExtension(finalPath);
            while (File.Exists(finalPath))
            {
                finalPath = path.Replace(name, name + " (" + version.ToString() + ")");
                version++;
            }

            Basics.VerifyFolder(finalPath, true);

            file.SaveAs(finalPath);

            attach.FileName = Path.GetFileName(finalPath);
            attach.Insert(applicationUserId, instancenName);

            attach = Attach.ById(attach.Id, instancenName);

            this.Response.Clear();
            this.Response.ContentType = "application/json";
            this.Response.Write(attach.Json);
            this.Response.Flush();
        }
    };
}