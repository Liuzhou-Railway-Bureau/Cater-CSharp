using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03.CaterDAL
{
    public class SqliteHelper
    {
        #region gets connection string
        /// <summary>
        /// gets connection string
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <returns>connection string</returns>
        public static string GetConnectionString(string conn)
        {
            return ConfigurationManager.ConnectionStrings[conn].ConnectionString;
        }
        #endregion

        #region executes non-query and returns the number of rows affected
        /// <summary>
        /// executes non-query statement
        /// </summary>
        /// <param name="connStr">connection string</param>
        /// <param name="sqlText">SQL statement</param>
        /// <param name="parameters">SQL parameters</param>
        /// <returns>the number of rows affected</returns>
        public static int ExecuteNonQuery(string connStr, string sqlText, params SQLiteParameter[] parameters)
        {
            int num = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connStr))
            {
                using (SQLiteCommand command = new SQLiteCommand(sqlText, connection))
                {
                    connection.Open();
                    command.Parameters.AddRange(parameters);
                    num = command.ExecuteNonQuery();
                }
            }
            return num;
        }
        #endregion

        #region executes query and returns the first column of the first row
        /// <summary>
        /// executes query statement
        /// </summary>
        /// <param name="connStr">connection string</param>
        /// <param name="sqlText">SQL statement</param>
        /// <param name="parameters">SQL parameters</param>
        /// <returns>the first column of the first row</returns>
        public static object ExecuteScalar(string connStr, string sqlText, params SQLiteParameter[] parameters)
        {
            object value = null;
            using (SQLiteConnection connection = new SQLiteConnection(connStr))
            {
                using (SQLiteCommand command = new SQLiteCommand(sqlText, connection))
                {
                    connection.Open();
                    command.Parameters.AddRange(parameters);
                    value = command.ExecuteScalar();
                }
            }
            return value;
        }

        /// <summary>
        /// executes query statement
        /// </summary>
        /// <typeparam name="T">a class</typeparam>
        /// <param name="connStr">connection string</param>
        /// <param name="sqlText">SQL statement</param>
        /// <param name="parameters">SQL parameters</param>
        /// <returns>the first column of the first row</returns>
        public static T ExecuteScalar<T>(string connStr, string sqlText, params SQLiteParameter[] parameters) where T : class
            // where constrains T
        {
            T value = null;
            using (SQLiteConnection connection = new SQLiteConnection(connStr))
            {
                using (SQLiteCommand command = new SQLiteCommand(sqlText, connection))
                {
                    connection.Open();
                    command.Parameters.AddRange(parameters);
                    value = (T)command.ExecuteScalar();
                }
            }
            return value;
        }
        #endregion

        #region executes a SQL and returns a data table or a data set
        /// <summary>
        /// executes a SQL
        /// </summary>
        /// <param name="connStr">connection string</param>
        /// <param name="sqlText">SQL statement</param>
        /// <param name="tableName">table name</param>
        /// <param name="parameters">SQL parameters</param>
        /// <returns>a data table with data</returns>
        public static DataTable ExecuteTable(string connStr, string sqlText, string tableName, params SQLiteParameter[] parameters)
        {
            return ExecuteTable(connStr, sqlText, tableName, CommandType.Text, parameters);
        }

        /// <summary>
        /// executes a SQL
        /// </summary>
        /// <param name="connStr">connection string</param>
        /// <param name="sqlText">SQL statement</param>
        /// <param name="dataSetName">data set name</param>
        /// <param name="parameters">SQL parameters</param>
        /// <returns>a data set with data</returns>
        public static DataSet ExecuteSet(string connStr, string sqlText, string dataSetName, params SQLiteParameter[] parameters)
        {
            return ExecuteSet(connStr, sqlText, dataSetName, CommandType.Text, parameters);
        }
        #endregion

        #region executes a SQL and returns a data reader
        /// <summary>
        /// executes a SQL
        /// </summary>
        /// <param name="connStr">connection string</param>
        /// <param name="sqlText">SQL statement</param>
        /// <param name="parameters">SQL parameters</param>
        /// <returns>a data reader</returns>
        public static SQLiteDataReader ExecuteReader(string connStr, string sqlText, params SQLiteParameter[] parameters)
        {
            // DataReader occupies Connection when reading data
            SQLiteConnection connection = new SQLiteConnection(connStr);// don't release connection for it will be needed later
            SQLiteCommand command = new SQLiteCommand(sqlText, connection);
            connection.Open();
            command.Parameters.AddRange(parameters);
            // CommandBehavior.CloseConnection: incidentally releases Connection when releasing DataReader
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }
        #endregion

        #region executes special command
        /// <summary>
        /// executes special command
        /// </summary>
        /// <param name="connStr">connection string</param>
        /// <param name="sqlText">SQL statement</param>
        /// <param name="tableName">table name</param>
        /// <param name="commandType">SQL command type</param>
        /// <param name="parameters">SQL parameters</param>
        /// <returns>a data table with data</returns>
        public static DataTable ExecuteTable(string connStr, string sqlText, string tableName, CommandType commandType, params SQLiteParameter[] parameters)
        {
            DataTable dt = null;
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlText, connStr))
            {
                dt = new DataTable(tableName);
                adapter.SelectCommand.Parameters.AddRange(parameters);
                adapter.SelectCommand.CommandType = commandType;
                adapter.Fill(dt);
            }
            return dt;
        }

        /// <summary>
        /// executes special command
        /// </summary>
        /// <param name="connStr">connection string</param>
        /// <param name="sqlText">SQL statement</param>
        /// <param name="dataSetName">data set name</param>
        /// <param name="commandType">SQL command type</param>
        /// <param name="parameters">SQL parameters</param>
        /// <returns>a data set with data</returns>
        public static DataSet ExecuteSet(string connStr, string sqlText, string dataSetName, CommandType commandType, params SQLiteParameter[] parameters)
        {
            DataSet ds = null;
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlText, connStr))
            {
                ds = new DataSet(dataSetName);
                adapter.SelectCommand.Parameters.AddRange(parameters);
                adapter.SelectCommand.CommandType = commandType;
                adapter.Fill(ds);
            }
            return ds;
        }
        #endregion

        #region executes transaction and returns the number of rows affected
        /// <summary>
        /// executes transaction
        /// </summary>
        /// <param name="connStr">connection string</param>
        /// <param name="sqlText">SQL statement</param>
        /// <param name="parameters">SQL parameters</param>
        /// <returns>the number of rows affected</returns>
        public static int ExecuteTransaction(string connStr, string sqlText, params SQLiteParameter[] parameters)
        {
            int num = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connStr))
            {
                using (SQLiteCommand command = new SQLiteCommand(sqlText, connection))
                {
                    connection.Open();
                    SQLiteTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        command.Transaction = transaction;
                        command.Parameters.AddRange(parameters);
                        num = command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        num = 0;
                        transaction.Rollback();
                    }
                }
            }
            return num;
        }
        #endregion
    }
}
