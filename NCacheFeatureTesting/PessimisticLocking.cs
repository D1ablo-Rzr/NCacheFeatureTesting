using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Client;
using Models;

namespace NCacheFeatureTesting
{
    public class PessimisticLocking
    {
        private static ICache _cache;
        public static void Run()
        {
            InitializeCache();
            //_cache.Clear();
            LockHandle lockHandle = new LockHandle();
            LockHandle lockHandle1 = new LockHandle();
            TimeSpan timeSpan = new TimeSpan(0, 0, 0, 20);

            //List<Customer> TestList = AddCustomer(73);
            //AddIteminCache(TestList);
            //string key = GetKey();
            //lockHandle = LockItem("2", lockHandle, timeSpan);

            //UpdateItem("2", timeSpan, lockHandle);
            //GetItem("2", lockHandle, timeSpan);
            ////UnLockForcefull("2",lockHandle);


            RemoveItem("2",lockHandle1);
            //LockItem("1", lockHandle, timeSpan);
            //GetItem("1", lockHandle1, timeSpan);
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
            string key = "2";

            CacheItem item = new CacheItem(Cust);
            _cache.Add(key, item);
            Console.WriteLine("Data Added Succesfully");
        }

        private static void RemoveItem(string key,LockHandle lockHandle)
        {
            List<Customer> Cust =new List<Customer>();
            _cache.Remove(key,lockHandle);
            //if(isremoved)
            //{
            //    Console.WriteLine("Item Removed successfully");
            //}
            //else
            //{
            //    Console.WriteLine("Error Occured");
            //}

        }

        private static string GetKey()
        {
            return "0";
        }

        private static void GetItem(string key, LockHandle lockHandle, TimeSpan timeSpan)
        {
            List<Customer> getCustomer = _cache.Get<List<Customer>>(key, false, timeSpan, ref lockHandle);

            PrintCustomerDetails(getCustomer);
            Console.WriteLine("Lock acquired on " + lockHandle.LockId);
        }

        private static LockHandle LockItem(string key, LockHandle lockHandle, TimeSpan timeSpan)
        {
            bool isLocked = _cache.Lock(key, timeSpan, out lockHandle);

            if (isLocked)
            {
                Console.WriteLine("Lock acquired on " + lockHandle.LockId);
                return lockHandle;
            }
            else
            {
                Console.WriteLine("Lock Not Acquired");
                return null;
            }
        }

        private static void UnLockItemInCache(string key, LockHandle lockHandle)
        {
            _cache.Unlock(key, lockHandle);
        }

        private static void UnLockForcefull(string key, LockHandle lockHandle)
        {
            _cache.Unlock(key);
            TimeSpan timeSpan = new TimeSpan(0, 0, 0, 20);
            List<Customer> getCustomer = _cache.Get<List<Customer>>(key, false, timeSpan, ref lockHandle);

            PrintCustomerDetails(getCustomer);
        }

        private static void UpdateItem(string key, TimeSpan timespan, LockHandle lockHandle)
        {
            List<Customer> CustList = _cache.Get<List<Customer>>(key, false, timespan, ref lockHandle);
            CustList[0].CustomerName = "Edward";
            CacheItem item = new CacheItem(CustList);
            CacheItemVersion version = _cache.Insert(key, item, lockHandle, true);
        }

        public static void PrintCustomerDetails(List<Customer> customer)
        {
            if (customer!= null)
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
