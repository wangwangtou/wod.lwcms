#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王星 时间：2015-02-06 12:01:29
* 文件名：updateManage
* 版本：V1.0.1
*
* 修改者： 时间： 
* 修改说明：
* ========================================================================
*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

namespace wod.lwcms.update
{
    public class updateManage
    {
        private HttpContext context;
        private izip zip;
        private idb db;
        public updateManage(HttpContext context,zip zip,db db)
        {
            this.context = context;
            this.zip = zip;
            this.db = db;
        }

        public Stream download(string dpath)
        {
            dpath = "~/"+dpath.TrimStart('~','/');
            var path = context.Server.MapPath(dpath);
            return zip.ZipPath(path);
        }

        public string getName()
        {
            return string.Format("{0:yyMMdd_HHmmss}.zip", DateTime.Now);
        }

        public void update()
        {
            Stream stream = context.Request.Files["updatepkg"].InputStream;
            var temp = Environment.GetEnvironmentVariable("TEMP");
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(temp,"wod_update"));
            if (!dir.Exists) dir.Create();
            zip.UnZipPath(dir.FullName, stream);
            var targetPath = context.Server.MapPath("~/");
            zip.CopyTo(dir.FullName, targetPath);
            var sqlFile = new FileInfo(Path.Combine(targetPath, "update.sql"));
            if (sqlFile.Exists)
            {
                string allSql = db.getSql(sqlFile.FullName);
                List<string> sqls = new List<string>();
                foreach (var item in allSql.Split(';'))
                {
                    var sql = item.Trim();
                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqls.Add(sql);
                    }
                }
                db.excute(sqls);
                sqlFile.Delete();
            }
            Directory.Delete(dir.FullName, true);
        }

        public void restart()
        {
            //System.Web.Hosting.ApplicationManager.GetApplicationManager().ShutdownApplication(System.Web.Hosting.HostingEnvironment.ApplicationID);
        }
    }
}
