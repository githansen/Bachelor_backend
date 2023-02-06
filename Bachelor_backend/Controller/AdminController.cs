using Bachelor_backend.DAL.Repositories;
using Bachelor_backend.Models.APIModels;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Bachelor_backend.Models;

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
        [HttpPost]
        public async Task<ActionResult> CreateTag(string text)
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
        [HttpGet]
        public async Task<ActionResult> GetTags()
        {
            var list = await _textRep.GetAllTags();
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> CreateText([FromBody] Text text)
        {
            bool success = await _textRep.CreateText(text);
            return Ok(success);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllTexts()
        {
            var list = await _textRep.GetAllTexts();
            return Ok(list);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteText([FromQuery(Name ="TextId")]int TextId) {
            bool success = await _textRep.DeleteText(TextId);

            return Ok(success);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteTag([FromQuery(Name ="TagId")]int TagId) {
            bool success = await _textRep.DeleteTag(TagId);
            return Ok(success);
        }

           }
}
