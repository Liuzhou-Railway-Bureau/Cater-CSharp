using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.CaterUI
{
    public class DdlFreeSearchModel
    {
        public string Title { get; set; }
        public string Id { get; set; }

        public DdlFreeSearchModel(string id, string title)
        {
            this.Title = title;
            this.Id = id;
        }
    }
}
