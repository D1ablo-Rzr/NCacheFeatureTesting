using System;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using Models;

namespace OQL
{
    class Program
    {
        static void Main(string[] args) 
        {
            OQL.Run();
            //_cache.Clear();
            

            //var writethruoptions = new WriteThruOptions();
            //writethruoptions.Mode = WriteMode.WriteThru;
            //var cust1 = new Customer();
            //cust1.CustomerName = "Bhatti";
            //cust1.CompanyName = "Alachisoft";
            //cust1.Address = "Islo";
            //cust1.CustomerTitle = "SCE";
            //cust1.CustomerId = "STBRJ";
            //for (int i = 10; i < 1000; i++)
            //{
            //    cust1.CustomerId = cust1.CustomerId + i;
            //    var cacheitem = new CacheItem(cust1);
            //    _cache.Add(cust1.CustomerId, cacheitem);
            //}
        }
    }
}
