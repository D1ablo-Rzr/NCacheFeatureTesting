using System;
using System.Collections.Generic;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using System.Text;
using Models;

namespace OQL
{
    class OQL
    {
        private static ICache _cache;
        public static void Run()
        {
            InitializeCache();
            //_cache.Clear();
            //createData();
            //assignnamedtags();
            //searchbygroup();
            //searchnamedtags();
            var readthruoptions = new ReadThruOptions();
            readthruoptions.Mode = ReadMode.ReadThru;
            var cust = _cache.Get<Customer>("BLAUS", readthruoptions);
            Console.WriteLine(cust.CustomerName);

            var writethruoptions = new WriteThruOptions();
            writethruoptions.Mode = WriteMode.WriteThru;
            Customer Cust = new Customer()
            {
                CustomerId = "ALPHAK12",
                CompanyName="ALACHI",
                CustomerName="BHATTI",
                CustomerTitle="CSE",
                Address = "ISLO"
            };
            var item = new CacheItem(Cust);
            CacheItemVersion ver = _cache.Insert(Cust.CustomerId, item, writethruoptions);
            if(ver.Equals(null))
            {
                
            }
            else
            { 
                Console.WriteLine("Item will be inserted"); 
            }
            //ICacheReader read = queryindex();
            //printdata(read);
            //ICacheReader read1 = groupbyquery();
            //printdata(read1);
        }  

        private static void InitializeCache() 
        {
            CacheConnectionOptions connects = new CacheConnectionOptions();
            connects.EnableClientLogs = true;
            _cache = CacheManager.GetCache("MirroredNet", connects);
            Console.WriteLine("Cache Initialized");
        }

        private static CacheItem AddTag(CacheItem item,Tag[] tag)
        {
            item.Tags = tag;
            return item;
        }

        private static void AddtoCache(string key, CacheItem item)
        {
            _cache.Add(key, item);
        }

        private static void assignnamedtags()
        {
            for(int i=1;i<=20;i++)
            {
                string key = "ALPHAK" + i;
                Customer cust = _cache.Get<Customer>(key);
                var cacheitem = new CacheItem(cust);
                var namedtags = new NamedTagsDictionary();
                char? c = null;
                namedtags.Add("Region", c);
                namedtags.Add("Ability", "Mind Reading");
                namedtags.Add("Age", 26);
                cacheitem.NamedTags = namedtags;
                _cache.Insert(key, cacheitem);
            }

            for(int i=21;i<=40;i++)
            {
                string key = "ALPHAK" + i;
                Customer cust = _cache.Get<Customer>(key);
                var cacheitem = new CacheItem(cust);
                var namedtags = new NamedTagsDictionary();
                namedtags.Add("SkinColor", "Fair");
                namedtags.Add("BodyType", "Fat");
                namedtags.Add("Weight", 73);
                cacheitem.NamedTags = namedtags;
                _cache.Insert(key, cacheitem);
            }

            for (int i = 41; i <= 60; i++)
            {
                string key = "ALPHAK" + i;
                Customer cust = _cache.Get<Customer>(key);
                var cacheitem = new CacheItem(cust);
                cacheitem.Group = "North Coast";
                _cache.Insert(key, cacheitem);
            }

            for (int i = 61; i <= 80; i++)
            {
                string key = "ALPHAK" + i;
                Customer cust = _cache.Get<Customer>(key);
                var cacheitem = new CacheItem(cust);
                cacheitem.Group = "South Coast";
                _cache.Insert(key, cacheitem);
            }

            for (int i = 81; i <= 100; i++)
            {
                string key = "ALPHAK" + i;
                Customer cust = _cache.Get<Customer>(key);
                var cacheitem = new CacheItem(cust);
                cacheitem.Group = "Space Coast";
                _cache.Insert(key, cacheitem);
            }
        }

