using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Collections.Concurrent;
using System.ComponentModel;
using SharedClassLibrary;
using User = SharedClassLibrary.User;
using System.Data.Common;
using Microsoft.Extensions.Options;
using System.Data;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;

namespace ServerSideApiSsl
{
    public class SqlDbService<T> : ISqlDbService<T>
    {
        private readonly SqlConnection _connection;

        public SqlDbService(IOptionsSnapshot<SqlSettings> options)
        {
            string connectionString = options.Value.ConnectionString;
            _connection = new SqlConnection(connectionString);
        }

        public bool Authenticate(string username, string password)
        {
            string sql = $"SELECT Id FROM Users WHERE Name = '{username}' AND Password = '{password}'";
            try
            {
                _connection.Open();
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
            } catch (Exception)
            {
                throw;
            }
            finally
            {
                _connection.Close();
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

        public User? GetUser(string id)
        {
            string sql = $"SELECT * FROM Users WHERE Name = '{id}'";
            try
            {
                List<User>? users;
                DataTable tabelUsers = GetDataTable(sql);
                users = tabelUsers.ToList<User>();
                if (users?.Any() ?? false)
                {
                    return users.First();
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task UpdateItemAsync(string id, T item)
        {
            throw new NotImplementedException();
        }

        private DataTable GetDataTable(string Query)
        {
            try
            {
                DataTable data = new();
                _connection.Open();
                SqlCommand command = new(Query, _connection);
                SqlDataReader reader = command.ExecuteReader();
                data.Load(reader);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

    }
}
