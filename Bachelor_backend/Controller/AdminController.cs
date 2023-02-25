﻿using Bachelor_backend.DAL.Repositories;
using Bachelor_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace Bachelor_backend.Controller
{
    [Route("[controller]/[action]")]
    public class AdminController : ControllerBase
    {
        private readonly ITextRepository _textRep;
        private readonly IVoiceRepository _voicerep;
        public AdminController(ITextRepository textrep, IVoiceRepository voicerep)
        {
            _textRep = textrep;
            _voicerep = voicerep;
        }

        /// <summary>
        /// Login as admin
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        public async Task<ActionResult> LogIn()
        {
            //bool success = await _textRep.login();
            throw new NotImplementedException();
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
        public async Task<ActionResult> CreateTag([FromQuery]string text)
        {
            var regex = new Regex("^([a-zA-ZæøåÆØÅ]{2,20})$");
            if (!regex.IsMatch(text))
            {
                return BadRequest("Fault in input");
            }
            
            bool success = await _textRep.CreateTag(text);
            if (success)
            {
                return Ok(true);
            }
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
            if (ModelState.IsValid)
            {
                bool success = await _textRep.CreateText(text);
                if(success)
                {
                    return Ok(true);
                }
                
                return StatusCode(StatusCodes.Status500InternalServerError, false);
                }

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
            
            bool success = await _textRep.DeleteTag(TagId);
            if (success)
            {
                return Ok(true);
            }
            return BadRequest(false);
        }

        [ApiExplorerSettings(IgnoreApi = true)]

        [HttpGet]
        public async Task<ActionResult> GetNumberOfTexts()
        {
            int total = await _textRep.GetNumberOfTexts();
            if(total > -1)
            {
                return Ok(total);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, -1);

            }
        }
        [ApiExplorerSettings(IgnoreApi = true)]

        [HttpGet]
        public async Task<ActionResult> GetNumberOfRecordings(){
            int total = await _voicerep.GetNumberOfRecordings();
            if (total > -1)
            {
                return Ok(total);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, -1);
            }
        }
        [ApiExplorerSettings(IgnoreApi = true)]

        [HttpGet]
        public async Task<ActionResult> GetNumberOfUsers()
        {
            int total = await _textRep.GetNumberOfUsers();
            if(total > -1) {
                return Ok(total);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, -1);

            }
        }
        [ApiExplorerSettings(IgnoreApi = true)]

        [HttpGet]
        public async Task<ActionResult> GetOneText(int id) 
        {
            Text text = await _textRep.GetOneText(id);
            if(text != null)
            {
                return Ok(text);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, null);
            }

        }
        [ApiExplorerSettings(IgnoreApi = true)]

        [HttpGet]
        public async Task<ActionResult> GetNumberOfDeletedRecordings() { throw new NotImplementedException(); }

        [HttpGet]
        public async Task<ActionResult> GetAllRecordings() {
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
        public async Task<ActionResult> GetOneRecording(string uuid) {
            IFormFile file = await _voicerep.GetOneRecording(uuid);
            if(file != null)
            {
                return Ok(file);
            }
            else
            {
                return BadRequest(null);
            }
        }

        [HttpGet]
        public async Task<ActionResult> test()
        {
            Dictionary<string, int> t = new Dictionary<string, int>();
            t.Add("Mann", 25);
            t.Add("Kvinne", 30);
            t.Add("Annet", 55);
            t.Add("Ukjent", 45);
            return Ok(t);
        }
    }
}
