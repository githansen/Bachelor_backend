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
        private readonly DatabaseContext t;
        private readonly ILogger<UserController> _logger;

        public UserController(IVoiceRepository voiceRep, DatabaseContext t, ILogger<UserController> logger)
        {
            _voiceRep = voiceRep;
            t = t;
            _logger = logger
        }
        [HttpGet]
        public List<Text> test()
        {
            List<Text> liste = t.Texts.ToList();
            return liste;
        }
        [HttpPost]
        public async Task<ActionResult<string>> SaveFile(IFormFile recording)
        {

            string uuid = await _voiceRep.SaveFile(recording);
            if (uuid.IsNullOrEmpty())
            {
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
        public async Task<ActionResult> GetText()
        {
            throw new NotImplementedException();
        }

        //Login a good name? 
        [HttpPost]
        public async Task<ActionResult> Login()
        {
            throw new NotImplementedException();
        }
    }
}
