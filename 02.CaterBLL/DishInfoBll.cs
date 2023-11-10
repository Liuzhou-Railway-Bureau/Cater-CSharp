using _03.CaterDAL;
using _04.CaterModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.CaterBLL
{
    public class DishInfoBll
    {
        private DishInfoDal diDal = new DishInfoDal();// creates database access layer object

        /// <summary>
        /// gets undeleted dish list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="dic">find condition</param>
        /// <returns>dish list</returns>
        public List<DishInfo> GetList(string conn, Dictionary<string, string> dic)
        {
            // calls method to query dish list
            return diDal.GetList(conn, dic);
        }

        /// <summary>
        /// adds dish
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="di">dish to be added</param>
        /// <returns>whether successfully added</returns>
        public bool Add(string conn, DishInfo di)
        {
            // calls method to complete insert
            return diDal.Insert(conn, di) > 0;
        }

        /// <summary>
        /// edits dish
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="di">dish to be edited</param>
        /// <returns>whether successfully edited</returns>
        public bool Edit(string conn, DishInfo di)
        {
            // calls method to complete update
            return diDal.Update(conn, di) > 0;
        }

        /// <summary>
        /// removes dish according to dish id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">dish id</param>
        /// <returns>whether successfully removed</returns>
        public bool Remove(string conn, params int[] ids)
        {
            // calls method to complete delete
            return diDal.Delete(conn, ids) > 0;
        }
    }
}
