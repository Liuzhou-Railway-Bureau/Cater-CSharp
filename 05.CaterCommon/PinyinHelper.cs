using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05.CaterCommon
{
    public partial class PinyinHelper
    {
        /// <summary>
        /// gets each char's first Pinyin letter
        /// </summary>
        /// <param name="s">provided string</param>
        /// <returns>each char's first Pinyin letter</returns>
        public static string GetPinyin(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                ChineseChar cc = new ChineseChar(c);
                sb.Append(cc.Pinyins[0][0]);
            }
            return sb.ToString();
        }
    }
}
