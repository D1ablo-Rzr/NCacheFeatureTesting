using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.CacheLoader;
using Alachisoft.NCache.Runtime.Caching;
using Models;

namespace LoaderRefresher
{
    class Loader : ICacheLoader
    {
        private ICache _cache;
        private string _connectionString;
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, RefreshPreference> GetDatasetsToRefresh(IDictionary<string, object> userContexts)
        {
            IDictionary<string, RefreshPreference> DatasetsNeedToRefresh = new Dictionary<string, RefreshPreference>();
            DateTime? lastRefreshTime;
            bool datasetHasUpdated;

            foreach (var dataSet in userContexts.Keys)
            {
                switch (dataSet.ToLower())
                {
                    case "products":
                        lastRefreshTime = userContexts[dataSet] as DateTime?;
                        datasetHasUpdated = HasProductDatasetUpdated(dataSet, lastRefreshTime);
                        if (datasetHasUpdated)
                        {
                            DatasetsNeedToRefresh.Add(dataSet, RefreshPreference.RefreshNow);
                        }
                        break;
                    case "customers":
                        lastRefreshTime = userContexts[dataSet] as DateTime?;
                        datasetHasUpdated = HasCustomerDatasetUpdated(dataSet, lastRefreshTime);
                        if (datasetHasUpdated)
                        {
                            DatasetsNeedToRefresh.Add(dataSet, RefreshPreference.RefreshOnNextTimeOfDay);
                        }
                        break;
                    default:
                        throw new InvalidOperationException("Invalid Dataset.");
                }
            }

            return DatasetsNeedToRefresh;
        }

        public void Init(IDictionary<string, string> parameters, string cacheName)
        {
            if (parameters != null && parameters.Count > 0)
                _connectionString = parameters.ContainsKey("ConnectionString")
                    ? parameters["ConnectionString"] as string : string.Empty;

            _cache = CacheManager.GetCache(cacheName);
        }

        public object LoadDatasetOnStartup(string dataset)
        {
            IList<object> datasetToLoad=null;

            if (dataset.ToLower() is "customers")
            {
                datasetToLoad = FetchCustomersFromDataSource();
                string[] keys = GetKeys(datasetToLoad);
                IDictionary<string, CacheItem> cacheData = GetCacheItemDictionary(keys, datasetToLoad);
                _cache.InsertBulk(cacheData);
            }
            else if (dataset.ToLower() is "products")
            {
                datasetToLoad = FetchProductsFromDataSource();
                string[] keys = GetKeys(datasetToLoad);
                IDictionary<string, CacheItem> cacheData = GetCacheItemDictionary(keys, datasetToLoad);
                _cache.InsertBulk(cacheData);
            }
            object userContext = DateTime.Now;
            return userContext;
        }

        public object RefreshDataset(string dataSet, object userContext)
        {
            if (string.IsNullOrEmpty(dataSet))
                throw new InvalidOperationException("Invalid dataset.");

            DateTime? lastRefreshTime;

            switch (dataSet.ToLower())
            {
                case "products":
                    lastRefreshTime = userContext as DateTime?;
                    IList<Products> productsNeedToRefresh = FetchUpdatedProducts(lastRefreshTime) as IList<Products>;
                    foreach (var product in productsNeedToRefresh)
                    {
                        string key = $"ProductID:{product.ProductID}";
                        CacheItem cacheItem = new CacheItem(product);
                        _cache.Insert(key, cacheItem);
                    }
                    break;
                case "customers":
                    lastRefreshTime = userContext as DateTime?;
                    IList<Customer> suppliersNeedToRefresh = FetchUpdatedCustomers(lastRefreshTime) as IList<Customer>;
                    foreach (var customer in suppliersNeedToRefresh)
                    {
                        string key = $"CustomerID:{customer.CustomerId}";
                        CacheItem cacheItem = new CacheItem(customer);
                        _cache.Insert(key, cacheItem);
                    }

                    break;
                default:
                    throw new InvalidOperationException("Invalid Dataset.");
            }

