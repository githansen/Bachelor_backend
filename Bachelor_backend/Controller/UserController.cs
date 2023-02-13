﻿using Bachelor_backend.DAL.Repositories;
using Bachelor_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Numerics;
using System.Text.RegularExpressions;
using Microsoft.Net.Http.Headers;

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
        /// <response code="500">Error while saving file</response>
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Voice recording is not saved");
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

            int userId = int.Parse(Regex.Match(sessionString, @"\d+").Value);
            var user = await _textRep.GetUser(userId);
            var text = await _textRep.GetText(user);
            return Ok(text);
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
        /// <response code="200"> Userinfo saved to database, returns true </response>
        /// <response code="500">Error on server, returns false</response>
        [HttpPost]
        public async Task<ActionResult> RegisterUserInfo([FromBody] User user)
        {
            //Save yser info in db and returns user with id
            var userFromDb = await _textRep.RegisterUserInfo(user);
            //TODO: Return user id from db
            if (userFromDb != null)
            {
                HttpContext.Session.SetString(_loggedIn, userFromDb.UserId.ToString());
                return Ok(true);
            }
            else
            {
                _logger.LogInformation("Error while creating user");
                return StatusCode(StatusCodes.Status500InternalServerError, false);
            }
        }

        public bool IsLoggedIn()
        {
            string sessionString = HttpContext.Session.GetString(_loggedIn);
            if (string.IsNullOrEmpty(sessionString))
            {
                return false;
            }
            return true;
        }
        
        public HttpResponseMessage SetCookie()
        {
            //Get user id from session
            string sessionString = HttpContext.Session.GetString(_loggedIn);
            if(sessionString != null)
            {
                return new HttpResponseMessage (HttpStatusCode.BadRequest);
            }
            //Create cookie
            var response = new HttpResponseMessage();

            var cookie = new CookieOptions();
            cookie.Expires = DateTimeOffset.Now.AddMonths(1); //Expires in 1 month
            cookie.Path = "/";
            
            HttpContext.Response.Cookies.Append("userid", sessionString, cookie);
            return response;
        }
        
        //TODO GetCookie
    }
}
