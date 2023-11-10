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
    public partial class TableInfoDal
    {
        /// <summary>
        /// gets undeleted table list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="dic">find condition</param>
        /// <returns>table list</returns>
        public List<TableInfo> GetList(string conn, Dictionary<string, string> dic)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"SELECT ti.*,hi.HTitle HallTitle FROM TableInfo ti JOIN HallInfo hi ON ti.THallId=hi.HId WHERE ti.TIsDelete=0 AND hi.HIsDelete=0";
            List<TableInfo> list = new List<TableInfo>();

            // constructs parameters
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();

            #region find condition
            if (dic.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in dic)
                {
                    sqlText += $" AND ti.{item.Key}=@{item.Key}";
                    parameters.Add(new SQLiteParameter($"@{item.Key}", item.Value));
                }
            }
            #endregion

            // reads data
            SQLiteDataReader reader = null;
            try
            {
                reader = SqliteHelper.ExecuteReader(connStr, sqlText, parameters.ToArray());
                while (reader.Read())
                {
                    list.Add(new TableInfo()
                    {
                        TId = Convert.ToInt32(reader["TId"]),
                        TTitle = reader["TTitle"].ToString(),
                        THallId = Convert.ToInt32(reader["THallId"]),
                        TIsFree = Convert.ToBoolean(reader["TIsFree"]),
                        TIsDelete = Convert.ToBoolean(reader["TIsDelete"]),
                        THallTitle = reader["HallTitle"].ToString()
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
        /// inserts table
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ti">table to be inserted</param>
        /// <returns>the number of rows affected</returns>
        public int Insert(string conn, TableInfo ti)
        {
            // constructs SQL statement
            string sqlText = @"INSERT INTO TableInfo(TTitle,THallId,TIsFree,TIsDelete) VALUES(@title,@hallId,@isFree,0)";
            string connStr = SqliteHelper.GetConnectionString(conn);
            // constructs parameters
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@title",ti.TTitle),
                new SQLiteParameter("@hallId",ti.THallId),
                new SQLiteParameter("@isFree",ti.TIsFree)
            };
            // inserts table
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameters);
        }

        /// <summary>
        /// updates table
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ti">table to be updated</param>
        /// <returns>the number of rows affected</returns>
        public int Update(string conn, TableInfo ti)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"UPDATE TableInfo SET TTitle=@title,THallId=@hallId,TIsFree=@isFree WHERE TId=@id";
            // constructs parameters
            SQLiteParameter[] parameters =
            {
               new SQLiteParameter("@title",ti.TTitle),
               new SQLiteParameter("@hallId",ti.THallId),
               new SQLiteParameter("@isFree", ti.TIsFree),
               new SQLiteParameter("@id",ti.TId)
            };
            // updates table
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameters);
        }

        /// <summary>
        /// deletes table according to table id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">table id</param>
        /// <returns>the number of rows affected</returns>
        public int Delete(string conn, params int[] ids)
        {
            // constructs SQL statement and parameters
            string connStr = SqliteHelper.GetConnectionString(conn);
            StringBuilder sb = new StringBuilder();
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            for (int i = 0; i < ids.Length; i++)
            {
                sb.Append($"UPDATE TableInfo SET TIsDelete=1 WHERE TId=@id{i};");
                parameters.Add(new SQLiteParameter($"@id{i}", ids[i]));
            }
            // deletes table
            return SqliteHelper.ExecuteNonQuery(connStr, sb.ToString(), parameters.ToArray());
        }
    }
}
