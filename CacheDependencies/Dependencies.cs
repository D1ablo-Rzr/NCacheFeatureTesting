using System;
using System.Collections.Generic;
using System.Text;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.CacheLoader;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.Dependencies;
using Models;

namespace CacheDependencies
{
    class Dependencies
    {
        private static ICache _cache;
        private static ICacheLoader loader;
        public static void Run()
        {
            InitializeCache();
            //createdependency();
            //check();
            AddFileBasedDependency(); 
        }

        private static void InitializeCache()
        {
            _cache = CacheManager.GetCache("ClusteredCacheZainTest");
            Console.WriteLine("Cache Initialized");
        }

        private static void createdependency()
        {
            Customer customer = new Customer { CustomerId = "Customer:76", CustomerName = "David Johnes", CompanyName = "Lonesome Pine Restaurant", CustomerTitle="CSE", Address = "Silicon Valley, Santa Clara, California" };
            _cache.Add(customer.CustomerId, customer);

            OrderDetails order = new OrderDetails { OrderID = 10248, OrderDate = DateTime.Parse("1996-08-16 00:00:00.000"), EmployeeID = 1 };

            CacheDependency dependency = new KeyDependency(customer.CustomerId);
            CacheItem cacheItem = new CacheItem(order);

            cacheItem.Dependency = dependency;
            _cache.Add(order.OrderID.ToString(), cacheItem);
        }

        private static void check()
        {
            _cache.Remove("Customer:76");
            Products prod = _cache.Get<Products>("10248");
            if(prod!=null)
            {
                Console.WriteLine("Dependency not created");
            }
            else
            {
                Console.WriteLine("Dependency was created. Both Items Removed");
            }
        }

        private static void AddFileBasedDependency()
        {
            string dependencyfile = "C:\\Users\\sal_bhatti\\source\\repos\\NCache-Samples\\dotnet\\Dependencies\\FileBasedDependency\\DependencyFile\\foobar.txt";

            Products product = new Products { ProductID = 74, ProductName = "Filo Mix",  UnitPrice = 46 };
            CacheItem cacheItem = new CacheItem(product);

            cacheItem.Dependency = new FileDependency(dependencyfile);

            _cache.Add(product.ProductID.ToString(), cacheItem);

            Console.WriteLine("\nItem '" + product.ProductID.ToString() + "' added to cache with file dependency. ");

            ModifyDependencyFile(dependencyfile);

            object item = _cache.Get<Products>(product.ProductID.ToString());
            if (item == null)
            {
                Console.WriteLine("Item has been removed due to file dependency.");
            }
            else
            {
                Console.WriteLine("File based dependency did not work. Dependency file located at " + dependencyfile + " might be missing or file not changed within the given interval.");
            }

            _cache.Remove(product.ProductID.ToString());
        }
        private static void ModifyDependencyFile(string path)
        {
            using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(path, true))
            {
                streamWriter.WriteLine(string.Format("\n{0}\tFile is modifed. ", DateTime.Now));
            }

            Console.WriteLine(string.Format("File '{0}' is modified programmatically.", path));
        }
    }
}
