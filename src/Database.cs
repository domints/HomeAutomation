using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace src
{
    public interface IDatabase
    {
        void Work(Action<SqliteConnection> stuff);
        T Work<T>(Func<SqliteConnection, T> stuff);

        Task WorkAsync(Func<SqliteConnection, Task> stuff);
        Task<T> WorkAsync<T>(Func<SqliteConnection, Task<T>> stuff);
    }

    public class SqliteDatabase : IDatabase, IDisposable
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly SqliteConnection _connection;
        

        public SqliteDatabase()
        {
            _semaphore = new SemaphoreSlim(1,1);
            _connection = new SqliteConnection("Data Source=home.db");
            _connection.Open();
            Initialize();
        }

        private void Initialize()
        {
            using var createCallsTableCmd = _connection.CreateCommand();
            createCallsTableCmd.CommandText = "CREATE TABLE IF NOT EXISTS calls (number INTEGER, timestamp INTEGER, state TEXT, duration INTEGER);";
            createCallsTableCmd.ExecuteNonQuery();
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }

        public void Work(Action<SqliteConnection> stuff)
        {
            try
            {
                _semaphore.Wait();
                stuff(_connection);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public T Work<T>(Func<SqliteConnection, T> stuff)
        {
            try
            {
                _semaphore.Wait();
                return stuff(_connection);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task WorkAsync(Func<SqliteConnection, Task> stuff)
        {
            try
            {
                await _semaphore.WaitAsync();
                await stuff(_connection);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<T> WorkAsync<T>(Func<SqliteConnection, Task<T>> stuff)
        {
            try
            {
                await _semaphore.WaitAsync();
                return await stuff(_connection);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}