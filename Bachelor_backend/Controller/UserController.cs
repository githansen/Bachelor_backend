using Bachelor_backend.DAL.Repositories;
using Bachelor_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;

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
        public async Task<ActionResult> SaveFile(IFormFile recording, int textId)
        {
            var sessionString = HttpContext.Session.GetString(_loggedIn);
            if (string.IsNullOrEmpty(sessionString))
            {
                return Unauthorized();
            }

            string uuid = await _voiceRep.SaveFile(recording, textId, int.Parse(Regex.Match(sessionString, @"\d+").Value));
            
            if (uuid.IsNullOrEmpty())
            {
                _logger.LogInformation("Fault in saving voice recording");
                return StatusCode(500, "Voice recording is not saved");
            }

            if (uuid.Equals("File extension not allowed"))
            {
                _logger.LogInformation("File extension not allowed");
                return BadRequest("File extension not allowed");
            }

            if (uuid.Equals("Audiofile is too big"))
            {
                _logger.LogInformation("Audiofile is too big");
                return BadRequest("Audiofile is too big");
            }
            return Ok(uuid);

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteFile([FromBody] string uuid)
        {
            bool deleted = await _voiceRep.DeleteFile(uuid);

            if (!deleted)
            {
                _logger.LogInformation("Fault in deleting voice recording");
                return BadRequest("Voice recording is not deleted");
            }

            return Ok("Voice recording is deleted");
        }

        //Get text based on session value, discuss later
        [HttpPost]
        public async Task<ActionResult> GetText()
        {
            string sessionString = HttpContext.Session.GetString(_loggedIn);
            if (string.IsNullOrEmpty(sessionString))
            {
                return Unauthorized();
            }

            int userId = int.Parse(Regex.Match(sessionString, @"\d+").Value);
            var user = await _textRep.GetUser(userId);
            var text = await _textRep.GetText(user);
            return Ok(text);
        }

        //Login a good name? 
        [HttpPost]
        public async Task<ActionResult> RegisterUserInfo([FromBody] User user)
        {
            //Save yser info in db and returns user with id
            var userFromDb = await _textRep.RegisterUserInfo(user);
            
            HttpContext.Session.SetString(_loggedIn, userFromDb.UserId.ToString());
            return Ok("Ok");
        }
    }
}
