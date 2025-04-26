using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Dal
{
    public static class SqlHelper
    {
        // 定义连接字符串
        private static readonly string connString = ConfigurationManager.ConnectionStrings["connString"].ToString();

        /// <summary>
        /// 执行增删改操作
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>受影响的行数</returns>
        public static int Update(string sql, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // 记录错误信息
                        LogError(ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行单一结果查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>查询结果</returns>
        public static object ExecuteScalar(string sql, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        conn.Open();
                        return cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        // 记录错误信息
                        LogError(ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多结果的查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>SQLDataReader对象</returns>
        public static SqlDataReader ExecuteReader(string sql, SqlParameter[] parameters = null)
        {
            SqlConnection conn = new SqlConnection(connString);
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                try
                {
                    conn.Open();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception ex)
                {
                    // 记录错误信息
                    LogError(ex);
                    conn.Close();
                    throw;
                }
            }
        }

        /// <summary>
        /// 执行返回DataTable的查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        try
                        {
                            conn.Open();
                            da.Fill(dt);
                            return dt;
                        }
                        catch (Exception ex)
                        {
                            // 记录错误信息
                            LogError(ex);
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 执行返回数据集的查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>DataSet对象</returns>
        public static DataSet ExecuteDataSet(string sql, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        try
                        {
                            conn.Open();
                            da.Fill(ds);
                            return ds;
                        }
                        catch (Exception ex)
                        {
                            // 记录错误信息
                            LogError(ex);
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns>服务器时间</returns>
        public static DateTime GetServerTime()
        {
            string sql = "SELECT GETDATE()";
            return (DateTime)ExecuteScalar(sql);
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="ex">异常对象</param>
        public static void LogError(Exception ex)
        {
            // 实现日志记录逻辑，例如写入文件或数据库
            Console.WriteLine(ex.Message);
        }
    }
}