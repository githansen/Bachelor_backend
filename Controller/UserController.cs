using Bachelor_backend.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Bachelor_backend.Controller
{
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {

        private readonly IVoiceRepository _db;

        public UserController(IVoiceRepository db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<ActionResult<string>> SaveFile(IFormFile recording)
        {

            string uuid = await _db.SaveFile(recording);
            if (uuid.Equals(null))
            {
                return BadRequest("Voice recording is not saved");
            }
            return Ok(uuid);
            
        }

    }
}
