namespace OpenFramework.Instance.ViuLleure
{
    using System;
    using System.Web.UI;
    using OpenFrameworkV3;

    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.Session["ApplicationUser"] == null)
            {
                this.Response.Redirect("/LogOut.aspx", Constant.EndResponse);
            }
            else
            {
                Go();
            }

            Context.ApplicationInstance.CompleteRequest();
        }

        private void Go()
        {
            var x = "";
        }
    }
}