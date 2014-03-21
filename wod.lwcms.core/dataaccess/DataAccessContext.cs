using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Text;
using System.Data.Common;
using System.Configuration;

namespace wod.lwcms.dataaccess
{
    public class DataAccessContext : IDisposable
    {
        #region
        public bool RecordLog { get; set; }

        public DataAccessContext(DbProviderFactory dbProviderFactory, string connectionString)
        {
            this.m_dbProviderFactory = dbProviderFactory;
            this.m_connectionString = connectionString;
        }


        private DbProviderFactory m_dbProviderFactory = null;
        private string m_connectionString = null;
        /// <summary>
        /// 得到数据库提供者工厂对象
        /// </summary>
        private DbProviderFactory DbProviderFactory
        {
            get
            {
                return this.m_dbProviderFactory;
            }
        }
        private string ConnectionString
        {
            get
            {
                return this.m_connectionString;
            }
        }

        /// <summary>
        /// 数据库链接对象
        /// </summary>
        private DbConnection DbConnection
        {
            get;
            set;
        }

        /// <summary>
        /// 得到数据库链接是否已创建
        /// </summary>
        public bool IsOpen
        {
            get { return this.DbConnection != null; }
        }

        private bool CheckDbConnectionIsOpen()
        {
            bool result = this.IsOpen;
            if (!result)
            {
                throw new ApplicationException("请先初始化数据库链接对象！");
            }
            return result;
        }

        private bool m_allowCloseDbConnection = true;
        /// <summary>
        /// 得到是否允许关闭当前数据库链接
        /// </summary>
        private bool AllowCloseDbConnection
        {
            get { return this.m_allowCloseDbConnection; }
            set { this.m_allowCloseDbConnection = value; }
        }

        /// <summary>
        /// 打开一个新的数据库链接
        /// </summary>
        public virtual void Open()
        {
            try
            {
                this.DbConnection = this.DbProviderFactory.CreateConnection();
                this.DbConnection.ConnectionString = this.ConnectionString;
                this.DbConnection.Open();

                this.AddConnectionOpenLog(this.ConnectionString);
            }
            catch (Exception exp)
            {
                this.AddConnectionExceptionLog(this.ConnectionString, exp);
                throw exp;
            }
        }

        /// <summary>
        /// 使用已有的数据库链接
        /// </summary>
        /// <param name="_dataContext"></param>
        public void Open(DataAccessContext _dataContext)
        {
            this.DbConnection = _dataContext.DbConnection;
            this.DbTransaction = _dataContext.DbTransaction;
            this.m_dbProviderFactory = _dataContext.DbProviderFactory;
            this.AllowCloseDbConnection = false;
            this.RecordLog = _dataContext.RecordLog;
        }

        /// <summary>
        /// 关闭当前的数据库链接对象
        /// </summary>
        public void Close()
        {
            try
            {
                if ((this.AllowCloseDbConnection) && (this.IsOpen))
                {
                    this.DbConnection.Close();
                    this.DbConnection = null;
                    this.DbTransaction = null;
                    this.m_dbProviderFactory = null;

                    this.AddConnectionCloseLog(this.ConnectionString);
                }
            }
            catch (Exception exp)
            {
                this.AddConnectionExceptionLog(this.ConnectionString, exp);
            }
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        /// <remarks></remarks>
        public virtual void Dispose()
        {
            this.Close();
        }

        #endregion

        #region 得到当前的语言字符串

        /// <summary>
        /// 得到当前的语言字符串
        /// </summary>
        public virtual string DbLanguage
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLower();
            }
        }

        #endregion

        #region 事务

        /// <summary>
        /// 数据库事物对象
        /// </summary>
        private DbTransaction DbTransaction
        {
            get;
            set;
        }

        /// <summary>
        /// 得到是否开启了数据库事物
        /// </summary>
        private bool IsBeginDbTransaction
        {
            get { return this.DbTransaction != null; }
        }

        /// <summary>
        /// 开启事物
        /// </summary>
        public void BeginTransaction()
        {
            if (this.CheckDbConnectionIsOpen() && (!this.IsBeginDbTransaction))
            {
                this.DbTransaction = this.DbConnection.BeginTransaction();
            }
        }

        /// <summary>
        /// 开启事物
        /// </summary>
        /// <param name="_level">事物锁定级别</param>
        public void BeginTransaction(IsolationLevel _level)
        {
            if (this.CheckDbConnectionIsOpen())
            {
                this.DbTransaction = this.DbConnection.BeginTransaction(_level);
            }
        }

