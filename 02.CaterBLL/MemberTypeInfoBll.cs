using _03.CaterDAL;
using _04.CaterModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.CaterBLL
{
    public class MemberTypeInfoBll
    {
        private MemberTypeInfoDal mtiDal = new MemberTypeInfoDal();// creates database access layer object

        /// <summary>
        /// gets undeleted member type list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <returns>member type list</returns>
        public List<MemberTypeInfo> GetList(string conn)
        {
            // calls method to query member type list
            return mtiDal.GetList(conn);
        }

        /// <summary>
        /// adds member type
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="mti">member type to be added</param>
        /// <returns>whether successfully added</returns>
        public bool Add(string conn, MemberTypeInfo mti)
        {
            // calls method to complete insert
            return mtiDal.Insert(conn, mti) > 0;
        }

        /// <summary>
        /// edits member type
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="mti">member type to be edited</param>
        /// <returns>whether successfully edited</returns>
        public bool Edit(string conn, MemberTypeInfo mti)
        {
            // calls method to complete update
            return mtiDal.Update(conn, mti) > 0;
        }

        /// <summary>
        /// removes member type according to member type id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">member type id</param>
        /// <returns>whether successfully removed</returns>
        public bool Remove(string conn, params int[] ids)
        {
            // calls method to complete delete
            return mtiDal.Delete(conn, ids) > 0;
        }
    }
}
