using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Serializable]
    public class OrderDetails
    {
        public int OrderID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? OrderDate { get; set; }

    }
}
