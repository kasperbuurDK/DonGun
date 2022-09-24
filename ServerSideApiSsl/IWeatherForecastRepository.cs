using System.Collections.Generic;

namespace WebApiSslCore
{
    public interface IWeatherForecastRepository
    {
        Task<bool> Authenticate(string username, string password);
        Task<List<string>> GetUserNames();
    }
}
