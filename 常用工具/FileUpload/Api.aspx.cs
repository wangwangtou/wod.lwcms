using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace FileUpload
{
    public partial class Api : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                using (StreamReader sr = new StreamReader(Request.InputStream))
                {
                    string ipt = sr.ReadToEnd();
                }

                string fn = Request.Form["fn"];
                string bufferHex = Request.Form["buffer"];

                byte[] buffer = Convert.FromBase64String(bufferHex);

                FileInfo fi = new FileInfo(fn);

                string path = Server.MapPath("~/data");

                FileInfo nfi = new FileInfo(Path.Combine(path, fi.Name));
                if (!nfi.Directory.Exists)
                    nfi.Directory.Create();
                File.WriteAllBytes(nfi.FullName, buffer);
            }
        }
    }
}