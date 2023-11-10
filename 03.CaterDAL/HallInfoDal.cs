using _04.CaterModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03.CaterDAL
{
    public partial class HallInfoDal
    {
        /// <summary>
        /// gets undeleted hall list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <returns>hall list</returns>
        public List<HallInfo> GetList(string conn)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"SELECT * FROM HallInfo WHERE HIsDelete=0";
            List<HallInfo> list = new List<HallInfo>();
            // reads data
            SQLiteDataReader reader = null;
            try
            {
                reader = SqliteHelper.ExecuteReader(connStr, sqlText);
                while (reader.Read())
                {
                    list.Add(new HallInfo()
                    {
                        HId = Convert.ToInt32(reader["HId"]),
                        HTitle = reader["HTitle"].ToString(),
                        HIsDelete = Convert.ToBoolean(reader["HIsDelete"])
                    });
                }
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        /// <summary>
        /// inserts hall
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="hi">hall to be inserted</param>
        /// <returns>the number of rows affected</returns>
        public int Insert(string conn, HallInfo hi)
        {
            // constructs SQL statement
            string sqlText = @"INSERT INTO HallInfo(HTitle,HIsDelete) VALUES(@title,0)";
            string connStr = SqliteHelper.GetConnectionString(conn);
            // constructs parameter
            SQLiteParameter parameter = new SQLiteParameter("@title", hi.HTitle);
            // inserts hall
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameter);
        }

        /// <summary>
        /// updates hall
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="hi">hall to be updated</param>
        /// <returns>the number of rows affected</returns>
        public int Update(string conn, HallInfo hi)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"UPDATE HallInfo SET HTitle=@title WHERE HId=@id";
            // constructs parameters
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@title",hi.HTitle),
                new SQLiteParameter("@id",hi.HId)
            };
            // updates hall
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameters);
        }

        /// <summary>
        /// deletes hall according to hall id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">hall id</param>
        /// <returns>the number of rows affected</returns>
        public int Delete(string conn, params int[] ids)
        {
            // constructs SQL statement and parameters
            string connStr = SqliteHelper.GetConnectionString(conn);
            StringBuilder sb = new StringBuilder();
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            for (int i = 0; i < ids.Length; i++)
            {
                sb.Append($"UPDATE HallInfo SET HIsDelete=1 WHERE HId=@id{i};");
                parameters.Add(new SQLiteParameter($"@id{i}", ids[i]));
            }
            // deletes hall
            return SqliteHelper.ExecuteNonQuery(connStr, sb.ToString(), parameters.ToArray());
        }
    }
}
