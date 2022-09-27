using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerSideApiSsl;
using SharedClassLibrary;

namespace ServerSideApiSsl.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastRepository _userRepository;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IWeatherForecastRepository userRepository, ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<Character_abstract> Get(string id)
        {
            return await _userRepository.GetUserNames(id);
        }
    }
}