using Bachelor_backend.DAL.Repositories;
using Bachelor_backend.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using Microsoft.IdentityModel.Tokens;

namespace Bachelor_backend.Controller
{
    [Route("[controller]/[action]")]
    public class AdminController : ControllerBase
    {
        private readonly ITextRepository _textRep;
        private readonly IVoiceRepository _voicerep;
        private readonly ISecurityRepository _security;
        private readonly ILogger<AdminController> _logger;
        
        private const string _loggedIn = "AdminSession";
        private const string _notLoggedIn = "";
        
        public AdminController(ITextRepository textrep,IVoiceRepository voicerep, ISecurityRepository security, ILogger<AdminController> logger)
        {
            _textRep = textrep;
            _voicerep = voicerep;
            _security = security;
            _logger = logger;
        }

        /// <summary>
        /// Login as admin
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<ActionResult> LogIn([FromBody] AdminUser user)
        {
            if (ModelState.IsValid)
            {
                var success = await _security.Login(user);

                if (success)
                {
                    HttpContext.Session.SetString(_loggedIn, user.Username);
                    return Ok(true);
                }
                return Unauthorized("Wrong username or password");
            }
            _logger.LogInformation("Fault in input");
            return BadRequest("Fault in input");

        }
        
        [HttpGet]
        public async Task<ActionResult> LogOut()
        {
            HttpContext.Session.SetString(_loggedIn, _notLoggedIn);
            return Ok(true);
        }

        [HttpPost]
        public async Task<ActionResult> RegisterAdmin([FromBody] AdminUser user)
        {
            var sessionString = HttpContext.Session.GetString(_loggedIn);

            if (sessionString.IsNullOrEmpty())
            {
                return Unauthorized();
            }

            var regexPassword = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$");

            if (regexPassword.IsMatch(user.Password))
            {
                return BadRequest("Password must contain at least 8 characters, one uppercase, one lowercase, one number and one special character");
            }
            
            if (ModelState.IsValid)
            {
                var success = await _security.Register(user);
                if (success)
                {
                    return Ok(true);
                }

                _logger.LogInformation("Fault in registering admin");
                return BadRequest("Failed to register new admin");
            }
            _logger.LogInformation("Fault in input");
            return BadRequest("Fault in input");
        }

        /// <summary>
        /// Post tag
        /// </summary>
        /// <remarks>
        /// Needs a string with the tag-text. Example: "Vold"
        /// </remarks>
        /// <param name="text"></param>
        /// <response code="200">OK - creation successful</response>
        /// <response code="400">Error while creating tag</response>
        [HttpPost]
        public async Task<ActionResult> CreateTag([FromBody]string text)
        {
            var sessionString = HttpContext.Session.GetString(_loggedIn);
            if (sessionString.IsNullOrEmpty())
            {
                return Unauthorized();
            }

            bool success = await _textRep.CreateTag(text);
            if (success)
            {
                return Ok(true);
            }
            _logger.LogInformation("Fault in creating tag");
            return BadRequest(false);
        }
        
        /// <summary>
        /// Get all tags
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns list of tags</response>
        /// <response code="400">Returns empty list</response>"
        [HttpGet]
        public async Task<ActionResult> GetTags()
        {
            var sessionString = HttpContext.Session.GetString(_loggedIn);
            if (sessionString.IsNullOrEmpty())
            {
                return Unauthorized();
            }

            var list = await _textRep.GetAllTags();
            return Ok(list);
        }
        /// <summary>
        /// Post text
        /// </summary>
        /// <remarks>
        /// Needs text-object as argument
        /// Example: 
        /// text = 
        /// {
        /// "textText": "Jeg er uskyldig .... ..... . dette er teksten som skal leses opp......",
        /// "tags":
        ///     [
        ///         {
        ///             "tagText": "vold"
        ///         },
        ///         {
        ///             "tagText": "narkotika"
        ///         }
        ///     ],
        /// "targetUser": 
        ///     {
        ///         "nativeLanguage": "Norsk",
        ///         "ageGroup": "18-28",
        ///         "dialect": "østlandsk"
        ///     }
        /// }
        /// </remarks>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <response code="200">Successfully saved text, returns true</response>
        /// <response code="500">Failed, returns false</response>
        [HttpPost]
        public async Task<ActionResult> CreateText([FromBody] Text text)
        {
            var sessionString = HttpContext.Session.GetString(_loggedIn);
            if (sessionString.IsNullOrEmpty())
            {
                return Unauthorized();
            }
            
            if(ModelState.IsValid)
            {
                bool success = await _textRep.CreateText(text);
                if(success)
                {
                    return Ok(true);
                }
                _logger.LogInformation("Error in creating text");
                return StatusCode(StatusCodes.Status500InternalServerError, false);
            }
            _logger.LogInformation("Fault in input");
            return BadRequest("Fault in input");
        }
        
