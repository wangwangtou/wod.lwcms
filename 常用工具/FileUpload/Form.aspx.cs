using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LitJson;
using ParseToHtml;

namespace FileUpload
{
    public partial class Form : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                Parser parser = new Parser();
                JsonData data = parser.ParseForm(Request.Form);

                Response.Clear();
                string html = parser.ParseToHtml(data, "~/template/default/user.ascx");
                Response.Write(html);
                Response.End();
            }
        }
    }
}