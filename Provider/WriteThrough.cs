using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.DatasourceProviders;
using Alachisoft.NCache.Web.Caching;
using Models;

namespace Provider
{
    class WriteThrough : IWriteThruProvider
    {

        private SqlDatasource sqlDatasource;
        public void Dispose()
        {
            sqlDatasource.DisConnect();
        }

        public void Init(IDictionary parameters, string cacheId)
        {
            object connString = parameters["connstring"];
            sqlDatasource = new SqlDatasource();
            sqlDatasource.Connect(connString == null ? "" : connString.ToString());
        }

        public OperationResult WriteToDataSource(WriteOperation operation)
        {
            bool result = false;
            // initialize operation result with failure
            OperationResult operationResult = new OperationResult(operation, OperationResult.Status.Failure);
            // get value of object
            Customer value = operation.ProviderItem.GetValue<Customer>();
            // check if value is the type you need
            if (value.GetType().Equals(typeof(Customer)))
            {
                // send data to cache for writing
                result = sqlDatasource.AddCustomer((Customer)value);
                // if write operatio is success, change status of operation result
                if (result) operationResult.OperationStatus = OperationResult.Status.Success;
            }
            // return result to cache
            return operationResult;
        }

        public ICollection<OperationResult> WriteToDataSource(ICollection<WriteOperation> operations)
        {
            ICollection<OperationResult> operationResults = new List<OperationResult>();
            // initialize variable for confirmation of write operation
            bool result = false;
            // iterate over each operation sent by cache
            foreach (var item in operations)
            {
                // initialize operation result with failure
                OperationResult operationResult = new OperationResult(item, OperationResult.Status.Failure);
                // get object from provider cache item
                Customer value = item.ProviderItem.GetValue<Customer>();
                // check if the type is what you need
                if (value.GetType().Equals(typeof(Customer)))
                {
                    // perform write command and get confirmation from data source
                    result = sqlDatasource.AddCustomer((Customer)value);
                    // if write operation is successful, change status of operationResult
                    if (result) operationResult.OperationStatus = OperationResult.Status.Success;
                }
                // insert result operation to collect of results
                operationResults.Add(operationResult);
            }

            // send result to cache
            return operationResults;
        }

        public ICollection<OperationResult> WriteToDataSource(ICollection<DataTypeWriteOperation> dataTypeWriteOperations)
        {
            ICollection<OperationResult> operationResults = new List<OperationResult>();
            // initialize variable for confirmation of write operation
            bool result = false;
            // iterate over each operation sent by cache
            foreach (DataTypeWriteOperation operation in dataTypeWriteOperations)
            {
                // initialize operation result with failure
                OperationResult operationResult = new OperationResult(operation, OperationResult.Status.Failure);
                // determine the type of data structure
                switch (operation.DataType)
                {
                    // for counters, get value from ProviderItem.Counter
                    case DistributedDataType.Counter:
                        result = sqlDatasource.SaveCounter(operation.ProviderItem.Counter);
                        if (result) operationResult.OperationStatus = OperationResult.Status.Success;
                        break;
                    // for every other data structure, the new entry is sent as object from cache
                    default:
                        if (operation.OperationType.Equals(DatastructureOperationType.UpdateDataType))
                            result = sqlDatasource.SaveCustomer((Customer)operation.ProviderItem.Data);
                        else if (operation.OperationType.Equals(DatastructureOperationType.AddToDataType))
                            result = sqlDatasource.AddCustomer((Customer)operation.ProviderItem.Data);
                        else if (operation.OperationType.Equals(DatastructureOperationType.DeleteFromDataType))
                            result = sqlDatasource.RemoveCustomer((Customer)operation.ProviderItem.Data);
                        if (result) operationResult.OperationStatus = OperationResult.Status.Success;
                        break;
                }
                // add result to list of operation results
                operationResults.Add(operationResult);
            }
            // return list of operations to cache
            return operationResults;
        }
    }
}
