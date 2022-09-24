
using SharedClassLibrary;

namespace ServerSideApiSsl
{
    public interface IWeatherForecastRepository
    {
        Task<bool> Authenticate(string username, string password);
        Task<Character_abstract> GetUserNames(string username);
    }
}
