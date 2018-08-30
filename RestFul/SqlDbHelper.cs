using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;
using System.Collections.Generic;


namespace RestFul
{
    /// <summary>
    /// 数据访问抽象基础类
    /// </summary>
    public abstract class SqlDbHelper
    {
        public static string connectionString= ConfigurationManager.ConnectionStrings["MssqlConnectionString"].ConnectionString;

        public SqlDbHelper()
        {
            
        }

        public static int ExecuteSql(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                DataTable dt = null;
                try
                {
                    connection.Open();
                    string SQLString = @"select top {2} numComImg.* from 
                            ( select row_number() over(order by {1}) as rownumber,* from (select *  FROM {0}) as comImg )
                                as numComImg where rownumber>({2}*{3})";
                    SQLString = string.Format(SQLString, tableName, orderField, pageSize, pageNum);
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                    if (ds.Tables.Count > 0) dt = ds.Tables[0];
                }
                catch (System.Data.SqlClient.SqlException ex)
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                DataTable dt = null;
                try
                {
                    connection.Open();
                    string SQLString = @"select top {2} numComImg.* from 
                            ( select row_number() over(order by {1}) as rownumber,* from (select *  FROM {0} where {4}) as comImg )
                                as numComImg where rownumber>({2}*{3})";
                    SQLString = string.Format(SQLString, tableName, orderField, pageSize, pageNum, strWhere);
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                    if (ds.Tables.Count > 0) dt = ds.Tables[0];
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return dt;
            }
        }
        #endregion

        public static DataTable ExecuteSqlInsert(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    DataTable dt = null;
                    try
                    {
                        da.Fill(ds, "ds");
                        if (ds.Tables.Count > 0) dt = ds.Tables[0];
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return dt;
                }
            }
        }

        public static int ExecuteSqlUpdate(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
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

        public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
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


                foreach (SqlParameter parameter in cmdParms)
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
