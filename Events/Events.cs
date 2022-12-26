using System;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Events;
using Alachisoft.NCache.Runtime.Exceptions;
using Models;
using System.Threading;

namespace Events
{
    internal class Events
    {
        private static ICache _cache;
        private static CacheEventDescriptor _eventDescriptor;
        private static string cacheName = "ClusteredCacheZainTest";
        public static void Run()
        {
            Console.WriteLine("-Starting Run (EVENTS)-");

            // Initialize cache
            InitializeCache();
            _cache.Clear();
            // Event notifications must be enabled in NCache manager -> Options for events to work
            //RegisterCacheNotification();
            Thread.Sleep(1000);
            Products p = new Products() { ProductID = 1, ProductName = "Beverages", UnitPrice = 5 };
            // Generate a new instance of product
            string key = p.ProductID.ToString();
            _cache.Add(key, p);
            AddNotificationOnKey(key);
            Thread.Sleep(1000);
            _cache.Insert(key, p);
            Thread.Sleep(1000);
            _cache.Remove(key);
            Thread.Sleep(1000);
            // Add item in cache
            //UpdateItem(key, p);

            //Thread.Sleep(1000);
            //// Register Notification for given key
            //// Update item to trigger key based notification.
            //UpdateItem(key, p);
            //Thread.Sleep(1000);
            //// Delete item to trigger key based notification.
            //DeleteItem(key);

            //Thread.Sleep(1000);
            //UnRegisterCacheNotification();

            //// Dispose cache once done
            //_cache.Dispose();

            //Console.WriteLine("-Exiting Run (EVENTS)-");
        }

        private static void InitializeCache()
        {
            try
            {

                // Connect to cache
                _cache = CacheManager.GetCache(cacheName);
                Console.WriteLine("Successfully Connected to " + cacheName);
            }
            catch (OperationFailedException ex)
            {
                // NCache specific exception
                if (ex.ErrorCode == NCacheErrorCodes.NO_SERVER_AVAILABLE)
                {
                    // Make sure NCache Service and cache is running
                }
                else
                {
                    // Exception can occur due to:
                    // Connection Failures
                    // Operation Timeout
                    // Operation performed during state transfer
                }
            }
            catch (ConfigurationException ex)
            {
                if (ex.ErrorCode == NCacheErrorCodes.SERVER_INFO_NOT_FOUND)
                {
                    // client.ncconf must have server information
                }
            }
            catch (Exception ex)
            {
                // Any generic exception like ArgumentNullException or ArgumentException
                // Argument exception occurs in case of empty string name
                // Make sure TLS is enabled on both client and server
            }
        }
        private static string GetKey(Products product)
        {
            return "Item:" + product.ProductID.ToString();
        }
        private static void RegisterCacheNotification()
        {
            CacheItem item = null;
            _eventDescriptor = _cache.MessagingService.RegisterCacheNotification(new CacheDataNotificationCallback(CacheDataModified),
                                                   EventType.ItemAdded | EventType.ItemRemoved | EventType.ItemUpdated,
                                                   EventDataFilter.Metadata);

            Console.WriteLine("Cache Notification " + ((_eventDescriptor.IsRegistered) ? "Registered successfully" : "Not Registered"));
            Console.WriteLine(_eventDescriptor.CacheName);
            Console.WriteLine(_eventDescriptor.DataFilter);
        }
        private static void UnRegisterCacheNotification()
        {
            _cache.MessagingService.UnRegisterCacheNotification(_eventDescriptor);
            Console.WriteLine("Cache Notification " + "Unregistered successfully");
        }
        private static void AddItem(string key, Products product)
        {
            CacheItem item = new CacheItem(product);
            _cache.Insert(key, item);
            //Console.WriteLine("Object Added in Cache");
        }
        private static void AddNotificationOnKey(string key)
        {
            CacheDataNotificationCallback notificationCallback = new CacheDataNotificationCallback(KeyNotificationMethod);
            _cache.MessagingService.RegisterCacheNotification(key, notificationCallback, EventType.ItemAdded | EventType.ItemRemoved | EventType.ItemUpdated,
                                            EventDataFilter.Metadata);
            Console.WriteLine("Event Register for Key:{0}", key);

        }
        private static void UpdateItem(string key, Products product)
        {
            product.ProductName = "updatedProduct1";
            _cache.Insert(key, product);
            //Console.WriteLine("Object Updated in Cache");
        }
        private static void DeleteItem(string key)
        {
            _cache.Remove(key);
            //Console.WriteLine("Object Removed from Cache");
        }
        public static void CacheDataModified(string key, CacheEventArg cacheEventArgs)
        {
            Console.WriteLine("Cache data modification notification for the the item of the key : {0}", key); //To change body of generated methods, choose Tools | Templates.
            Console.WriteLine(cacheEventArgs.EventType);
        }
        private static void KeyNotificationMethod(string key, CacheEventArg cacheEventArgs)
        {
            switch (cacheEventArgs.EventType)
            {
                case EventType.ItemAdded:
                    Console.WriteLine("Key: " + key + " is added to the cache");
                    break;
                case EventType.ItemRemoved:
                    Console.WriteLine("Key: " + key + " is removed from the cache");
                    break;
                case EventType.ItemUpdated:
                    Console.WriteLine("Key: " + key + " is updated in the cache");
                    Console.WriteLine(cacheEventArgs.Item.GetType());
                    break;
                default:
                    Console.WriteLine("Incorrect actions");
                    break;
            }
        }
        private static void CacheCustomEvent(object notifId, object data)
        {
            Console.WriteLine("The custom event has been raised");
        }
        private static Products GenerateProduct()
        {
            Products p = new Products() { ProductID = 1, ProductName = "Beverages", UnitPrice = 5 };
            return p;
        }
    }
}
