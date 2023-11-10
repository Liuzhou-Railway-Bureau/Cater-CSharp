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
    public partial class MemberInfoDal
    {
        /// <summary>
        /// gets undeleted member list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="dic">find condition</param>
        /// <returns>member list</returns>
        public List<MemberInfo> GetList(string conn, Dictionary<string, string> dic)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"SELECT mi.*,mti.MTitle TypeTitle,mti.MDiscount Discount FROM MemberInfo mi JOIN MemberTypeInfo mti ON mi.MTypeId=mti.MId WHERE mi.MIsDelete=0 AND mti.MIsDelete=0";
            List<MemberInfo> list = new List<MemberInfo>();

            #region find condition
            if (dic.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in dic)
                {
                    sqlText += $" AND mi.{item.Key} LIKE '%{item.Value}%'";
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
                    list.Add(new MemberInfo()
                    {
                        MId = Convert.ToInt32(reader["MId"]),
                        MTypeId = Convert.ToInt32(reader["MTypeId"]),
                        MName = reader["MName"].ToString(),
                        MPhone = reader["MPhone"].ToString(),
                        MMoney = Convert.ToDecimal(reader["MMoney"]),
                        MIsDelete = Convert.ToBoolean(reader["MIsDelete"]),
                        MTypeTitle = reader["TypeTitle"].ToString(),
                        MDiscount = Convert.ToDecimal(reader["Discount"])
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
        /// inserts member
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="mi">member to be inserted</param>
        /// <returns>the number of rows affected</returns>
        public int Insert(string conn, MemberInfo mi)
        {
            // constructs SQL statement
            string sqlText = @"INSERT INTO MemberInfo(MName,MTypeId,MPhone,MMoney,MIsDelete) VALUES(@name,@typeId,@phone,@money,0)";
            string connStr = SqliteHelper.GetConnectionString(conn);
            // constructs parameters
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@name",mi.MName),
                new SQLiteParameter("@typeId",mi.MTypeId),
                new SQLiteParameter("@phone",mi.MPhone),
                new SQLiteParameter("@money",mi.MMoney)
            };
            // inserts member
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameters);
        }

        /// <summary>
        /// updates member
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="mi">member to be updated</param>
        /// <returns>the number of rows affected</returns>
        public int Update(string conn, MemberInfo mi)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"UPDATE MemberInfo SET MName=@name,MTypeId=@typeId,MPhone=@phone,MMoney=@money WHERE MId=@id";
            // constructs parameters
            SQLiteParameter[] parameters =
            {
               new SQLiteParameter("@name",mi.MName),
               new SQLiteParameter("@typeId",mi.MTypeId),
               new SQLiteParameter("@phone",mi.MPhone),
               new SQLiteParameter("@money",mi.MMoney),
               new SQLiteParameter("@id",mi.MId)
            };
            // updates member
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameters);
        }

        /// <summary>
        /// deletes member according to member id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">member id</param>
        /// <returns>the number of rows affected</returns>
        public int Delete(string conn, params int[] ids)
        {
            // constructs SQL statement and parameters
            string connStr = SqliteHelper.GetConnectionString(conn);
            StringBuilder sb = new StringBuilder();
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            for (int i = 0; i < ids.Length; i++)
            {
                sb.Append($"UPDATE MemberInfo SET MIsDelete=1 WHERE MId=@id{i};");
                parameters.Add(new SQLiteParameter($"@id{i}", ids[i]));
            }
            // deletes member
            return SqliteHelper.ExecuteNonQuery(connStr, sb.ToString(), parameters.ToArray());
        }
    }
}
