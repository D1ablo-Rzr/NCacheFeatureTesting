using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alachisoft.NCache.Runtime.Caching;

namespace Models
{
    [Serializable]
    public class Products
    {
        [QueryIndexed]
        public int ProductID { get; set; }

        [QueryIndexed]
        public string ProductName { get; set; }

        [QueryIndexed]
        public int UnitPrice { get; set; }
        
    }
}
