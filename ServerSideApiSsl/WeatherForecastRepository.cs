using System.Collections.Generic;
using WebApiSslCore;

public class WeatherForecasetRepository : IWeatherForecastRepository 
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private List<WeatherForecast> _users = new ()
        {
            new WeatherForecast
            {
                Date = DateTime.Now.AddDays(1),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Id = 1, Username = "peter", Password = "peter123"
            },
            new WeatherForecast
            {
                Date = DateTime.Now.AddDays(2),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Id = 2, Username = "joydip", Password = "joydip123"

            },
            new WeatherForecast
            {
                Date = DateTime.Now.AddDays(3),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Id = 3, Username = "james", Password = "james123"
            }
        };
    public async Task<bool> Authenticate(string username, string password)
    {
        if (await Task.FromResult(_users.SingleOrDefault(x => x.Username == username && x.Password == password)) != null)
        {
            return true;
        }
        return false;
    }
    public async Task<List<string>> GetUserNames()
    {
        List<string> users = new();
        foreach (var user in _users)
        {
            users.Add(user.Username);
        }
        return await Task.FromResult(users);
    }
}