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
    public partial class OrderInfoDal
    {
        /// <summary>
        /// inserts order
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="tableId">table id</param>
        /// <returns>latest order id</returns>
        public int Insert(string conn, int tableId)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"INSERT INTO OrderInfo(ODate,IsPay,TableId) VALUES(DATETIME('NOW','LOCALTIME'),0,@tableId);" +
                "UPDATE TableInfo SET TIsFree=0 WHERE TId=@id;" +
                "SELECT OId FROM OrderInfo ORDER BY OId DESC LIMIT 0,1;";
            // constructs parameters
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@tableId", tableId),
                new SQLiteParameter("@id", tableId)
            };
            // inserts order
            return Convert.ToInt32(SqliteHelper.ExecuteScalar(connStr, sqlText, parameters));
        }

        /// <summary>
        /// inserts or updates order detail
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="orderId">order id</param>
        /// <param name="dishId">dish id</param>
        /// <returns>the number of rows affected</returns>
        public int InsertOrUpdate(string conn, int orderId, int dishId)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"SELECT COUNT(*) FROM OrderDetailInfo WHERE OrderId=@orderId AND DishId=@dishId";
            // constructs parameters
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@orderId", orderId),
                new SQLiteParameter("@dishId", dishId)
            };
            // duplicated dish check
            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(connStr, sqlText, parameters));
            if (count > 0)
            {
                #region If yes, count+1
                sqlText = @"UPDATE OrderDetailInfo SET Count=Count+1 WHERE OrderId=@orderId AND DishId=@dishId";
                #endregion
            }
            else
            {
                #region otherwise inserts new record
                sqlText = @"INSERT INTO OrderDetailInfo(OrderId,DishId,Count) VALUES(@orderId,@dishId,1)";
                #endregion
            }
            // inserts or updates order
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameters);
        }

        /// <summary>
        /// updates count by order id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="orderId">order id</param>
        /// <param name="count">dish count</param>
        /// <returns>the number of rows affected</returns>
        public int UpdateCountByOId(string conn, int orderId, int count)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"UPDATE OrderDetailInfo SET Count=@count WHERE OId=@id";
            // constructs SQL parameters
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@count",count),
                new SQLiteParameter("@id", orderId)
            };
            // updates count
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameters);
        }

        /// <summary>
        /// updates order
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="orderId">order id</param>
        /// <param name="money">payable account</param>
        /// <returns>the number of rows affected</returns>
        public int UpdateOrderMoney(string conn, int orderId, decimal money)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = "UPDATE OrderInfo SET OMoney=@money WHERE OId=@id";
            // constructs SQL parameters
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@money",money),
                new SQLiteParameter("@id", orderId)
            };
            // updates order
            return SqliteHelper.ExecuteNonQuery(connStr, sqlText, parameters);
        }

        /// <summary>
        /// updates multiple tables
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="isUseBalance">whether to use member balance</param>
        /// <param name="payMoney">money for payment</param>
        /// <param name="memberId">member id</param>
        /// <param name="orderId">order id</param>
        /// <param name="discount">member discount</param>
        /// <returns>the number of rows affected</returns>
        public int UpdateTables(string conn, bool isUseBalance, decimal payMoney, int memberId, int orderId, decimal discount)
        {
            string connStr = SqliteHelper.GetConnectionString(conn);
            StringBuilder sb = new StringBuilder();
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            #region pays as member
            if (isUseBalance)
            {
                #region uses member balance

                #region deducts money
                sb.Append(@"UPDATE MemberInfo SET MMoney=MMoney-@pay WHERE MId=@memberId;");
                parameters.Add(new SQLiteParameter("@pay", payMoney));
                parameters.Add(new SQLiteParameter("@memberId", memberId));
                #endregion

                #region updates order
                sb.Append(@"UPDATE OrderInfo SET IsPay=1,MemberId=@memberId,Discount=@discount WHERE OId=@orderId;");
                parameters.Add(new SQLiteParameter("@discount", discount));
                parameters.Add(new SQLiteParameter("@orderId", orderId));
                #endregion

                #region updates table
                sb.Append(@"UPDATE TableInfo SET TIsFree=1 WHERE TId=(SELECT TableId FROM OrderInfo WHERE OId=@orderId);");
                #endregion

                #endregion
            }
            else
            {
                #region doesn't use member balance

                #region updates order
                sb.Append("UPDATE OrderInfo SET IsPay=1,MemberId=@memberId WHERE OId=@orderId;");
                parameters.Add(new SQLiteParameter("@memberId", memberId));
                parameters.Add(new SQLiteParameter("@orderId", orderId));
                #endregion

                #region updates table
                sb.Append("UPDATE TableInfo SET TIsFree=1 WHERE TId=(SELECT TableId FROM OrderInfo WHERE OId=@orderId);");
                #endregion

                #endregion
            }
            #endregion
            return SqliteHelper.ExecuteNonQuery(connStr, sb.ToString(), parameters.ToArray());
        }

        /// <summary>
        /// updates multiple tables
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="orderId">order id</param>
        /// <returns>the number of rows affected</returns>
        public int UpdateTables(string conn, int orderId)
        {
            string connStr = SqliteHelper.GetConnectionString(conn);
            StringBuilder sb = new StringBuilder();
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            #region pays as not member

            #region updates order
            sb.Append("UPDATE OrderInfo SET IsPay=1 WHERE OId=@orderId;");
            parameters.Add(new SQLiteParameter("@orderId", orderId));
            #endregion

            #region updates table
            sb.Append("UPDATE TableInfo SET TIsFree=1 WHERE TId=(SELECT TableId FROM OrderInfo WHERE OId=@orderId);");
            #endregion

            #endregion
            return SqliteHelper.ExecuteNonQuery(connStr, sb.ToString(), parameters.ToArray());
        }

        /// <summary>
        /// deletes detail according to detail id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">order detail id</param>
        /// <returns>the number of rows affected</returns>
        public int DeleteDetailById(string conn, params int[] ids)
        {
            // constructs SQL statement and parameters
            string connStr = SqliteHelper.GetConnectionString(conn);
            StringBuilder sb = new StringBuilder();
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            for (int i = 0; i < ids.Length; i++)
            {
                sb.Append($"DELETE FROM OrderDetailInfo WHERE OId=@id{i};");
                parameters.Add(new SQLiteParameter($"@id{i}", ids[i]));
            }
            // deletes detail
            return SqliteHelper.ExecuteNonQuery(connStr, sb.ToString(), parameters.ToArray());
        }

        /// <summary>
        /// gets order detail list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="orderId">order id</param>
        /// <returns>order detail list</returns>
        public List<OrderDetailInfo> GetDetailList(string conn, int orderId)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"SELECT odi.*,di.DTitle Title,di.DPrice Price FROM OrderDetailInfo odi JOIN DishInfo di ON odi.DishId=di.DId WHERE di.DIsDelete=0 AND odi.OrderId=@orderId";
            // constructs SQL parameter
            SQLiteParameter parameter = new SQLiteParameter("@orderId", orderId);
            // reads data
            SQLiteDataReader reader = null;
            List<OrderDetailInfo> list = new List<OrderDetailInfo>();
            try
            {
                reader = SqliteHelper.ExecuteReader(connStr, sqlText, parameter);
                while (reader.Read())
                {
                    list.Add(new OrderDetailInfo()
                    {
                        OId = Convert.ToInt32(reader["OId"]),
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        DishId = Convert.ToInt32(reader["DishId"]),
                        Count = Convert.ToInt32(reader["Count"]),
                        DTitle = reader["Title"].ToString(),
                        DPrice = Convert.ToDecimal(reader["Price"]),
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
        /// gets order id by table id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="tableId">table id</param>
        /// <returns>order id</returns>
        public int GetOrderIdByTableId(string conn, int tableId)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"SELECT OId FROM OrderInfo WHERE TableId=@tableId AND IsPay=0";
            // constructs SQL parameter
            SQLiteParameter parameter = new SQLiteParameter("@tableId", tableId);
            // returns order id
            return Convert.ToInt32(SqliteHelper.ExecuteScalar(connStr, sqlText, parameter));
        }

        /// <summary>
        /// gets table id by order id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="orderId">order id</param>
        /// <returns>table id</returns>
        public int GetTableIdByOrderId(string conn, int orderId)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"SELECT TableId FROM OrderInfo WHERE OId=@orderId AND IsPay=0";
            // constructs SQL parameter
            SQLiteParameter parameter = new SQLiteParameter("@orderId", orderId);
            // returns table id
            return Convert.ToInt32(SqliteHelper.ExecuteScalar(connStr, sqlText, parameter));
        }

        /// <summary>
        /// gets total price by order id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="orderId">order id</param>
        /// <returns>total price</returns>
        public decimal GetTotalPriceByOrderId(string conn, int orderId)
        {
            // constructs SQL statement
            string connStr = SqliteHelper.GetConnectionString(conn);
            string sqlText = @"SELECT SUM(di.DPrice*odi.Count) FROM OrderDetailInfo odi JOIN DishInfo di ON odi.DishId=di.DId WHERE odi.OrderId=@orderId";
            // constructs SQL parameter
            SQLiteParameter parameter = new SQLiteParameter("@orderId", orderId);
            // returns total price
            object obj = SqliteHelper.ExecuteScalar(connStr, sqlText, parameter);
            return obj == DBNull.Value ? 0 : Convert.ToDecimal(obj);
        }
    }
}
