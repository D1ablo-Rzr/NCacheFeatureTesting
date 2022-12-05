using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Client;
using Models;

namespace DAL
{
    public class PessimisticLocking
    {
        private static ICache _cache;
        private static void Run()
        {
            InitializeCache();
            List<Customer> TestList = AddCustomer(12);
            AddIteminCache(TestList);
            string key = GetKey(TestList);
            RemoveItem(key);
        }

        private static void InitializeCache()
        {
             _cache = CacheManager.GetCache("ClusteredCacheZainTest");
            Console.WriteLine("Cache Initialized");
        }

        private static List<Customer> AddCustomer(int i)
        {
            List <Customer> CustList = new List<Customer>();
            for (int j = 1; j <= i; j++)
            {
                Customer Cust = new Customer();
                Cust.CustomerId = "ALPHAK"+j;
                Cust.CompanyName = "Alachisoft"+j;
                Cust.CustomerName = "Saifullah"+j;
                Cust.CustomerTitle = "CSE"+j;
                Cust.Address = "Islamabad"+j;
                CustList.Add(Cust);
            }

            return CustList;
        }

        private static void AddIteminCache(List<Customer> Cust)
        {
            string key = string.Format("Customer:{0}", Cust[0].CustomerId);

            CacheItem item = new CacheItem(Cust);
            _cache.Add(key, item);
            Console.WriteLine("Data Added Succesfully");
        }

        private static void RemoveItem(string key)
        {
            Customer Cust=new Customer();
            bool isremoved=_cache.Remove<Customer>(key,out Cust);
            if(isremoved)
            {
                Console.WriteLine("Item Removed successfully");
            }
            else
            {
                Console.WriteLine("Error Occured");
            }

        }

        private static string GetKey(List<Customer> Cust)
        {
            return string.Format("Customer:{0}", Cust[0].CustomerId);
        }

    }
}
