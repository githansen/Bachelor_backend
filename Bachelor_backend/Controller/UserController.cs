using Bachelor_backend.DAL.Repositories;
using Bachelor_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Bachelor_backend.Controller
{
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private const string _loggedIn = "UserSession";

        private readonly IVoiceRepository _voiceRep;
        private readonly ITextRepository _textRep;

        private readonly ILogger<UserController> _logger;

        public UserController(IVoiceRepository voiceRep, ITextRepository textRep, ILogger<UserController> logger)
        {
            _voiceRep = voiceRep;
            _textRep = textRep;
            _logger = logger;
        }


        [HttpPost]
        public async Task<ActionResult> SaveFile(IFormFile recording)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized("Not logged in");
            }

            string uuid = await _voiceRep.SaveFile(recording);
            if (uuid.IsNullOrEmpty())
            {
                _logger.LogInformation("Fault in saving voice recording");
                return BadRequest("Voice recording is not saved");
            }
            return Ok(uuid);

        }

        public async Task<ActionResult<bool>> DeleteFile(string uuid)
        {
            bool deleted = await _voiceRep.DeleteFile(uuid);
            return Ok(deleted);
        }
        //Get text based on session value, discuss later
        [HttpPost]
        public async Task<ActionResult> GetText([FromBody] User user)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized();
            }
            Text t = await _textRep.GetText(user);
            return Ok(t);
        }

        //Login a good name? 
        [HttpPost]
        public async Task<ActionResult> GetUserInfo([FromBody] User user)
        {
            var userFromDb = await _textRep.GetUserInfo(user);

            //TODO: Return user id from db
            HttpContext.Session.SetString(_loggedIn, userFromDb.UserId.ToString());
            return Ok("Ok");
        }
    }
}
