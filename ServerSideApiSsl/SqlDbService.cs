using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Collections.Concurrent;
using System.ComponentModel;
using SharedClassLibrary;
using User = SharedClassLibrary.User;
using System.Data.Common;
using Microsoft.Extensions.Options;

namespace ServerSideApiSsl
{
    public class SqlDbService<T> : ISqlDbService<T>
    {
        private readonly SqlConnection _connection;

        public SqlDbService(IOptionsSnapshot<SqlSettings> options)
        {
            string connectionString = options.Value.ConnectionString;
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        public bool Authenticate(string username, string password)
        {
            string sql = $"SELECT * FROM Users WHERE Name = '{username}' AND Password = '{password}'";

            SqlCommand command = new(sql, _connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                // Need to save Id...
                // If more then one user exist
                if (reader.Read())
                {
                    return false;
                }
                return true;
            } 
            return false;
        }

        public async Task AddItemAsync(T item, string Id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<T?> GetItemAsync(string id)
        {
            throw new NotImplementedException();
            try
            {
                //...
            }
            catch (SqlException ex)
            {
                //...
            }
        }

        public async Task UpdateItemAsync(string id, T item)
        {
            throw new NotImplementedException();
        }
    }
}