        /// <summary>
        /// Get all texts
        /// </summary>
        /// <returns></returns>
        /// <response code="200">OK - returns list of all texts</response>
        /// <response code="500">Returns empty list</response>
        [HttpGet]
        public async Task<ActionResult> GetAllTexts()
        {
            var sessionString = HttpContext.Session.GetString(_loggedIn);
            if (sessionString.IsNullOrEmpty())
            {
                return Unauthorized();
            }

            var list = await _textRep.GetAllTexts();
            if(list != null)
            {
                return Ok(list);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, list);
        }
        /// <summary>
        /// Delete text
        /// </summary>
        /// <param name="TextId"></param>
        /// <response code="200">OK - returns true, deletion successful</response>
        /// <response code="400">Bad request, returns false. Likely from sending non existent TextId</response>
        [HttpDelete]
        public async Task<ActionResult> DeleteText(int TextId)
        {
            var sessionString = HttpContext.Session.GetString(_loggedIn);
            if (sessionString.IsNullOrEmpty())
            {
                return Unauthorized();
            }
            
            bool success = await _textRep.DeleteText(TextId);
            if (success)
            {
                return Ok(true);
            }
            return BadRequest(false);
            }
        
        /// <summary>
        /// Delete tag
        /// </summary>
        /// <param name="TagId"></param>
        /// <response code="200">OK - returns true, deletion successful</response>
        /// <response code="400">Bad request, returns false. Likely from sending non existent TextId</response>
        [HttpDelete]
        public async Task<ActionResult> DeleteTag(int TagId)
        {
            var sessionString = HttpContext.Session.GetString(_loggedIn);
            if (sessionString.IsNullOrEmpty())
            {
                return Unauthorized();
            }

            bool success = await _textRep.DeleteTag(TagId);
            if (success)
            {
                return Ok(true);
            }
            return BadRequest(false);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditText([FromBody]Text text)
        {
            var sessionString = HttpContext.Session.GetString(_loggedIn);
            if (sessionString.IsNullOrEmpty())
            {
                return Unauthorized();
            }

            bool success = await _textRep.EditText(text);
            if (success)
            {
                return Ok(true);
            }
            return BadRequest(false);
        }
        
        [HttpPost]
        public async Task<ActionResult> EditTag([FromBody] Tag tag) 
        {
            if (ModelState.IsValid)
            {
            
                var sessionString = HttpContext.Session.GetString(_loggedIn);
                if (sessionString.IsNullOrEmpty())
                {
                    return Unauthorized();
                }
                
                var success = await _textRep.EditTag(tag);
                if (success)
                {
                    return Ok(true);
                }
                
                
                _logger.LogInformation("Error in editing tag");
                return BadRequest(false);
            }

            _logger.LogInformation("Fault in input");
            return BadRequest("Fault in input");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult> GetNumberOfTexts()
        {
            var sessionString = HttpContext.Session.GetString(_loggedIn);
            if (sessionString.IsNullOrEmpty())
            {
                return Unauthorized();
            }

            int total = await _textRep.GetNumberOfTexts();
            if(total > -1)
            {
                return Ok(total);
            }
            
            return StatusCode(StatusCodes.Status500InternalServerError, -1);
            }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult> GetNumberOfRecordings()
        {
            var sessionString = HttpContext.Session.GetString(_loggedIn);
            if (sessionString.IsNullOrEmpty())
            {
                return Unauthorized();
            }
            
            int total = await _voicerep.GetNumberOfRecordings();
            if (total > -1)
            {
                return Ok(total);
            }
            
            return StatusCode(StatusCodes.Status500InternalServerError, -1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult> GetNumberOfUsers()
        {
            var sessionString = HttpContext.Session.GetString(_loggedIn);
            if (sessionString.IsNullOrEmpty())
            {
                return Unauthorized();
            }
            
            int total = await _textRep.GetNumberOfUsers();
            if(total > -1) {
                return Ok(total);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, -1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult> GetOneText(int id) 
        {
            var sessionString = HttpContext.Session.GetString(_loggedIn);
            if (sessionString.IsNullOrEmpty())
            {
                return Unauthorized();
            }
            
            Text text = await _textRep.GetOneText(id);
            if(text != null)
            {
                return Ok(text);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, null);

        }

 
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAllRecordings()
        {
            List<Audiofile> list = await _voicerep.GetAllRecordings();
            if(list != null)
            {
                return Ok(list);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, null);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        public async Task<ActionResult> GetOneRecording(string uuid) 
        {
            var sessionString = HttpContext.Session.GetString(_loggedIn);
            if (sessionString.IsNullOrEmpty())
            {
                return Unauthorized();
            }

            IFormFile file = await _voicerep.GetOneRecording(uuid);
            if(file != null)
            {
                return Ok(file);
            }
            return BadRequest("Recording not found");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            var sessionString = HttpContext.Session.GetString(_loggedIn);
            if (sessionString.IsNullOrEmpty())
            {
                return Unauthorized();
            }
            
            var list = await _textRep.GetAllUsers();
            if(list != null)
            {
                return Ok(list);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, null);
        }
        
    }
}
