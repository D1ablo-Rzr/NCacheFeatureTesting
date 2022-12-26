using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.DatasourceProviders;
using Alachisoft.NCache.Web.Caching;

namespace Provider
{
    public class ReadThrough : IReadThruProvider
    {
        private SqlDatasource sqlDatasource;
        

        /// <summary>
        /// Perform tasks like allocating resources or acquiring connections
        /// </summary>
        /// <param name="parameters">Startup paramters defined in the configuration</param>
        /// <param name="cacheId">Define for which cache provider is configured</param>
        public void Init(IDictionary parameters, string cacheId)
        {
            object connString = parameters["connstring"];
            sqlDatasource = new SqlDatasource();
            sqlDatasource.Connect(connString == null ? "" : connString.ToString());
        }

        /// <summary>
        ///  Perform tasks associated with freeing, releasing, or resetting resources.
        /// </summary>
        public void Dispose()
        {
            sqlDatasource.DisConnect();
        }

        /// <summary>
        /// Responsible for loading an object from the external data source. 
        /// Key is passed as parameter.
        /// <param name="key">item identifier; probably a primary key</param>
        /// <returns>data contained in ProviderCacheItem</returns>
        public ProviderCacheItem LoadFromSource(string key)
        {
            ProviderCacheItem cacheItem = new ProviderCacheItem(sqlDatasource.LoadCustomer(key));
            cacheItem.ResyncOptions.ResyncOnExpiration = true;
            // Resync provider name will be picked from default provider.
            return cacheItem;
        }

        #region IReadThruProvider Members
        /// <summary>
        /// Responsible for loading multiple objects from the external data source. 
        /// </summary>
        /// <param name="keys">Collection of keys to be fetched from data source</param>
        /// <returns>Dictionary of keys with respective data contained in ProviderCacheItem</returns>
        public IDictionary<string, ProviderCacheItem> LoadFromSource(ICollection<string> keys)
        {
            // initialize dictionary to return to cache
            IDictionary<string, ProviderCacheItem> providerItems = new Dictionary<string, ProviderCacheItem>();
            // iterate through all the keys and fetch data
            foreach (string key in keys)
            {
                // get data from data source
                object data = sqlDatasource.LoadCustomer(key);
                // initialize ProviderCacheItem with the received data
                ProviderCacheItem item = new ProviderCacheItem(data);
                // you can change item properties before adding to cache. For example
                // adding expiration
                item.Expiration = new Expiration(ExpirationType.DefaultAbsolute);
                // assigning group to key
                item.Group = "customers";

                // add the provider item with the key to result
                providerItems.Add(key, item);
            }
            // return result to cache
            return providerItems;
        }

        /// <summary>
        /// Responsible for loading data structures from the external data source. 
        /// </summary>
        /// <param name="key">key to fetch from data source</param>
        /// <param name="dataType">type of data structure received</param>
        /// <returns>Data structure contained in ProviderCacheItem which can be enumerated</returns>
        public ProviderDataTypeItem<IEnumerable> LoadDataTypeFromSource(string key, DistributedDataType dataType)
        {
            // initialize ProviderDataTypeItem to return
            ProviderDataTypeItem<IEnumerable> providerItem = null;
            // create a switch to handle different Data Structures
            switch (dataType)
            {
                // incase of counter, the value will be a long data type
                case DistributedDataType.Counter:
                    providerItem = new ProviderDataTypeItem<IEnumerable>(sqlDatasource.GetCustomerCountByCompanyName(key));
                    break;
                case DistributedDataType.Queue:
                    providerItem = new ProviderDataTypeItem<IEnumerable>(sqlDatasource.LoadCustomersByOrder(key));
                    break;
                case DistributedDataType.Set:
                    // only primitive types can be used for HashSet
                    providerItem = new ProviderDataTypeItem<IEnumerable>(sqlDatasource.LoadOrderIDsByCustomer(key));
                    break;
            }

            return providerItem;
        }
        #endregion
    }
}
