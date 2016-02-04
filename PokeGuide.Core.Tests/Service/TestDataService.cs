using System;
using System.Threading.Tasks;
using NUnit.Framework;
using PokeGuide.Core.Service.Interface;
using SQLite.Net.Interop;

namespace PokeGuide.Core.Tests.Service
{
    public class StorageImplementation : IStorageService
    {
        public Task CopyDatabaseAsync(string fileName)
        {
            var tcs = new TaskCompletionSource<Task>();
            return tcs.Task;
        }

        public Task<string> GetDatabasePathForFileAsync(string fileName)
        {
            var tcs = new TaskCompletionSource<string>();
            tcs.SetResult("");
            return tcs.Task;
        }
    }

    public class SqlitePlatformImplementation : ISQLitePlatform
    {
        public IReflectionService ReflectionService
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ISQLiteApi SQLiteApi
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IStopwatchFactory StopwatchFactory
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IVolatileService VolatileService
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }

    public class TestDataService
    {
    }
}
