using SharedClassLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerSideApiSsl.Database
{
    public interface ISqlDbService<T>
    {
        bool Authenticate(string username, string password);
        User? GetUser(string name);
        Player? GetSheet(int id);
        Dictionary<int, Player> GetSheets(int id);
        int PutSheet(int user, object data);
        public int PostSheet(int id, object data);
        public int DeleteSheet(int id);
        public int CreateUser(User u);
        public int DeleteUser(User u);
    }
}
