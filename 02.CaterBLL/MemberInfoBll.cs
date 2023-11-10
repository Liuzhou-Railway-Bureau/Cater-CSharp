using _03.CaterDAL;
using _04.CaterModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.CaterBLL
{
    public class MemberInfoBll
    {
        private MemberInfoDal miDal = new MemberInfoDal();// creates database access layer object

        /// <summary>
        /// gets undeleted member list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="dic">find condition</param>
        /// <returns>member list</returns>
        public List<MemberInfo> GetList(string conn, Dictionary<string, string> dic)
        {
            // calls method to query member list
            return miDal.GetList(conn, dic);
        }

        /// <summary>
        /// adds member
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="mi">member to be added</param>
        /// <returns>whether successfully added</returns>
        public bool Add(string conn, MemberInfo mi)
        {
            // calls method to complete insert
            return miDal.Insert(conn, mi) > 0;
        }

        /// <summary>
        /// edits member
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="mi">member to be edited</param>
        /// <returns>whether successfully edited</returns>
        public bool Edit(string conn, MemberInfo mi)
        {
            // calls method to complete update
            return miDal.Update(conn, mi) > 0;
        }

        /// <summary>
        /// removes member according to member id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">member id</param>
        /// <returns>whether successfully removed</returns>
        public bool Remove(string conn, params int[] ids)
        {
            // calls method to complete delete
            return miDal.Delete(conn, ids) > 0;
        }
    }
}
