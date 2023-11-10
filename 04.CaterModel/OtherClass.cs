using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04.CaterModel
{
    public partial class MemberInfo
    {
        public string MTypeTitle { get; set; }
        public decimal MDiscount { get; set; }
    }

    public partial class DishInfo
    {
        public string DTypeTitle { get; set; }
    }

    public partial class TableInfo
    {
        public string THallTitle { get; set; }
    }

    public partial class OrderDetailInfo
    {
        public string DTitle { get; set; }
        public decimal DPrice { get; set; }
    }
}
