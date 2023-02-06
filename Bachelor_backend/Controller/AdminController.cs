using Bachelor_backend.DAL.Repositories;
using Bachelor_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace Bachelor_backend.Controller
{
    [Route("[controller]/[action]")]
    public class AdminController : ControllerBase
    {
        private readonly ITextRepository _textRep;
        public AdminController(ITextRepository textrep)
        {
            _textRep = textrep;
        }
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
            bool success = await _textRep.CreateTag(text);
            if (success)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest(false);
            }
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
        ///             tagText: "vold"
        ///         },
        ///         {
        ///             tagText: "narkotika"
        ///         }
        ///     ],
        /// "targetUser": 
        ///     {
        ///         "nativeLanguage": "Norsk",
        ///         "ageGroup": "18-28,
        ///         "dialect": "østlandsk"
        ///     }
        /// }
        /// </remarks>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <response code="200">Successfully saved text</response>
        /// <response code="400">Failed</response>
        [HttpPost]
        public async Task<ActionResult> CreateText([FromBody] Text text)
        {
            bool success = await _textRep.CreateText(text);
            return Ok(success);
        }
        /// <summary>
        /// Get all texts
        /// </summary>
        /// <returns></returns>
        /// <response code="200">OK - returns list of all texts</response>
        /// <response code="400">Returns empty list</response>
        [HttpGet]
        public async Task<ActionResult> GetAllTexts()
        {
            var list = await _textRep.GetAllTexts();
            return Ok(list);
        }
        /// <summary>
        /// Delete text
        /// </summary>
        /// <param name="TextId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteText(int TextId) {
            Debug.WriteLine(TextId);
            bool success = await _textRep.DeleteText(TextId);

            return Ok(success);
        }
        /// <summary>
        /// Delete tag
        /// </summary>
        /// <param name="TagId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteTag(int TagId) {
            bool success = await _textRep.DeleteTag(TagId);
            return Ok(success);
        }

    }
}
