using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Entities.RequestFeatures
{
    public class CategoryParameters : RequestParameters
    {
        public string SearchCategory { get; set; }
    }
}
