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
using SharedClassLibrary.MessageStrings;
using System.Security.Cryptography;

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
            string sql = $"SELECT Id FROM Users WHERE Name = '{username}'";
            try
            {
                List<User>? users = GetDataTable(sql).ToList<User>();
                if (users?.Any() ?? false)
                {
                    return VerifyHashPass(users.First().Password, password);
                }
                return false;
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

        public int CreateUser(User u)
        {
            string sql = $"SELECT * FROM Users WHERE [Name] = '{u.Name}'";
            try
            {
                // Does the DB containe any user with this username?
                List<User>? users = GetDataTable(sql).ToList<User>();
                if (users?.Any() ?? false)
                {
                    return (int)HttpStatusCode.Conflict;
                }

                sql = $"INSERT INTO Users ([Name], [Password]) VALUES ('{u.Name}', '{HashPass(u.Password)}')";
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
            catch (Exception)
            {
                throw;
            }
        }

        public int DeleteUser(User u)
        {
            string sql = $"DELETE FROM Users WHERE [Name] = '{u.Name}'";
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
        private static string HashPass(string password)
        {
            byte[] salt, buf, dst;
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            using (Rfc2898DeriveBytes bytes = new(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buf = bytes.GetBytes(0x20);
            }
            dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buf, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        private static bool VerifyHashPass(string hash, string password)
        {
            byte[] src, buf, tempBuf, dst;
            if (string.IsNullOrEmpty(hash))
            {
                return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }
            
            src = Convert.FromBase64String(hash);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }

            dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            buf = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buf, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new(password, dst, 0x3e8))
            {
                tempBuf = bytes.GetBytes(0x20);
            }
            return buf.SequenceEqual(tempBuf);
        }
    }
}
