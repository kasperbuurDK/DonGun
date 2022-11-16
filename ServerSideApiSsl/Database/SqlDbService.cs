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
using System.Web;
using DevExpress.DirectX.NativeInterop.Direct2D.CCW;
using SharedClassLibrary.MessageStrings;
using System.Security.Cryptography;
using System.Drawing;
using System.Data.SqlTypes;
using DevExpress.XtraExport;

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
            string sql = "SELECT * FROM Users WHERE Name = @ContName";
            SqlCommand _cmd = new(sql, _connection);
            SqlParameter paramName = _cmd.Parameters.Add("@ContName", SqlDbType.NVarChar, -1);
            paramName.Value = username;
            paramName.Direction = ParameterDirection.Input;
            try
            {
                List<UserSalt>? users = GetDataTable(_cmd).ToList<UserSalt>();
                if (users?.Any() ?? false)
                {
                    UserSalt u = users.First();
                    if (u.CompareHashed(u.SaltedHash(password)))
                        return true;
                }
                throw new Exception();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                _connection.Close();
            }
        }

        private DataTable GetDataTable(SqlCommand command)
        {
            try
            {
                _connection.Open();
                DataTable data = new();
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

        private int SetDataTable(SqlCommand command)
        {
            try
            {
                _connection.Open();
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
            string sql = "SELECT * FROM Users WHERE [Name] = @ContName";
            SqlCommand _cmd = new(sql, _connection);
            SqlParameter paramName = _cmd.Parameters.Add("@ContName", SqlDbType.NVarChar, -1);
            paramName.Value = name;
            paramName.Direction = ParameterDirection.Input;
            try
            {
                List<UserSalt>? users = GetDataTable(_cmd).ToList<UserSalt>();
                if (users?.Any() ?? false)
                {
                    return (User)users.First();
                }
                throw new Exception();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int CreateUser(User u)
        {
            var cmdCheck = new SqlCommand("SELECT * FROM Users WHERE [Name] = @ContName", _connection);
            SqlParameter paramName = new()
            {
                ParameterName = "@ContName",
                Value = u.Name,
                SqlDbType = SqlDbType.NVarChar,
                Size = -1,
                Direction = ParameterDirection.Input
            };
            cmdCheck.Parameters.Add(paramName);

            UserSalt uSalt = u.DownCast<User, UserSalt>();
            uSalt.Salt = UserSalt.CreateSalt();
            var cmdInsert = new SqlCommand("INSERT INTO Users ([Name], [HashedPass], [Salt]) VALUES (@ContName, @ContPass, @ContSalt)", _connection);
            SqlParameter paramPass = new()
            {
                ParameterName = "@ContPass",
                Value = uSalt.SaltedHash(uSalt.Password),
                SqlDbType = SqlDbType.VarBinary,
                Size = 50,
                Direction = ParameterDirection.Input
            };
            SqlParameter paramSalt = new()
            {
                ParameterName = "@ContSalt",
                Value = uSalt.Salt,
                SqlDbType = SqlDbType.VarBinary,
                Size = 50,
                Direction = ParameterDirection.Input
            };
            SqlParameter paramNameIn = new()
            {
                ParameterName = "@ContName",
                Value = u.Name,
                SqlDbType = SqlDbType.NVarChar,
                Size = -1,
                Direction = ParameterDirection.Input
            };
            cmdInsert.Parameters.Add(paramSalt);
            cmdInsert.Parameters.Add(paramPass);
            cmdInsert.Parameters.Add(paramNameIn);
            SqlTransaction? CTx = null;
            try
            {
                _connection.Open();
                CTx = _connection.BeginTransaction();
                cmdCheck.Transaction = CTx;
                cmdInsert.Transaction = CTx;
                // Does the DB containe any user with this username?
                DataTable data = new();
                data.Load(cmdCheck.ExecuteReader());
                List<UserSalt>? users = data.ToList<UserSalt>();
                if (users?.Any() ?? false)
                    throw new Exception();

                int rowsAffected = cmdInsert.ExecuteNonQuery();
                if (rowsAffected >= 1)
                {
                    CTx.Commit();
                    return (int)HttpStatusCode.OK;
                }
                throw new Exception();
            }
            catch (Exception)
            {
                CTx?.Rollback();
                return (int)HttpStatusCode.BadRequest;
            }
            finally
            {
                _connection.Close();
            }
        }

        public int DeleteUser(User u)
        {
            var cmdDeleteUser = new SqlCommand("DELETE FROM Users WHERE [Name] = @ContName", _connection);
            SqlParameter paramName = new()
            {
                ParameterName = "@ContName",
                Value = u.Name,
                SqlDbType = SqlDbType.NVarChar,
                Size = -1,
                Direction = ParameterDirection.Input
            };
            cmdDeleteUser.Parameters.Add(paramName);

            var cmdDeleteSheets = new SqlCommand("DELETE FROM Sheets WHERE [User] = @ContUserId", _connection);
            SqlParameter paramUserId = new()
            {
                ParameterName = "@ContUserId",
                Value = u.Id,
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input
            };
            cmdDeleteSheets.Parameters.Add(paramUserId);

            SqlTransaction? CTx = null;
            try
            {
                _connection.Open();
                CTx = _connection.BeginTransaction();
                cmdDeleteSheets.Transaction = CTx;
                cmdDeleteUser.Transaction = CTx;
                cmdDeleteSheets.ExecuteNonQuery();
                int rowsAffected = cmdDeleteUser.ExecuteNonQuery();
                if (rowsAffected >= 1)
                {
                    CTx.Commit();
                    return (int)HttpStatusCode.OK;
                }
                throw new Exception();
            }
            catch (Exception)
            {
                CTx?.Rollback();
                return (int)HttpStatusCode.BadRequest;
            }
        }

        public Player? GetSheet(int id)
        {
            string sql = "SELECT * FROM Sheets WHERE [Id] = @ContId";
            SqlCommand _cmd = new(sql, _connection);
            SqlParameter paramId = _cmd.Parameters.Add("@ContId", SqlDbType.Int);
            paramId.Value = id;
            paramId.Direction = ParameterDirection.Input;
            try
            {
                List<Sheet>? sheets = GetDataTable(_cmd).ToList<Sheet>();
                if (sheets?.Any() ?? false)
                {
                    Sheet sheet = sheets.First();
                    return sheet.Data?.JsonToType<Player>();
                }
                throw new Exception();
            }
            catch (Exception)
            {
                Player? playerSheet = default;
                return playerSheet;
            }
        }

        public Dictionary<int, Player> GetSheets(int id)
        {
            string sql = "SELECT * FROM Sheets WHERE [User] = @ContId";
            SqlCommand _cmd = new(sql, _connection);
            SqlParameter paramId = _cmd.Parameters.Add("@ContId", SqlDbType.Int);
            paramId.Value = id;
            paramId.Direction = ParameterDirection.Input;
            try
            {
                List<Sheet>? sheets = GetDataTable(_cmd).ToList<Sheet>();
                if (sheets?.Any() ?? false)
                {
                    Dictionary<int, Player>? playerSheets = new();
                    foreach (Sheet sheet in sheets)
                    {
                        Player? player = sheet.Data?.JsonToType<Player>();
                        if (player is not null)
                            playerSheets.Add(sheet.Id, player);
                    }
                    return playerSheets;
                }
                throw new Exception();
            }
            catch (Exception)
            {
                Dictionary<int, Player>? playerSheets = new();
                return playerSheets;
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

            string sql = "INSERT INTO Sheets ([User], [Data], [Type]) VALUES (@ContUser, @ContData, @ContType)";
            SqlCommand _cmd = new(sql, _connection);
            SqlParameter paramUser = _cmd.Parameters.Add("@ContUser", SqlDbType.Int);
            paramUser.Value = user;
            paramUser.Direction = ParameterDirection.Input;
            SqlParameter paramData = _cmd.Parameters.Add("@ContData", SqlDbType.NVarChar, -1);
            paramData.Value = jsonData;
            paramData.IsNullable = true;
            paramData.Direction = ParameterDirection.Input;
            SqlParameter paramType = _cmd.Parameters.Add("@ContType", SqlDbType.Int);
            paramType.Value = (int)type;
            paramType.Direction = ParameterDirection.Input;
            try
            {
                int rowsAffected = SetDataTable(_cmd);
                if ( rowsAffected >= 1 )
                {
                    return (int)HttpStatusCode.OK;
                }
                throw new Exception();
            }
            catch (Exception)
            {
                return (int)HttpStatusCode.BadRequest;
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

            string sql = "UPDATE Sheets SET [Data] = @ContData, [Type] = @ContType WHERE [Id] = @ContId";
            SqlCommand _cmd = new(sql, _connection);
            SqlParameter paramId = _cmd.Parameters.Add("@ContId", SqlDbType.Int);
            paramId.Value = id;
            paramId.Direction = ParameterDirection.Input;
            SqlParameter paramData = _cmd.Parameters.Add("@ContData", SqlDbType.NVarChar, -1);
            paramData.Value = jsonData;
            paramData.IsNullable = true;
            paramData.Direction = ParameterDirection.Input;
            SqlParameter paramType = _cmd.Parameters.Add("@ContType", SqlDbType.Int);
            paramType.Value = (int)type;
            paramType.Direction = ParameterDirection.Input;
            try
            {
                int rowsAffected = SetDataTable(_cmd);
                if (rowsAffected >= 1)
                {
                    return (int)HttpStatusCode.OK;
                }
                throw new Exception();
            }
            catch (Exception)
            {
                return (int)HttpStatusCode.BadRequest;
            }
        }

        public int DeleteSheet(int id)
        {
            string sql = "DELETE FROM Sheets WHERE [Id] = @ContId";
            SqlCommand _cmd = new(sql, _connection);
            SqlParameter paramId = _cmd.Parameters.Add("@ContId", SqlDbType.Int);
            paramId.Value = id;
            paramId.Direction = ParameterDirection.Input;
            try
            {
                int rowsAffected = SetDataTable(_cmd);
                if (rowsAffected >= 1)
                {
                    return (int)HttpStatusCode.OK;
                }
                throw new Exception();
            }
            catch (Exception)
            {
                return (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
