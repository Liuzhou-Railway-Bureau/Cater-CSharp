using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _05.CaterCommon
{
    public partial class Md5Helper
    {
        /// <summary>
        /// encrypts string
        /// </summary>
        /// <param name="s">string to be encrypted</param>
        /// <returns>encrypted string</returns>
        public static string Encrypt(string s)
        {
            // universal encode: UTF-8, x2

            /*
             * ways of creating objects
             *  1.constructor
             *  2.static method (factory)
             */

            // creates encryption object
            MD5 md5 = MD5.Create();
            // converts string to byte array
            byte[] byteOld = Encoding.UTF8.GetBytes(s);
            // calls encryption method
            byte[] byteNew = md5.ComputeHash(byteOld);
            // converts what was encrypted to string
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // converts byte to hexadecimal string (two digits per byte)
                sb.Append(b.ToString("x2"));// two digits per byte, zero-filling
            }
            // returns encrypted string
            return sb.ToString();
        }
    }
}
