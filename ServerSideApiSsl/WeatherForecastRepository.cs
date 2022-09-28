using SharedClassLibrary;
using System.Drawing.Imaging;
using System.Numerics;

namespace ServerSideApiSsl
{
    public class WeatherForecasetRepository : IWeatherForecastRepository
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private List<WeatherForecast> _users = new()
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
                Id = 2, Username = "user", Password = "password"

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
            // Make auth reqv. to database here.
            // If database contains 
            if (await Task.FromResult(_users.SingleOrDefault(x => x.Username == username && x.Password == password)) != null)
            {
                return true;
            }
            return false;
        }
        public async Task<Npc> GetUserNames(string username)
        {
            // Pull chara sheet from database given username.
            Npc chara = new Npc();
            foreach (var user in _users)
            {
                if (user.Username.Equals(username))
                    chara = new Npc() {Charisma = user.Id, Constitution = user.GetHashCode(), Dexterity = user.TemperatureC, Health = user.Id+10, Resource = user.Id+20 };
            }

            return await Task.FromResult(chara);
        }
    }
}