            userContext = DateTime.Now;
            return userContext;
        }
        private IList<object> FetchProductsFromDataSource()
        {
            string Query = "select ProductID, ProductName,UnitPrice from Products where UnitsInStock > 0";
            return ExecuteQuery(Query, "products");

        }

        private IList<object> FetchCustomersFromDataSource()
        {
            string Query = "select CustomerID,CompanyName,ContactName,ContactTitle,Address from Customers";
            return ExecuteQuery(Query, "customers");

        }

        private IList<object> ExecuteQuery(string Query, string dataSet)
        {
            IList<object> Data;

            using (SqlConnection myConnection = new SqlConnection(_connectionString))
            {
                SqlCommand oCmd = new SqlCommand(Query, myConnection);

                myConnection.Open();

                using (var reader = oCmd.ExecuteReader())
                {
                    Data = GetData(reader, dataSet);
                }

                myConnection.Close();
            }
            return Data;
        }

        public string[] GetKeys(IList<object> objects)
        {
            string[] keys = new string[objects.Count];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = objects[i].GetType() == typeof(Products) ? $"ProductId:{(objects[i] as Products).ProductID}" : $"CustomerId:{(objects[i] as Customer).CustomerId}";
            }

            return keys;
        }

        public static IDictionary<string, CacheItem> GetCacheItemDictionary(string[] keys, IList<Object> value)
        {
            IDictionary<string, CacheItem> items = new Dictionary<string, CacheItem>();
            CacheItem cacheItem = null;

            for (int i = 0; i < value.Count - 1; i++)
            {
                cacheItem = new CacheItem(value[i]);
                items.Add(keys[i], cacheItem);
            }

            return items;
        }

        private bool HasProductDatasetUpdated(string dataSet, object dateTime)
        {
            bool result = false;
            string query = $"select count(*) from Products where LastModify > '{dateTime as DateTime?}'";
            result = ExecuteAggregateQuery(query) > 0;
            return result;
        }

        private bool HasCustomerDatasetUpdated(string dataSet, object dateTime)
        {
            bool result = false;
            string query = $"select count(*) from Customers where LastModify > '{dateTime as DateTime?}'";
            result = ExecuteAggregateQuery(query) > 0;
            return result;
        }

        private IList<object> FetchUpdatedProducts(object dateTime)
        {
            string Query = $"select * from Products where LastModify > '{dateTime as DateTime?}'";
            return ExecuteQuery(Query, "products");

        }

        private IList<object> FetchUpdatedCustomers(object dateTime)
        {
            string Query = $"select * from Customers where LastModify > '{dateTime as DateTime?}'";
            return ExecuteQuery(Query, "customers");

        }

        private int ExecuteAggregateQuery(string Query)
        {
            int result = 0;
            using (SqlConnection myConnection = new SqlConnection(_connectionString))
            {
                SqlCommand oCmd = new SqlCommand(Query, myConnection);

                myConnection.Open();
                using (var reader = oCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = Convert.ToInt32(reader[0]);
                    }
                }
                myConnection.Close();
            }
            return result;
        }

        private IList<object> GetData(SqlDataReader sqlDataReader, string dataSet)
        {
            IList<object> dataList = new List<object>();
            while (sqlDataReader.Read())
            {
                if (string.Compare(dataSet, "customers", true) == 0)
                {
                    Customer customer = new Customer()
                    {
                        CustomerId = sqlDataReader["CustomerID"].ToString(),
                        CompanyName = sqlDataReader["CompanyName"].ToString(),
                        CustomerName = sqlDataReader["ContactName"].ToString(),
                        CustomerTitle = sqlDataReader["ContactTitle"].ToString(),
                        Address = sqlDataReader["Address"].ToString()
                    };
                    dataList.Add(customer);

                }
                if (string.Compare(dataSet, "products", true) == 0)
                {
                    Products product = new Products()
                    {
                        ProductID = Convert.ToInt32(sqlDataReader["ProductID"]),
                        ProductName = sqlDataReader["ProductName"].ToString(),
                        UnitPrice = Convert.ToInt32(sqlDataReader["UnitPrice"])
                    };
                    dataList.Add(product);

                }
            }
            return dataList;
        }

    }
}
