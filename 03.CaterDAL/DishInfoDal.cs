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
    public partial class DishInfoDal
    {
        /// <summary>
        /// gets undeleted dish list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="dic">find condition</param>
        /// <returns>dish list</returns>
        public List<DishInfo> GetList(string conn, Dictionary<string, string> dic)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"SELECT di.*,dti.DTitle TypeTitle FROM DishInfo di JOIN DishTypeInfo dti ON di.DTypeId=dti.DId WHERE di.DIsDelete=0 AND dti.DIsDelete=0";
            List<DishInfo> list = new List<DishInfo>();

            #region find condition
            if (dic.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in dic)
                {
                    sqlText += $" AND di.{item.Key} LIKE '%{item.Value}%'";
                }
            }
            #endregion

            // reads data
            SQLiteDataReader reader = null;
            try
            {
                reader = SqliteHelper.ExecuteReader(connStr, sqlText);
                while (reader.Read())
                {
                    list.Add(new DishInfo()
                    {
                        DId = Convert.ToInt32(reader["DId"]),
                        DTitle = reader["DTitle"].ToString(),
                        DTypeId = Convert.ToInt32(reader["DTypeId"]),
                        DPrice = Convert.ToDecimal(reader["DPrice"]),
                        DChar = reader["DChar"].ToString(),
                        DIsDelete = Convert.ToBoolean(reader["DIsDelete"]),
                        DTypeTitle = reader["TypeTitle"].ToString()
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
        /// inserts dish
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="di">dish to be inserted</param>
        /// <returns>the number of rows affected</returns>
        public int Insert(string conn, DishInfo di)
        {
            // constructs SQL statement
            string sqlText = @"INSERT INTO DishInfo(DTitle,DTypeId,DPrice,DChar,DIsDelete) VALUES(@title,@typeId,@price,@char,0)";
            string connStr = SqliteHelper.GetConnectionString(conn);
            // constructs parameters
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@title",di.DTitle),
                new SQLiteParameter("@typeId",di.DTypeId),
                new SQLiteParameter("@price",di.DPrice),
                new SQLiteParameter("@char",di.DChar)
            };
            // inserts dish
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameters);
        }

        /// <summary>
        /// updates dish
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="di">dish to be updated</param>
        /// <returns>the number of rows affected</returns>
        public int Update(string conn, DishInfo di)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"UPDATE DishInfo SET DTitle=@title,DTypeId=@typeId,DPrice=@price,DChar=@char WHERE DId=@id";
            // constructs parameters
            SQLiteParameter[] parameters =
            {
               new SQLiteParameter("@title",di.DTitle),
               new SQLiteParameter("@typeId",di.DTypeId),
               new SQLiteParameter("@price",di.DPrice),
               new SQLiteParameter("@char",di.DChar),
               new SQLiteParameter("@id",di.DId)
            };
            // updates dish
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameters);
        }

        /// <summary>
        /// deletes dish according to dish id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">dish id</param>
        /// <returns>the number of rows affected</returns>
        public int Delete(string conn, params int[] ids)
        {
            // constructs SQL statement and parameters
            string connStr = SqliteHelper.GetConnectionString(conn);
            StringBuilder sb = new StringBuilder();
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            for (int i = 0; i < ids.Length; i++)
            {
                sb.Append($"UPDATE DishInfo SET DIsDelete=1 WHERE DId=@id{i};");
                parameters.Add(new SQLiteParameter($"@id{i}", ids[i]));
            }
            // deletes dish
            return SqliteHelper.ExecuteNonQuery(connStr, sb.ToString(), parameters.ToArray());
        }
    }
}
