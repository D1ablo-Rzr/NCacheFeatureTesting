using System;
using Alachisoft.NCache.Runtime;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Cache;
using Models;

namespace NCacheFeatureTesting
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //NCacheFeatureTesting.PessimisticLocking.Run();
            //NCacheFeatureTesting.OptimisticLocking.Run();
            NCacheFeatureTesting.OQL.Run();
        }
    }

    
}
