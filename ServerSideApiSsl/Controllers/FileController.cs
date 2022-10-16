using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServerSideApiSsl.Database;
using SharedClassLibrary;
using System.Net;

namespace ServerSideApiSsl.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FileController : ControllerBase
    {
        private readonly ISqlDbService<Player> _userRepository;
        private readonly ILogger<FileController> _logger;

        public FileController(ISqlDbService<Player> userRepository, ILogger<FileController> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        private bool HasAccess(string name)
        {
            return Request.HttpContext.User.Claims.Any(c => c.Type == "name" && c.Value == name);
        }

        [Authorize]
        [HttpGet("{name}")]
        public List<User> GetUser(string name)
        {
            List<User> ret = new();
            if (HasAccess(name))
            {
                User? user = _userRepository.GetUser(name);
                if (user is not null)
                    ret.Add(user);
                else
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return ret;
        }

        [Authorize]
        [HttpGet("{name}/{id}")]
        public Player GetSheet(string name, int id)
        {
            Player ret = new();
            if (HasAccess(name))
            {
                Player? sheet = _userRepository.GetSheet(id);
                if (sheet is not null)
                    ret = sheet;
                else
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return ret;
        }

        [Authorize]
        [HttpGet("{name}")]
        public Dictionary<int, Player> GetSheet(string name)
        {
            Dictionary<int, Player> ret = new();
            if (HasAccess(name))
            {
                User? user = _userRepository.GetUser(name);
                if (user is not null)
                {
                    Dictionary<int, Player> sheets = _userRepository.GetSheets(user.Id);
                    if (!sheets.IsNullOrEmpty())
                        ret = sheets;
                    else
                        Response.StatusCode = (int)HttpStatusCode.NoContent;
                }
                else
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return ret;
        }

        [Authorize]
        [HttpPut("{name}")]
        public IActionResult SetPlayerSheet([FromBody]Player p, string name)
        {
            if (HasAccess(name))
            {
                User? user = _userRepository.GetUser(name);
                if (user is not null) {
                    int statusCode = _userRepository.PutSheet(user.Id, p);
                    if (statusCode == (int)HttpStatusCode.Accepted)
                        return Accepted(name);
                    else
                        return BadRequest(name);
                } else
                    return NotFound(name);
            }
            else
                return Unauthorized(name);
        }

        [Authorize]
        [HttpPost("{name}/{id}")]
        public IActionResult SetPlayerSheet([FromBody] Player p, string name, int id)
        {
            if (HasAccess(name))
            {
                int statusCode = _userRepository.PostSheet(id, p);
                if (statusCode == (int)HttpStatusCode.OK)
                    return Ok(id);
                else
                    return BadRequest(id);
            }
            else
                return Unauthorized(name);
        }

        [Authorize]
        [HttpDelete("{name}/{id}")]
        public IActionResult SetPlayerSheet(string name, int id)
        {
            if (HasAccess(name))
            {
                int statusCode = _userRepository.DeleteSheet(id);
                if (statusCode == (int)HttpStatusCode.OK)
                    return Ok(id);
                else
                    return BadRequest(id);
            }
            else
                return Unauthorized(name);
        }
    }
}
