using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublisherTest
{
    [Serializable]
    class Order
    {
        public string OrderId { get; set; }

        public DateTime OrderDate { get; protected set; }
        public DateTime ShippedDate { get; set; }

        public static T GenerateOrder<T>() where T : Order, new()
        {
            T order = new T();
            order.OrderDate = DateTime.Now;
            return order;
        }
    }

    [Serializable]
    class ElectronicsOrder : Order
    {
        public ElectronicsOrder()
        {
            OrderId = $"ElectronicsOrder{Guid.NewGuid()}";
        }
    }

    [Serializable]
    class GarmentsOrder : Order
    {
        public GarmentsOrder()
        {
            OrderId = $"GarmentsOrder{Guid.NewGuid()}";
        }
    }

   
}

