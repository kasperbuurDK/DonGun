using DevExpress.Internal;
using Microsoft.AspNetCore.Authorization;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerSideApiSsl;
using SharedClassLibrary;
using System.Net;

namespace ServerSideApiSsl.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ISqlDbService<Player> _userRepository;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ISqlDbService<Player> userRepository, ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpGet("{id}")]
        public List<User> Get(string id)
        {
            List<User> ret = new();
            var hasAccess = Request.HttpContext.User.Claims.Any(c => c.Type == "name" && c.Value == id);
            if (hasAccess)
            {
                User? user = _userRepository.GetUser(id);
                if (user is not null)
                {
                    ret.Add(user);
                } else
                    Response.StatusCode = (int)HttpStatusCode.NotFound;  
            } else
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return ret;
        }
    }
}