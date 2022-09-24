namespace WebApiSslCore
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}