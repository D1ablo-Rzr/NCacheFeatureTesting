using System;
using System.Collections.Generic;
using Alachisoft.NCache.Client;
using System.Text;
using Models;

namespace NCacheFeatureTesting
{
    public class OptimisticLocking
    {
        private static ICache _cache;
        public static void Run()
        {
            InitializeCache();
            List<Customer> TestList = AddCustomer(73);
            var vers = AddIteminCache(TestList);
            var item = GetCacheItem("2");
            Console.ReadLine();
            var vers1 = UpdateItem("2", item, vers);
            var vers2 = UpdateItem("2", item, vers1);
            var result = GetifNewer("2", vers2);
            RemoveItem("2", vers2);
            
        }

        private static void InitializeCache()
        {
            _cache = CacheManager.GetCache("ClusteredCacheZainTest");
            Console.WriteLine("Cache Initialized");
        }

        private static List<Customer> AddCustomer(int i)
        {
            List<Customer> CustList = new List<Customer>();
            for (int j = 1; j <= i; j++)
            {
                Customer Cust = new Customer();
                Cust.CustomerId = "ALPHAK" + j;
                Cust.CompanyName = "Alachisoft" + j;
                Cust.CustomerName = "Saifullah" + j;
                Cust.CustomerTitle = "CSE" + j;
                Cust.Address = "Islamabad" + j;
                CustList.Add(Cust);
            }

            return CustList;
        }

        private static CacheItemVersion AddIteminCache(List<Customer> Cust)
        {
            string key = "2";

            CacheItem item = new CacheItem(Cust);
            CacheItemVersion version = _cache.Add(key, item);
            Console.WriteLine("Data Added Succesfully");
            return version;
        }

        private static void RemoveItem(string key, CacheItemVersion version)
        {
            List<Customer> Cust = new List<Customer>();
            _cache.Remove(key,null,version);
        }

        private static string GetKey()
        {
            return "0";
        }

        private static CacheItem GetCacheItem(string key)
        {
            CacheItem item = _cache.GetCacheItem(key);
            return item;
        }

        private static List<Customer> GetItem(string key, CacheItemVersion version)
        {

            CacheItem item = _cache.GetCacheItem(key, ref version);
            List<Customer> ls = item.GetValue<List<Customer>>();
            if(ls.Count!=0)
            {
                Console.WriteLine("Data Retreived Successfully");
                return ls;
            }
            else
            {
                Console.WriteLine("Data Not Retreived, Returning Null");
                return null;
            }
            
        }

        private static List<Customer> GetifNewer(string key, CacheItemVersion version)
        {
            var item = _cache.GetIfNewer<List<Customer>>(key,ref version);
            if (item != null)
            {
                Console.WriteLine("Newer Version Exists");
                return item;
            }
            else
            {
                Console.WriteLine("Same Version as in the cache");
                return null;
            }
        }

        private static CacheItemVersion UpdateItem(string key, CacheItem item, CacheItemVersion vers)
        {
            if (item != null)
            {
                List<Customer> ls = item.GetValue<List<Customer>>();
                ls[0].CustomerName = "Edward";
                CacheItem updateditem = new CacheItem(ls);
                updateditem.Version = vers;
                Console.ReadLine();
                var vers1 = _cache.Insert(key, updateditem);

                Console.WriteLine("Item Version Updated");
                Console.WriteLine(updateditem.Version);
                List<Customer> custls = updateditem.GetValue<List<Customer>>();
                PrintCustomerDetails(custls);
                return vers1;
            }
            else
            {
                Console.WriteLine("Item Version Invalid");
                return null;
            }
        }

        public static void PrintCustomerDetails(List<Customer> customer)
        {
            if (customer != null)
            {
                foreach (var c in customer)
                {
                    Console.WriteLine();
                    Console.WriteLine("Customer Details are as follows: ");
                    Console.WriteLine("Name: " + c.CustomerName);
                    Console.WriteLine("Company Name: " + c.CompanyName);
                    Console.WriteLine("Address: " + c.Address);
                }
            }
            else
            {
                Console.WriteLine("No data was returned");
            }
        }
    }
}
