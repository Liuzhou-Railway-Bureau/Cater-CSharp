using _03.CaterDAL;
using _04.CaterModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace _02.CaterBLL
{
    public class OrderInfoBll
    {
        private OrderInfoDal oiDal = new OrderInfoDal();// creates database access layer object

        /// <summary>
        /// places order
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="tableId">table id</param>
        /// <returns>order id</returns>
        public int PlaceOrder(string conn, int tableId)
        {
            // calls method to complete insert
            return oiDal.Insert(conn, tableId);
        }

        /// <summary>
        /// orders dish
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="orderId">order id</param>
        /// <param name="dishId">dish id</param>
        /// <returns>whether successfully ordered</returns>
        public bool OrderDish(string conn, int orderId, int dishId)
        {
            // calls method to complete insert
            return oiDal.InsertOrUpdate(conn, orderId, dishId) > 0;
        }

        /// <summary>
        /// gets order detail list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="orderId">order id</param>
        /// <returns>order detail list</returns>
        public List<OrderDetailInfo> GetDetailList(string conn, int orderId)
        {
            // calls method to query order detail list
            return oiDal.GetDetailList(conn, orderId);
        }

        /// <summary>
        /// gets order id by table id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="tableId">table id</param>
        /// <returns>order id</returns>
        public int GetOrderId(string conn, int tableId)
        {
            // calls method to query order id
            return oiDal.GetOrderIdByTableId(conn, tableId);
        }

        /// <summary>
        /// gets table id by order id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="orderId">order id</param>
        /// <returns>table id</returns>
        public int GetTableId(string conn, int orderId)
        {
            // calls method to query table id
            return oiDal.GetTableIdByOrderId(conn, orderId);
        }

        /// <summary>
        /// sets count
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="orderId">order id</param>
        /// <param name="count">dish count</param>
        /// <returns>whether successfully set</returns>
        public bool SetCount(string conn, int orderId, int count)
        {
            // calls method to complete update
            return oiDal.UpdateCountByOId(conn, orderId, count) > 0;
        }

        /// <summary>
        /// gets total price
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="orderId">order id</param>
        /// <returns>total price</returns>
        public decimal GetTotalPrice(string conn, int orderId)
        {
            // calls method to query total price
            return oiDal.GetTotalPriceByOrderId(conn, orderId);
        }

        /// <summary>
        /// places order
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="orderId">order id</param>
        /// <param name="money">payable account</param>
        /// <returns>whether successfully placed</returns>
        public bool PlaceOrder(string conn, int orderId, decimal money)
        {
            // calls method to complete update
            return oiDal.UpdateOrderMoney(conn, orderId, money) > 0;
        }

        /// <summary>
        /// removes detail according to detail id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">order detail id</param>
        /// <returns>whether successfully removed</returns>
        public bool RemoveDetail(string conn, params int[] ids)
        {
            // calls method to complete delete
            return oiDal.DeleteDetailById(conn, ids) > 0;
        }

        /// <summary>
        /// pays for bill as member
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="isUseBalance">whether to use member balance</param>
        /// <param name="payMoney">money for payment</param>
        /// <param name="memberId">member id</param>
        /// <param name="orderId">order id</param>
        /// <param name="discount">member discount</param>
        /// <returns>whether successfully paid</returns>
        public bool PayForBill(string conn, bool isUseBalance, decimal payMoney, int memberId, int orderId, decimal discount)
        {
            // calls method to complete updates
            return oiDal.UpdateTables(conn, isUseBalance, payMoney, memberId, orderId, discount) > 0;
        }

        /// <summary>
        /// pays for bill as not member
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="orderId">order id</param>
        /// <returns>whether successfully paid</returns>
        public bool PayForBill(string conn, int orderId)
        {
            // calls method to complete updates
            return oiDal.UpdateTables(conn, orderId) > 0;
        }
    }
}