        /// <summary>
        /// 提交当前事物
        /// </summary>
        public void Commit()
        {
            try
            {
                if (this.IsBeginDbTransaction)
                {
                    this.DbTransaction.Commit();
                }
                else
                {
#warning to duq : request resource.
                    throw new ApplicationException("提交数据库事物失败，请先启用数据库事物！");
                }
            }
            catch (Exception exp)
            {
                this.AddTransactionExceptionLog(exp);
                throw exp;
            }
        }

        /// <summary>
        /// 回滚当前事物
        /// </summary>
        public void Rollback()
        {
            try
            {
                if (this.IsBeginDbTransaction)
                {
                    this.DbTransaction.Rollback();
                }
                else
                {
#warning to duq : request resource.
                    throw new ApplicationException("回滚数据库事物失败，请先启用数据库事物！");
                }
            }
            catch (Exception exp)
            {
                this.AddTransactionExceptionLog(exp);
                throw exp;
            }
        }

        #endregion

        #region SQL相关方法

        #region 执行SQL语句

        /// <summary>
        /// 执行指定的SQL语句，并返回影响的数据行数
        /// </summary>
        /// <param name="_sqlString">需要执行的SQL字符串</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string _sqlString)
        {
            return this.ExecuteNonQuery(_sqlString, CommandType.Text, null);
        }

        /// <summary>
        /// 执行指定的SQL语句，并返回影响的数据行数
        /// </summary>
        /// <param name="_sqlString">需要执行的SQL字符串</param>
        /// <param name="_commandType">SQL字符串类型</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string _sqlString, CommandType _commandType)
        {
            return this.ExecuteNonQuery(_sqlString, _commandType, null);
        }

        /// <summary>
        /// 执行指定的SQL语句，并返回影响的数据行数
        /// </summary>
        /// <param name="_sqlString">需要执行的SQL字符串</param>
        /// <param name="_commandType">SQL字符串类型</param>
        /// <param name="_parameters">参数列表</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string _sqlString, CommandType _commandType, IEnumerable<WODDbParameter> _parameters)
        {
            try
            {
                int result = 0;

                if (this.CheckDbConnectionIsOpen())
                {
                    using (DbCommand cmd = this.CreateDbCommand(_sqlString, _commandType, _parameters))
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                }

                this.AddSqlLog(_sqlString, _parameters, _commandType);

                return result;
            }
            catch (Exception err)
            {
                this.AddSqlExceptionLog(_sqlString, _parameters, _commandType, err);
                throw err;
            }
        }

        #endregion

        #region 查询相关

        /// <summary>
        /// 执行查询语句，并返回数据集对象
        /// </summary>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <returns></returns>
        public DataSet QueryDataSet(string _sqlString)
        {
            return this.QueryDataSet(_sqlString, CommandType.Text, null);
        }

        /// <summary>
        /// 执行查询语句，并返回数据集对象
        /// </summary>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <param name="_commandType">SQL字符串类型</param>
        /// <returns></returns>
        public DataSet QueryDataSet(string _sqlString, CommandType _commandType)
        {
            return this.QueryDataSet(_sqlString, _commandType, null);
        }

        /// <summary>
        /// 执行查询语句，并返回数据集对象
        /// </summary>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <param name="_commandType">SQL字符串类型</param>
        /// <param name="_parameters">参数列表</param>
        /// <returns></returns>
        public DataSet QueryDataSet(string _sqlString, CommandType _commandType, IEnumerable<WODDbParameter> _parameters)
        {
            try
            {
                DataSet result = new DataSet();

                using (DbCommand cmd = this.CreateDbCommand(_sqlString, _commandType, _parameters))
                {
                    DbDataAdapter adapter = this.DbProviderFactory.CreateDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(result);
                }

                this.AddSqlLog(_sqlString, _parameters, _commandType);

                return result;
            }
            catch (Exception exp)
            {
                this.AddSqlExceptionLog(_sqlString, _parameters, _commandType, exp);
                throw exp;
            }
        }

        /// <summary>
        /// 根据Sql语句得到查询结构
        /// </summary>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <returns></returns>
        public DataSet QueryDataSetScheme(string _sqlString)
        {
            return this.QueryDataSetScheme(_sqlString, CommandType.Text, null);
        }

        /// <summary>
        /// 根据Sql语句得到查询结构
        /// </summary>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <param name="_commandType">SQL字符串类型</param>
        /// <returns></returns>
        public DataSet QueryDataSetScheme(string _sqlString, CommandType _commandType)
        {
            return this.QueryDataSetScheme(_sqlString, _commandType, null);
        }

