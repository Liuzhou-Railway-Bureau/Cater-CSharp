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
    public partial class DishTypeInfoDal
    {
        /// <summary>
        /// gets undeleted dish type list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <returns>dish type list</returns>
        public List<DishTypeInfo> GetList(string conn)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"SELECT * FROM DishTypeInfo WHERE DIsDelete=0";
            List<DishTypeInfo> list = new List<DishTypeInfo>();
            // reads data
            SQLiteDataReader reader = null;
            try
            {
                reader = SqliteHelper.ExecuteReader(connStr, sqlText);
                while (reader.Read())
                {
                    list.Add(new DishTypeInfo()
                    {
                        DId = Convert.ToInt32(reader["DId"]),
                        DTitle = reader["DTitle"].ToString(),
                        DIsDelete = Convert.ToBoolean(reader["DIsDelete"])
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
        /// inserts dish type
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="dti">dish type to be inserted</param>
        /// <returns>the number of rows affected</returns>
        public int Insert(string conn, DishTypeInfo dti)
        {
            // constructs SQL statement
            string sqlText = @"INSERT INTO DishTypeInfo(DTitle,DIsDelete) VALUES(@title,0)";
            string connStr = SqliteHelper.GetConnectionString(conn);
            // constructs parameter
            SQLiteParameter parameter = new SQLiteParameter("@title", dti.DTitle);
            // inserts dish type
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameter);
        }

        /// <summary>
        /// updates dish type
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="dti">dish type to be updated</param>
        /// <returns>the number of rows affected</returns>
        public int Update(string conn, DishTypeInfo dti)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"UPDATE DishTypeInfo SET DTitle=@title WHERE DId=@id";
            // constructs parameters
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@title",dti.DTitle),
                new SQLiteParameter("@id",dti.DId)
            };
            // updates dish type
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameters);
        }

        /// <summary>
        /// deletes dish type according to dish type id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">dish type id</param>
        /// <returns>the number of rows affected</returns>
        public int Delete(string conn, params int[] ids)
        {
            // constructs SQL statement and parameters
            string connStr = SqliteHelper.GetConnectionString(conn);
            StringBuilder sb = new StringBuilder();
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            for (int i = 0; i < ids.Length; i++)
            {
                sb.Append($"UPDATE DishTypeInfo SET DIsDelete=1 WHERE DId=@id{i};");
                parameters.Add(new SQLiteParameter($"@id{i}", ids[i]));
            }
            // deletes dish type
            return SqliteHelper.ExecuteNonQuery(connStr, sb.ToString(), parameters.ToArray());
        }
    }
}
