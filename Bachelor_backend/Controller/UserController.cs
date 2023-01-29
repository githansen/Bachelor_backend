using Bachelor_backend.DAL;
using Bachelor_backend.DAL.Repositories;
using Bachelor_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Bachelor_backend.Controller
{
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IVoiceRepository _voiceRep;

        private readonly ILogger<UserController> _logger;
        private readonly ITextRepository _text;

        public UserController(IVoiceRepository voiceRep, ITextRepository text, ILogger<UserController> logger)
        {
            _voiceRep = voiceRep;
            _logger = logger;
            _text = text;
        }

      
        [HttpPost]
        public async Task<ActionResult<string>> SaveFile(IFormFile recording)
        {

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
        [HttpGet]
        public async Task<ActionResult> GetText( )
        {
            User u = new User()
            {
                AgeGroup= "18-29",
                NativeLanguage="English"
            };
            Text t = await _text.GetText(u);
            return Ok(t);
        }

        //Login a good name? 
        [HttpPost]
        public async Task<ActionResult> Login()
        {
            throw new NotImplementedException();
        }
    }
}
