using _04.CaterModel;
using _05.CaterCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03.CaterDAL
{
    public partial class ManagerInfoDal
    {
        /// <summary>
        /// gets user list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <returns>user list</returns>
        public List<ManagerInfo> GetList(string conn)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"SELECT * FROM ManagerInfo";
            // queries table
            DataTable dt = SqliteHelper.ExecuteTable(connStr, sqlText, "ManagerInfo");
            // saves data to list
            List<ManagerInfo> list = new List<ManagerInfo>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new ManagerInfo()
                {
                    MId = Convert.ToInt32(dr["MId"]),
                    MName = dr["MName"].ToString(),
                    MPwd = dr["MPwd"].ToString(),
                    MType = Convert.ToInt32(dr["MType"])
                }); ;
            }
            return list;
        }

        /// <summary>
        /// inserts user
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="mi">user to be inserted</param>
        /// <returns>the number of rows affected</returns>
        public int Insert(string conn, ManagerInfo mi)
        {
            // constructs SQL statement
            string sqlText = @"INSERT INTO ManagerInfo(MName,MPwd,MType) VALUES(@name,@pwd,@type)";
            string connStr = SqliteHelper.GetConnectionString(conn);
            // constructs parameters
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@name",mi.MName),
                new SQLiteParameter("@pwd",Md5Helper.Encrypt(mi.MPwd)),
                new SQLiteParameter("@type",mi.MType)
            };
            // inserts user
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameters);
        }

        /// <summary>
        /// updates user
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="mi">user to be updated</param>
        /// <returns>the number of rows affected</returns>
        public int Update(string conn, ManagerInfo mi)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText;
            if (mi.MPwd.Equals("旧密码"))
            {
                sqlText = @"UPDATE ManagerInfo SET MName=@name,MType=@type WHERE MId=@id";
            }
            else
            {
                sqlText = @"UPDATE ManagerInfo SET MName=@name,MPwd=@pwd,MType=@type WHERE MId=@id";
            }
            // constructs parameters
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("@name", mi.MName));
            if (!mi.MPwd.Equals("旧密码"))
            {
                parameters.Add(new SQLiteParameter("@pwd", Md5Helper.Encrypt(mi.MPwd)));
            }
            parameters.Add(new SQLiteParameter("@type", mi.MType));
            parameters.Add(new SQLiteParameter("@id", mi.MId));
            // updates user
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameters.ToArray());
        }

        /// <summary>
        /// deletes user according to user id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">user id</param>
        /// <returns>the number of rows affected</returns>
        public int Delete(string conn, params int[] ids)
        {
            // constructs SQL statement and parameters
            string connStr = SqliteHelper.GetConnectionString(conn);
            StringBuilder sb = new StringBuilder();
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            for (int i = 0; i < ids.Length; i++)
            {
                sb.Append($"DELETE FROM ManagerInfo WHERE MId=@id{i};");
                parameters.Add(new SQLiteParameter($"@id{i}", ids[i]));
            }
            // deletes user
            return SqliteHelper.ExecuteNonQuery(connStr, sb.ToString(), parameters.ToArray());
        }

        /// <summary>
        /// finds user by username
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="username">username</param>
        /// <returns>found user or null</returns>
        public ManagerInfo GetUserByName(string conn, string username)
        {
            // defines a mi
            ManagerInfo mi = null;
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"SELECT * FROM ManagerInfo WHERE MName=@username";
            // constructs parameter
            SQLiteParameter p = new SQLiteParameter("@username", username);
            // queries table
            DataTable dt = SqliteHelper.ExecuteTable(connStr, sqlText, "ManagerInfo", p);

            // checks if user has been found
            if (dt.Rows.Count > 0)
            {
                // user exists
                mi = new ManagerInfo()
                {
                    MId = Convert.ToInt32(dt.Rows[0]["MId"]),
                    MName = dt.Rows[0]["MName"].ToString(),
                    MPwd = dt.Rows[0]["MPwd"].ToString(),
                    MType = Convert.ToInt32(dt.Rows[0]["MType"])
                };
            }

            return mi;
        }
    }
}
