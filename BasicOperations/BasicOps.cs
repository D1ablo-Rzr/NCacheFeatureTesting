using System;
using System.Collections.Generic;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using System.Text;
using System.Collections;
using Models;
using System.Threading.Tasks;
using System.Threading;

namespace BasicOperations
{
    class BasicOps
    {
        private static ICache _cache;
        public static void Run()
        {
            InitializeCache();
            _cache.Clear();
            AddBulkItems();
            //Console.ReadLine();
            string[] keys = iteratecache();
            //InsertBulkItems();
            //Console.ReadLine();
            GetItem(keys);
            Console.ReadLine();
            //
            //DeleteBulkItems(keys);
            //Console.ReadLine();
            //addAsync();
            //Console.ReadLine();
            //contains(keys);
        }


        private static void InitializeCache()
        {
            _cache = CacheManager.GetCache("ClusteredCacheZainTest");
            Console.WriteLine("Cache Initialized");
        }

        private static string[] iteratecache()
        {
            List<string> keys = new List<string>();
            IEnumerator x = _cache.GetEnumerator();
            int i = 0;
            while (x.MoveNext())
            {
                DictionaryEntry item = (DictionaryEntry)x.Current;
                keys.Add(item.Key.ToString());
                i++;
            }
            string[] keyarr = keys.ToArray();
            return keyarr;
        }

        private static void Removedata(out object removeditem)
        {
            string key = "ALPHAK12";
            _cache.Remove(key, out removeditem);
        }

        private static void addAsync()
        {
            Customer Cust = new Customer();
            Cust.CustomerId = "ALPHAK";
            Cust.CustomerName = "ZAIN";
            Cust.CompanyName = "ALACHISOFT";
            Cust.CustomerTitle = "DEV";
            Cust.Address = "ISLO";
            CacheItem item = new CacheItem(Cust);
            Task t = _cache.AddAsync(Cust.CustomerId, item);
            bool x = true;
            do
            {
                if (t.IsCompleted)
                {
                    Console.WriteLine("Task Completed");
                    x = false;
                }
                else if (t.IsFaulted)
                {
                    Console.WriteLine(t.Status);
                    x = false;
                }
                else
                {
                    Console.WriteLine("Task still executing");
                    Thread.Sleep(100);
                }
            } while (x);

        }

        private static void AddBulkItems()
        {
            IDictionary<string, CacheItem> add_dict = new Dictionary<string, CacheItem>();
            IDictionary<string, Exception> ret_dict = new Dictionary<string, Exception>();
            Customer Cust = new Customer();
            int i = 1;
            try
            {
                while (i <= 120)
                {
                    Cust.CustomerId = "ALPHAK" + i;
                    Cust.CompanyName = "Alachisoft" + i;
                    Cust.CustomerName = "Saifullah" + i;
                    Cust.CustomerTitle = "CSE";
                    Cust.Address = "Islamabad" + i;
                    var cacheitem = new CacheItem(Cust);
                    add_dict.Add(Cust.CustomerId, cacheitem);
                    i++;
                }
                
                ret_dict = _cache.AddBulk(add_dict);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to add Data");
            }


            if (ret_dict.Count>0)
            {
                foreach(KeyValuePair<string, Exception> keyfailed in ret_dict)
                    Console.WriteLine($"Could not add Item {keyfailed.Key} in cache due to error : {keyfailed.Value}");

            }
        }

        private static void InsertBulkItems()
        {
            IDictionary<string, CacheItem> insert_dict = new Dictionary<string, CacheItem>();
            IDictionary<string, Exception> ret_dict = new Dictionary<string, Exception>();
            Customer Cust = new Customer();
            int i = 115;
            try
            {
                while (i <= 130)
                {
                    Cust.CustomerId = "ALPHAK" + i;
                    Cust.CompanyName = "Alachisoft" + i;
                    Cust.CustomerName = "Saifullah" + i;
                    Cust.CustomerTitle = "CSE";
                    Cust.Address = "Islamabad" + i;
                    var cacheitem = new CacheItem(Cust);
                    insert_dict.Add(Cust.CustomerId, cacheitem);
                    i++;
                }

                ret_dict = _cache.InsertBulk(insert_dict);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to add Data");
            }

            if (ret_dict.Count > 0)
            {
                foreach (KeyValuePair<string, Exception> keyfailed in ret_dict)
                    Console.WriteLine($"Could not add Item {keyfailed.Key} in cache due to error : {keyfailed.Value}");
            }
        }

        private static void GetBulkItems(string[] keys)
        {
            IDictionary<string, CacheItem> getitems = _cache.GetCacheItemBulk(keys);

            foreach(KeyValuePair<string, CacheItem> keyValuePair in getitems)
                Console.WriteLine($"Customer: {getitems.Values}, Address : { getitems.Values}");
        }

        private static void GetItem(string[] key)
        {
            for (int i = 0; i < key.Length; i++)
            {
                Customer cust = _cache.Get<Customer>(key[i]);
                printdata(cust);
            }
        }

        private static void printdata(Customer cust)
        {
            Console.WriteLine(cust.CustomerId);
            Console.WriteLine(cust.CompanyName);
            Console.WriteLine(cust.CustomerName);
            Console.WriteLine(cust.CustomerTitle);
            Console.WriteLine(cust.Address);
        }

        private static void DeleteBulkItems(string[] keys)
        {
            IDictionary<string, CacheItem> deleteditems = new Dictionary<string, CacheItem>();
            _cache.RemoveBulk(keys);
        }

        private static void contains(string[] keys)
        {
            for(int i=0;i<keys.Length;i++)
            {
                string key = keys[i];
                Console.WriteLine(key);
            }
        }
    }
}

