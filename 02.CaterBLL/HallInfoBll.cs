using _03.CaterDAL;
using _04.CaterModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.CaterBLL
{
    public class HallInfoBll
    {
        private HallInfoDal hiDal = new HallInfoDal();// creates database access layer object

        /// <summary>
        /// gets undeleted hall list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <returns>hall list</returns>
        public List<HallInfo> GetList(string conn)
        {
            // calls method to query hall list
            return hiDal.GetList(conn);
        }

        /// <summary>
        /// adds hall
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="hi">hall to be added</param>
        /// <returns>whether successfully added</returns>
        public bool Add(string conn, HallInfo hi)
        {
            // calls method to complete insert
            return hiDal.Insert(conn, hi) > 0;
        }

        /// <summary>
        /// edits hall
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="hi">hall to be edited</param>
        /// <returns>whether successfully edited</returns>
        public bool Edit(string conn, HallInfo hi)
        {
            // calls method to complete update
            return hiDal.Update(conn, hi) > 0;
        }

        /// <summary>
        /// removes hall according to hall id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">hall id</param>
        /// <returns>whether successfully removed</returns>
        public bool Remove(string conn, params int[] ids)
        {
            // calls method to complete delete
            return hiDal.Delete(conn, ids) > 0;
        }
    }
}
