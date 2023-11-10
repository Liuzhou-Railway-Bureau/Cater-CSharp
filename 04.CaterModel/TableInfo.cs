using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04.CaterModel
{
    public partial class TableInfo
    {
        public int TId { get; set; }
        public string TTitle { get; set; }
        public int THallId { get; set; }
        public bool TIsFree { get; set; }
        public bool TIsDelete { get; set; }
    }
}
