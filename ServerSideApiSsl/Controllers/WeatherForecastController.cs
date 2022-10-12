using DevExpress.Internal;
using Microsoft.AspNetCore.Authorization;
using System.Web;
using Microsoft.AspNetCore.Identity;
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
        //[RequireHttps]
        [HttpGet("{id}")]
        public async Task<List<Npc>> Get(string id)
        {
            List<Npc> ret = new();
            var hasAccess = Request.HttpContext.User.Claims.Any(c => c.Type == "name" && c.Value == id);
            if (hasAccess)
                ret.Add(await _userRepository.GetUserNames(id));
            else
                Response.StatusCode = 401;
            return ret;
        }

        
    }
}