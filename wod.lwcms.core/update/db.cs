using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using wod.lwcms.commands;
using wod.lwcms.dataaccess;

namespace wod.lwcms.update
{
    public interface idb
    {
        string getSql(string sqlPath);
        void excute(List<string> sqls);
    }

    public class db : idb
    {
        private DataAccessContext dataAcc;
        public db(DataAccessContext dataaccess)
        {
            this.dataAcc = dataaccess;
        }
        #region idb 成员

        public string getSql(string sqlPath)
        {
            return File.ReadAllText(sqlPath);
        }

        public void excute(List<string> sqls)
        {
            dataAcc.Open();
            dataAcc.BeginTransaction();
            try
            {
                foreach (var sql in sqls)
                {
                    dataAcc.ExecuteNonQuery(sql);
                }
                dataAcc.Commit();
            }
            catch (Exception)
            {
                dataAcc.Rollback();
                throw;
            }
        }

        #endregion
    }

}
