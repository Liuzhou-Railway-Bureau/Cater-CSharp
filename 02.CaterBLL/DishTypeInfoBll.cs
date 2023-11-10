using _03.CaterDAL;
using _04.CaterModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.CaterBLL
{
    public class DishTypeInfoBll
    {
        private DishTypeInfoDal dtiDal = new DishTypeInfoDal();// creates database access layer object

        /// <summary>
        /// gets undeleted dish type list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <returns>dish type list</returns>
        public List<DishTypeInfo> GetList(string conn)
        {
            // calls method to query dish type list
            return dtiDal.GetList(conn);
        }

        /// <summary>
        /// adds dish type
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="dti">dish type to be added</param>
        /// <returns>whether successfully added</returns>
        public bool Add(string conn, DishTypeInfo dti)
        {
            // calls method to complete insert
            return dtiDal.Insert(conn, dti) > 0;
        }

        /// <summary>
        /// edits dish type
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="dti">dish type to be edited</param>
        /// <returns>whether successfully edited</returns>
        public bool Edit(string conn, DishTypeInfo dti)
        {
            // calls method to complete update
            return dtiDal.Update(conn, dti) > 0;
        }

        /// <summary>
        /// removes dish type according to dish type id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">dish type id</param>
        /// <returns>whether successfully removed</returns>
        public bool Remove(string conn, params int[] ids)
        {
            // calls method to complete delete
            return dtiDal.Delete(conn, ids) > 0;
        }
    }
}
