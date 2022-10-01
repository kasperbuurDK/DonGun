
using SharedClassLibrary;

namespace ServerSideApiSsl
{
    public interface IWeatherForecastRepository
    {
        Task<bool> Authenticate(string username, string password);
        Task<Npc> GetUserNames(string username);
    }
}