        private static void createData()
        {
            for(int i=1;i<=120;i++)
            {
                Customer Cust = new Customer();

                Cust.CustomerId = "ALPHAK" + i;
                Cust.CompanyName = "Alachisoft" + i;
                Cust.CustomerName = "Saifullah" + i;
                Cust.CustomerTitle = "CSE";
                Cust.Address = "Islamabad" + i;
                CacheItem item17 = new CacheItem(Cust);
                AddtoCache(Cust.CustomerId, item17);
            }

            //Products p= new Products() { ProductID = 1, ProductName = "Beverages", UnitPrice = 5 };
            //CacheItem item = new CacheItem(p);
            //Products p1= new Products() { ProductID = 2, ProductName = "Beverages", UnitPrice = 10 };
            //CacheItem item1 = new CacheItem(p1);
            //Products p2= new Products() { ProductID = 3, ProductName = "Beverages", UnitPrice = 20 };
            //CacheItem item2 = new CacheItem(p2);
            //Products p3= new Products() { ProductID = 4, ProductName = "Beverages", UnitPrice = 25 };
            //CacheItem item3 = new CacheItem(p3);
            //Products p4= new Products() { ProductID = 5, ProductName = "Beverages", UnitPrice = 30 };
            //CacheItem item4 = new CacheItem(p4);
            //Products p5= new Products() { ProductID = 6, ProductName = "laptops", UnitPrice = 35 };
            //CacheItem item5 = new CacheItem(p5);
            //Products p6= new Products() { ProductID = 7, ProductName = "laptops", UnitPrice = 40 };
            //CacheItem item6 = new CacheItem(p6);
            //Products p7= new Products() { ProductID = 8, ProductName = "laptops", UnitPrice = 45 };
            //CacheItem item7 = new CacheItem(p7);
            //Products p8= new Products() { ProductID = 9, ProductName = "laptops", UnitPrice = 50 };
            //CacheItem item8 = new CacheItem(p8);
            //Products p9= new Products() { ProductID = 10, ProductName = "Phones", UnitPrice = 55 };
            //CacheItem item9 = new CacheItem(p9);
            //Products p10= new Products() { ProductID = 11, ProductName = "Phones", UnitPrice = 60 };
            //CacheItem item10 = new CacheItem(p10);
            //Products p11= new Products() { ProductID = 12, ProductName = "Phones", UnitPrice = 65 };
            //CacheItem item11 = new CacheItem(p11);
            //Products p12= new Products() { ProductID = 13, ProductName = "Phones", UnitPrice = 70 };
            //CacheItem item12 = new CacheItem(p12);

            //AddtoCache(p.ProductID.ToString(), item);
            //AddtoCache(p1.ProductID.ToString(), item1);
            //AddtoCache(p2.ProductID.ToString(), item2);
            //AddtoCache(p3.ProductID.ToString(), item3);
            //AddtoCache(p4.ProductID.ToString(), item4);
            //AddtoCache(p5.ProductID.ToString(), item4);
            //AddtoCache(p6.ProductID.ToString(), item5);
            //AddtoCache(p7.ProductID.ToString(), item6);
            //AddtoCache(p8.ProductID.ToString(), item7);
            //AddtoCache(p9.ProductID.ToString(), item8);
            //AddtoCache(p10.ProductID.ToString(), item10);
            //AddtoCache(p11.ProductID.ToString(), item11);
            //AddtoCache(p12.ProductID.ToString(), item12);
        }

        private static ICacheReader queryindex()
        {
            var query = "select * FROM Models.Products";
            var querycommand = new QueryCommand(query);

            ICacheReader reader = _cache.SearchService.ExecuteReader(querycommand, false, 0);
            return reader;
        }

        private static ICacheReader groupbyquery()
        {
            var query = "select ProductName,COUNT(*) from Models.Products where UnitPrice>? group by ProductName";
            var querycommand = new QueryCommand(query);
            querycommand.Parameters.Add("UnitPrice", 10);

            ICacheReader reader = _cache.SearchService.ExecuteReader(querycommand, true, 0);

            if (reader.FieldCount > 0)
            {
                while (reader.Read())
                {
                    string result = reader.GetValue<string>(0);
                    string result1 = reader.GetValue<string>(1);
                    Console.WriteLine(result);
                    Console.WriteLine(result1);
                }
            }
                    
            return reader;
        }

        private static void printdata(ICacheReader reader)
        {
            if (reader.FieldCount > 0)
            {
                while (reader.Read())
                {
                    //Customer Cust = new Customer();
                    //Cust.CustomerId = reader.GetValue<string>("CustomerId");
                    //Cust.CompanyName = reader.GetValue<string>("CompanyName");
                    //Cust.CustomerName = reader.GetValue<string>("CustomerName");
                    //Cust.Address = reader.GetValue<string>("Address");
                    //Cust.CustomerTitle = reader.GetValue<string>("CustomerTitle");
                    //Console.WriteLine("ID: " + Cust.CustomerId);
                    //Console.WriteLine("Customer Name: " + Cust.CustomerName);
                    //Console.WriteLine("Company Name: " + Cust.CompanyName);
                    //Console.WriteLine("Customer Title: " + Cust.CustomerTitle);
                    //Console.WriteLine("Address: " + Cust.Address + "\n");
                    string key = reader.GetValue<string>(0);

                    Console.WriteLine("Key: " + key);
                }
            }
            else
            {
                Console.WriteLine("Reader Empty");
            }
        }

        private static void searchbygroup()
        {
            var query = "select CustomerId,CompanyName,CustomerName, Address, CustomerTitle from Models.Customer where $Group$= ?";
            var querycommand = new QueryCommand(query);
            querycommand.Parameters.Add("$Group$", "Space Coast");

            ICacheReader reader = _cache.SearchService.ExecuteReader(querycommand);
            printdata(reader);
        }

        private static void searchnamedtags()
        {
            var query = "select * from Models.Customer where Region=null";
            var querycommand = new QueryCommand(query);
            ICacheReader reader = _cache.SearchService.ExecuteReader(querycommand);
            printdata(reader);
        }
    }
}
