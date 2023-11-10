using _03.CaterDAL;
using _04.CaterModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.CaterBLL
{
    public class TableInfoBll
    {
        private TableInfoDal tiDal = new TableInfoDal();// creates database access layer object

        /// <summary>
        /// gets undeleted table list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="dic">find condition</param>
        /// <returns>table list</returns>
        public List<TableInfo> GetList(string conn, Dictionary<string, string> dic)
        {
            // calls method to query table list
            return tiDal.GetList(conn, dic);
        }

        /// <summary>
        /// adds table
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ti">table to be added</param>
        /// <returns>whether successfully added</returns>
        public bool Add(string conn, TableInfo ti)
        {
            // calls method to complete insert
            return tiDal.Insert(conn, ti) > 0;
        }

        /// <summary>
        /// edits table
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ti">table to be edited</param>
        /// <returns>whether successfully edited</returns>
        public bool Edit(string conn, TableInfo ti)
        {
            // calls method to complete update
            return tiDal.Update(conn, ti) > 0;
        }

        /// <summary>
        /// removes table according to table id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">table id</param>
        /// <returns>whether successfully removed</returns>
        public bool Remove(string conn, params int[] ids)
        {
            // calls method to complete delete
            return tiDal.Delete(conn, ids) > 0;
        }
    }
}
