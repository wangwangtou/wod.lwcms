#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王星 时间：2015-02-05 16:29:32
* 文件名：daHelper
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

namespace wod.lwcms.dataaccess
{
    public static class daHelper
    {
        public static string buildMdbConnectionString(string provider,string path,string userId,string password)
        {
            return string.Format("Provider={0};Data Source={1};User Id={2};Password={3};", provider, path, userId, password);
        }
    }
}
