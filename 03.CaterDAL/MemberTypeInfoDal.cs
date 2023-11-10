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
    public partial class MemberTypeInfoDal
    {
        /// <summary>
        /// gets undeleted member type list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <returns>member type list</returns>
        public List<MemberTypeInfo> GetList(string conn)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"SELECT * FROM MemberTypeInfo WHERE MIsDelete=0";
            List<MemberTypeInfo> list = new List<MemberTypeInfo>();
            // reads data
            SQLiteDataReader reader = null;
            try
            {
                reader = SqliteHelper.ExecuteReader(connStr, sqlText);
                while (reader.Read())
                {
                    list.Add(new MemberTypeInfo()
                    {
                        MId = Convert.ToInt32(reader["MId"]),
                        MTitle = reader["MTitle"].ToString(),
                        MDiscount = Convert.ToDecimal(reader["MDiscount"]),
                        MIsDelete = Convert.ToBoolean(reader["MIsDelete"])
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
        /// inserts member type
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="mti">member type to be inserted</param>
        /// <returns>the number of rows affected</returns>
        public int Insert(string conn, MemberTypeInfo mti)
        {
            // constructs SQL statement
            string sqlText = @"INSERT INTO MemberTypeInfo(MTitle,MDiscount,MIsDelete) VALUES(@title,@discount,0)";
            string connStr = SqliteHelper.GetConnectionString(conn);
            // constructs parameters
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@title",mti.MTitle),
                new SQLiteParameter("@discount",mti.MDiscount),
            };
            // inserts member type
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameters);
        }

        /// <summary>
        /// updates member type
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="mti">member type to be updated</param>
        /// <returns>the number of rows affected</returns>
        public int Update(string conn, MemberTypeInfo mti)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"UPDATE MemberTypeInfo SET MTitle=@title,MDiscount=@discount WHERE MId=@id";
            // constructs parameters
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@title",mti.MTitle),
                new SQLiteParameter("@discount",mti.MDiscount),
                new SQLiteParameter("@id",mti.MId)
            };
            // updates member type
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameters);
        }

        /// <summary>
        /// deletes member type according to member type id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">member type id</param>
        /// <returns>the number of rows affected</returns>
        public int Delete(string conn, params int[] ids)
        {
            // constructs SQL statement and parameters
            string connStr = SqliteHelper.GetConnectionString(conn);
            StringBuilder sb = new StringBuilder();
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            for (int i = 0; i < ids.Length; i++)
            {
                sb.Append($"UPDATE MemberTypeInfo SET MIsDelete=1 WHERE MId=@id{i};");
                parameters.Add(new SQLiteParameter($"@id{i}", ids[i]));
            }
            // deletes member type
            return SqliteHelper.ExecuteNonQuery(connStr, sb.ToString(), parameters.ToArray());
        }
    }
}