        /// <summary>
        /// 根据Sql语句得到查询结构
        /// </summary>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <param name="_commandType">SQL字符串类型</param>
        /// <param name="_parameters">参数列表</param>
        /// <returns></returns>
        public DataSet QueryDataSetScheme(string _sqlString, CommandType _commandType, IEnumerable<WODDbParameter> _parameters)
        {
            try
            {
                DataSet result = new DataSet();

                using (DbCommand cmd = this.CreateDbCommand(_sqlString, _commandType, _parameters))
                {
                    DbDataAdapter adapter = this.DbProviderFactory.CreateDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.FillSchema(result, SchemaType.Mapped);
                }

                this.AddSqlLog(_sqlString, _parameters, _commandType);

                return result;
            }
            catch (Exception exp)
            {
                this.AddSqlExceptionLog(_sqlString, _parameters, _commandType, exp);
                throw exp;
            }
        }

        /// <summary>
        /// 执行查询，得到从数据源读取行的只进流
        /// </summary>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <returns></returns>
        public DbDataReader QueryDataReader(string _sqlString)
        {
            return this.QueryDataReader(_sqlString, CommandType.Text, null);
        }

        /// <summary>
        /// 执行查询，得到从数据源读取行的只进流
        /// </summary>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <param name="_commandType">SQL字符串类型</param>
        /// <returns></returns>
        public DbDataReader QueryDataReader(string _sqlString, CommandType _commandType)
        {
            return this.QueryDataReader(_sqlString, _commandType, null);
        }

        /// <summary>
        /// 执行查询，得到从数据源读取行的只进流
        /// </summary>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <param name="_commandType">SQL字符串类型</param>
        /// <param name="_parameters">参数列表</param>
        /// <returns></returns>
        public DbDataReader QueryDataReader(string _sqlString, CommandType _commandType, IEnumerable<WODDbParameter> _parameters)
        {
            try
            {

                DbCommand cmd = this.CreateDbCommand(_sqlString, _commandType, _parameters);

                DbDataReader result = cmd.ExecuteReader();

                this.AddSqlLog(_sqlString, _parameters, _commandType);

                return result;
            }
            catch (Exception exp)
            {
                this.AddSqlExceptionLog(_sqlString, _parameters, _commandType, exp);
                throw exp;
            }
        }

        /// <summary>
        /// 执行查询，得到查询结果的第一行第一列的值
        /// </summary>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <returns></returns>
        public object QueryScalar(string _sqlString)
        {
            return this.QueryScalar(_sqlString, CommandType.Text, null);
        }

        /// <summary>
        /// 执行查询，得到查询结果的第一行第一列的值
        /// </summary>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <param name="_commandType">SQL字符串类型</param>
        /// <returns></returns>
        public object QueryScalar(string _sqlString, CommandType _commandType)
        {
            return this.QueryScalar(_sqlString, _commandType, null);
        }

        /// <summary>
        /// 执行查询，得到查询结果的第一行第一列的值
        /// </summary>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <param name="_commandType">SQL字符串类型</param>
        /// <param name="_parameters">参数列表</param>
        /// <returns></returns>
        public object QueryScalar(string _sqlString, CommandType _commandType, IEnumerable<WODDbParameter> _parameters)
        {
            try
            {
                object result = null;

                using (DbCommand cmd = this.CreateDbCommand(_sqlString, _commandType, _parameters))
                {
                    result = cmd.ExecuteScalar();
                }

                this.AddSqlLog(_sqlString, _parameters, _commandType);

                return result;
            }
            catch (Exception exp)
            {
                this.AddSqlExceptionLog(_sqlString, _parameters, _commandType, exp);
                throw exp;
            }
        }

        /// <summary>
        /// 执行查询，得到查询结果的第一行第一列的值
        /// （不记录参数值，用于防止记录敏感数据，例如用户登录时，记录了用户登录ID以及密码）
        /// </summary>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <param name="_commandType">SQL字符串类型</param>
        /// <param name="_parameters">参数列表</param>
        /// <returns></returns>
        public object QueryScalarWithoutLog(string _sqlString, CommandType _commandType, IEnumerable<WODDbParameter> _parameters)
        {
            try
            {
                object result = null;

                using (DbCommand cmd = this.CreateDbCommand(_sqlString, _commandType, _parameters))
                {
                    result = cmd.ExecuteScalar();
                }

                this.AddSqlLog(_sqlString, null, _commandType);

                return result;
            }
            catch (Exception exp)
            {
                this.AddSqlExceptionLog(_sqlString, null, _commandType, exp);
                throw exp;
            }
        }

        #endregion

        #region 更新DataSet

        /// <summary>
        /// 更新指定的数据集
        /// </summary>
        /// <param name="_changedData">待更新的数据集（为了尽量少的传输数据，请只传输更改过的数据）</param>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <returns></returns>
        public int UpdateDataSet(DataSet _changedData, string _sqlString)
        {
            return this.UpdateDataSet(_changedData, _sqlString, CommandType.Text, null);
        }

