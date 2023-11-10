using _03.CaterDAL;
using _04.CaterModel;
using _05.CaterCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.CaterBLL
{
    public class ManagerInfoBll
    {
        private ManagerInfoDal miDal = new ManagerInfoDal();// creates database access layer object

        /// <summary>
        /// gets user list
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <returns>user list</returns>
        public List<ManagerInfo> GetList(string conn)
        {
            // calls method to query user list
            return miDal.GetList(conn);
        }

        /// <summary>
        /// adds user
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="mi">user to be added</param>
        /// <returns>whether successfully added</returns>
        public bool Add(string conn, ManagerInfo mi)
        {
            // calls method to complete insert
            return miDal.Insert(conn, mi) > 0;
        }

        /// <summary>
        /// edits user
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="mi">user to be edited</param>
        /// <returns>whether successfully edited</returns>
        public bool Edit(string conn, ManagerInfo mi)
        {
            // calls method to complete update
            return miDal.Update(conn, mi) > 0;
        }

        /// <summary>
        /// removes user according to user id
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="ids">user id</param>
        /// <returns>whether successfully removed</returns>
        public bool Remove(string conn, params int[] ids)
        {
            // calls method to complete delete
            return miDal.Delete(conn, ids) > 0;
        }

        /// <summary>
        /// logs in system
        /// </summary>
        /// <param name="conn">connection name</param>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <param name="type">employee type</param>
        /// <returns>login state</returns>
        public LoginState Login(string conn, string username, string password, out int type)
        {
            // calls method to query user by username
            ManagerInfo mi = miDal.GetUserByName(conn, username);
            type = -1;
            // judges login state
            LoginState state;
            if (mi == null)
            {
                state = LoginState.NameError;
            }
            else
            {
                if (mi.MPwd.Equals(Md5Helper.Encrypt(password)))
                {
                    state = LoginState.OK;
                    type = mi.MType;
                }
                else
                {
                    state = LoginState.PasswordError;
                }
            }
            return state;
        }
    }
}
