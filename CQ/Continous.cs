using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Events;
using Alachisoft.NCache.Runtime.Exceptions;
using Models;

namespace CQ
{
    public class Continous
    {
        private static ICache _cache;
        static void Main(string[] args)
        {
            Run();
        }

        static void Run()
        {
            Initialize();
            Products pord = new Products();
            pord = _cache.Get<Products>("ProductId:1");
            Console.WriteLine(pord.ProductID);
            Console.WriteLine(pord.ProductName);
            Console.WriteLine(pord.UnitPrice);
        }

        static void Initialize()
        {
            _cache = CacheManager.GetCache("MirroredNet");

            // Print output on console
            Console.WriteLine(string.Format("\nCache '{0}' is initialized.", "ClusteredCacheZainTest"));
        }
        
        static void RegisterQuery(ContinuousQuery cquery)
        {
            cquery.RegisterNotification(new QueryDataNotificationCallback(QueryItemCallBack), EventType.ItemAdded|EventType.ItemRemoved| EventType.ItemUpdated, EventDataFilter.DataWithMetadata);
            _cache.MessagingService.RegisterCQ(cquery);
        }

        static void UnRegisterQuery(ContinuousQuery cquery)
        {
            _cache.MessagingService.UnRegisterCQ(cquery);
        }
        static void QueryItemCallBack(string key, CQEventArg arg)
        {
            switch (arg.EventType)
            {
                case EventType.ItemAdded:
                    Console.WriteLine("Item Added");
                    break;

                case EventType.ItemUpdated:
                    Customer Cust = arg.OldItem.GetValue<Customer>();
                    Customer Cust1 = arg.Item.GetValue<Customer>();
                    Console.WriteLine("Item Updated");
                    Console.WriteLine("Old value: "+Cust.Address);
                    Console.WriteLine("New value: "+Cust1.Address);
                    Console.WriteLine(arg.Item.CacheItemVersion);
                    break;

                case EventType.ItemRemoved:
                    Console.WriteLine("Item Removed");
                    break;
            }
        }

    }
}