        /// <summary>
        /// 更新指定的数据集
        /// </summary>
        /// <param name="_changedData">待更新的数据集（为了尽量少的传输数据，请只传输更改过的数据）</param>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <param name="_commandType">SQL字符串类型</param>
        /// <returns></returns>
        public int UpdateDataSet(DataSet _changedData, string _sqlString, CommandType _commandType)
        {
            return this.UpdateDataSet(_changedData, _sqlString, _commandType, null);
        }

        /// <summary>
        /// 更新指定的数据集
        /// </summary>
        /// <param name="_changedData">待更新的数据集（为了尽量少的传输数据，请只传输更改过的数据）</param>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <param name="_commandType">SQL字符串类型</param>
        /// <param name="_parameters">参数列表</param>
        /// <returns></returns>
        public int UpdateDataSet(DataSet _changedData, string _sqlString, CommandType _commandType, IEnumerable<WODDbParameter> _parameters)
        {
            try
            {
                int result = 0;
                using (DbCommand cmd = this.CreateDbCommand(_sqlString, _commandType, _parameters))
                {

                    DbDataAdapter adapter = this.DbProviderFactory.CreateDataAdapter();
                    adapter.SelectCommand = cmd;
                    DbCommandBuilder cmdBuilder = this.DbProviderFactory.CreateCommandBuilder();
                    cmdBuilder.DataAdapter = adapter;
                    result = adapter.Update(_changedData);
                }

                this.AddSqlLog(_sqlString, _parameters, _commandType);

                return result;
            }
            catch (Exception exp)
            {
                this.AddSqlExceptionLog(_sqlString, _parameters, _commandType, exp);
                throw exp;
            }
        }

        #endregion

        #endregion

        #region 私有方法

        /// <summary>
        /// 创建数据库命令对象
        /// </summary>
        /// <param name="_sqlString">查询SQL字符串</param>
        /// <param name="_commandType">SQL字符串类型</param>
        /// <param name="_parameters">参数列表</param>
        /// <returns></returns>
        private DbCommand CreateDbCommand(string _sqlString, CommandType _commandType, IEnumerable<WODDbParameter> _parameters)
        {
            DbCommand result = null;
            if (this.CheckDbConnectionIsOpen())
            {
                result = this.DbProviderFactory.CreateCommand();
                result.Connection = this.DbConnection;
                result.CommandText = _sqlString;
                result.CommandType = _commandType;
                if (_parameters != null)
                {
                    foreach (var p in _parameters)
                    {
                        DbParameter dbParam = this.ConvertDbParameter(p);
                        result.Parameters.Add(dbParam);
                    }
                }
                if (this.IsBeginDbTransaction)
                {
                    result.Transaction = this.DbTransaction;
                }
            }
            return result;
        }

        private DbParameter ConvertDbParameter(WODDbParameter _param)
        {
            DbParameter result = this.DbProviderFactory.CreateParameter();
            result.ParameterName = _param.ParameterName;
            result.Direction = _param.ParameterDirection;
            result.Value = _param.ParameterValue;

            
            result.DbType = _param.ParameterType;
            return result;
        }

        /// <summary>
        /// 将参数列表转换为字符串形式（用于记录日志）
        /// </summary>
        /// <param name="_parameters">参数列表</param>
        /// <returns></returns>
        private string Convert(IEnumerable<WODDbParameter> _parameters)
        {
            string result = string.Empty;

            if (_parameters != null)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var item in _parameters)
                {
                    builder.Append(item.ToString() + ",");
                }
                result = builder.ToString();
            }

            return result;
        }

        #endregion

        #region 日志方法

        private void AddConnectionOpenLog(string _dbName)
        {
            if (this.RecordLog)
            {
            }
        }

        private void AddConnectionExceptionLog(string _dbName, Exception _exp)
        {
            if (this.RecordLog)
            {
            }
        }

        private void AddConnectionCloseLog(string _dbName)
        {
            if (this.RecordLog)
            {
            }
        }

        private void AddTransactionExceptionLog(Exception _exp)
        {
            if (this.RecordLog)
            {
            }
        }

        private void AddSqlLog(string _sqlString, IEnumerable<WODDbParameter> _parameters, CommandType _commandType)
        {
            if (this.RecordLog)
            {
            }
        }

        private void AddSqlExceptionLog(string _sqlString, IEnumerable<WODDbParameter> _parameters, CommandType _commandType, Exception _exp)
        {
            if (this.RecordLog)
            {
            }
        }

        #endregion
    }
}