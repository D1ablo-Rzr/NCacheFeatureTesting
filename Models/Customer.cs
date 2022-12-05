using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alachisoft.NCache.Runtime;
using Alachisoft.NCache.Runtime.Caching;

namespace Models
{
    [Serializable]
    public class Customer
    {
        [QueryIndexed]
        public string CustomerId { get; set; }
        [QueryIndexed]
        public string CompanyName { get; set; }
        [QueryIndexed]
        public string CustomerName { get; set; }
        [QueryIndexed]
        public string CustomerTitle { get; set; }
        [QueryIndexed]
        public string Address { get; set; }

    }
}
