using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RestFul
{
    /// <summary>
    /// 数据访问抽象基础类
    /// </summary>
    public abstract class MysqlHelper
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString;

        public MysqlHelper()
        {

        }

        public static int ExecuteSql(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        #region 执行分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="orderField"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public static DataTable QueryTable(string tableName, string orderField = "id asc", int pageSize = 20, int pageNum = 0)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                DataTable dt = null;
                try
                {
                    connection.Open();
                    string SQLString = @"select * from " + tableName +" order by " + orderField + " limit " + (pageNum-1)*pageSize + "," + pageSize;
                    //SQLString = string.Format(SQLString, tableName, orderField, pageSize, pageNum);
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                    if (ds.Tables.Count > 0) dt = ds.Tables[0];
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return dt;
            }
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="pageSize">每页行数</param>
        /// <param name="pageNum">页数重0开始</param>
        /// <param name="strWhere">条件不包含where关键字</param>
        /// <returns></returns>
        public static DataTable QueryTable(string tableName, string orderField, int pageSize, int pageNum, string strWhere)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                DataTable dt = null;
                try
                {
                    connection.Open();
                    string SQLString = @"select * from " + tableName + " where " + strWhere + " order by " + orderField + " limit " + (pageNum - 1) * pageSize + "," + pageSize;
                    //SQLString = string.Format(SQLString, tableName, orderField, pageSize, pageNum, strWhere);
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                    if (ds.Tables.Count > 0) dt = ds.Tables[0];
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return dt;
            }
        }
        #endregion

        public static DataTable ExecuteSqlInsert(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    DataTable dt = null;
                    try
                    {
                        da.Fill(ds, "ds");
                        if (ds.Tables.Count > 0) dt = ds.Tables[0];
                        cmd.Parameters.Clear();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return dt;
                }
            }
        }

        public static int ExecuteSqlUpdate(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        throw e;
                    }
                }
            }
        }


        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {


                foreach (MySqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

    }
}
