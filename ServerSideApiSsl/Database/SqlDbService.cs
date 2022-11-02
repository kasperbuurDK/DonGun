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
using System.Collections.Generic;
using System.Net;
using DevExpress.DirectX.NativeInterop.Direct2D.CCW;

namespace ServerSideApiSsl.Database
{
    public class SqlDbService<T> : ISqlDbService<T>
    {
        private readonly SqlConnection _connection;

        public enum TypeCode
        {
            Player,
            Npc
        }

        public SqlDbService(IOptionsSnapshot<SqlSettings> options)
        {
            if (options.Value.ConnectionString is not null)
                _connection = new SqlConnection(options.Value.ConnectionString);
            else
                throw new ArgumentNullException(nameof(options));
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
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _connection.Close();
            }
            return false;
        }

        private DataTable GetDataTable(string Query)
        {
            try
            {
                _connection.Open();
                DataTable data = new();
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

        private int SetDataTable(string Query)
        {
            try
            {
                _connection.Open();
                SqlCommand command = new(Query, _connection);
                return command.ExecuteNonQuery();
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

        public User? GetUser(string name)
        {
            string sql = $"SELECT * FROM Users WHERE [Name] = '{name}'";
            try
            {
                List<User>? users = GetDataTable(sql).ToList<User>();
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
        }

        public Player? GetSheet(int id)
        {
            string sql = $"SELECT * FROM Sheets WHERE [Id] = '{id}'";
            try
            {
                Player? playerSheet = default;
                List<Sheet>? sheets = GetDataTable(sql).ToList<Sheet>();
                if (sheets?.Any() ?? false)
                {
                    Sheet sheet = sheets.First();
                    playerSheet = sheet.Data?.JsonToType<Player>();
                }
                return playerSheet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<int, Player> GetSheets(int id)
        {
            string sql = $"SELECT * FROM Sheets WHERE [User] = '{id}'";
            try
            {
                Dictionary<int, Player>? playerSheets = new();
                List<Sheet>? sheets = GetDataTable(sql).ToList<Sheet>();
                if (sheets?.Any() ?? false)
                {
                    foreach (Sheet sheet in sheets)
                    {
                        Player? player = sheet.Data?.JsonToType<Player>();
                        if (player is not null)
                            playerSheets.Add(sheet.Id, player);
                    }
                }
                return playerSheets;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int PutSheet(int user, object data)
        {
            TypeCode type;
            string jsonData;
            if (data is Player p)
            {
                type = TypeCode.Player;
                jsonData = p.TypeToJson();
            } else if (data is Npc n)
            {
                type = TypeCode.Npc;
                jsonData = n.TypeToJson();
            } else
            {
                throw new ArgumentException("Not a valid object type", nameof(data));
            }

            string sql = $"INSERT INTO Sheets ([User], [Data], [Type]) VALUES ({user}, '{jsonData}', {(int)type})";
            try
            {
                int rowsAffected = SetDataTable(sql);
                if ( rowsAffected >= 1 )
                {
                    return (int)HttpStatusCode.OK;
                }
                return (int)HttpStatusCode.BadRequest;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int PostSheet(int id, object data)
        {
            TypeCode type;
            string jsonData;
            if (data is Player p)
            {
                type = TypeCode.Player;
                jsonData = p.TypeToJson();
            }
            else if (data is Npc n)
            {
                type = TypeCode.Npc;
                jsonData = n.TypeToJson();
            }
            else
            {
                throw new ArgumentException("Not a valid object type", nameof(data));
            }

            string sql = $"UPDATE Sheets SET [Data] = '{jsonData}', [Type] = {(int)type} WHERE [Id] = {id}";
            try
            {
                int rowsAffected = SetDataTable(sql);
                if (rowsAffected >= 1)
                {
                    return (int)HttpStatusCode.OK;
                }
                return (int)HttpStatusCode.BadRequest;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int DeleteSheet(int id)
        {
            string sql = $"DELETE FROM Sheets WHERE [Id] = '{id}'";
            try
            {
                int rowsAffected = SetDataTable(sql);
                if (rowsAffected >= 1)
                {
                    return (int)HttpStatusCode.OK;
                }
                return (int)HttpStatusCode.BadRequest;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
