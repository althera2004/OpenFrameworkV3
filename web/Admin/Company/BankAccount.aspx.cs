// --------------------------------
// <copyright file="BankAccount.cs" company="OpenFramework">
//     Copyright (c) 2022 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV2.Web.Admin.Company
{
    using System;
    using System.Web.UI;
    using OpenFrameworkV3.Core;
    using OpenFrameworkV3.Core.Companies;

    public partial class BankAccount : Page
    {

        /// <summary>Master page</summary>
        private Main master;

        public string BankAccounts { get; private set; }

        public void Page_Init(object o, EventArgs e)
        {
            Instance.CheckPersistence();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddEncryptedLink("Configuració de companyia", "/Admin/Company/");
            this.master.BreadCrumb.AddLeaf("Cuentas bancarias");
            this.master.BreadCrumb.SetTitle("Companyia: " + this.master.Company.Name);

            this.master.AddScript("/js/jquery.mask.js");
            this.master.AddScript("/Admin/Company/BankAccount.js");

            var bankAccounts = CompanyBankAccount.ByCompany(this.master.Company.Id, this.master.Instance.Name);

            this.BankAccounts = CompanyBankAccount.JsonList(bankAccounts);
            this.master.SetPageType("pageAdmin");
        }
    }
}