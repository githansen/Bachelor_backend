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

        /// <summary>
        /// Saves recording to database
        /// </summary>
        /// <param name="recording"></param>
        /// <returns></returns>
        /// <response code="401">Not authorized</response>"
        /// <response code="200">Successfully saved file</response>
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
        /// <summary>
        /// Deletes recording
        /// </summary>
        /// <param name="uuid"></param>
        /// <remarks>
        /// Argument needs to be a uuid. 
        /// Example: 123e4567-e89b-12d3-a456-426614174000
        /// </remarks>
        /// <returns>String describing whether or not deletion was successful</returns>
        /// <response code="200">Deletion succeeded </response>
        /// <response code="400">Deletion unsuccessful - likely non existent uuid</response>

        [HttpDelete]
        public async Task<ActionResult> DeleteFile([FromQuery] string uuid)
        {
            bool deleted = await _voiceRep.DeleteFile(uuid);

            if (!deleted)
            {
                _logger.LogInformation("Fault in deleting voice recording");
                return BadRequest("Voice recording is not deleted");
            }

            return Ok("Voice recording is deleted");
        }

        /// <summary>
        /// GET text
        /// </summary>
        /// <remarks>
        /// No parameters needed, uses Session string to find text
        /// </remarks>
        /// <response code="401">Not Authorized</response>
        /// <response code="200">OK, returns text</response>
        [HttpPost]
        public async Task<ActionResult> GetText()
        {
            string sessionString = HttpContext.Session.GetString(_loggedIn);
            if (string.IsNullOrEmpty(sessionString))
            {
                return Unauthorized();
            }

            int UserId = int.Parse(Regex.Match(sessionString, @"\d+").Value);
            Debug.Write(UserId);
            User user = await _textRep.GetUser(UserId);
            Text t = await _textRep.GetText(user);
            return Ok(t);
        }

        ///<summary>Post user data</summary> 
        ///<param Name="user"></param>
        ///<remarks>
        ///Needs user data as parameter
        ///Dont include UserId - generated on creation
        /// Example:
        /// user = 
        /// {
        /// "nativeLanguage": "Norsk",
        /// "dialect": "Østlandsk",
        /// "ageGroup":"18-28"
        /// }
        /// </remarks>
        /// <response code="200"> Userinfo saved to database </response>
        [HttpPost]
        public async Task<ActionResult> RegisterUserInfo([FromBody] User user)
        {
            var userFromDb = await _textRep.RegisterUserInfo(user);

            //TODO: Return user id from db
            HttpContext.Session.SetString(_loggedIn, userFromDb.UserId.ToString());
            return Ok("Ok");
        }
    }
}
