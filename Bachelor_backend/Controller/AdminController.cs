using Bachelor_backend.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> CreateTag( string text)
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
        public async Task<ActionResult> CreateText( string text)
        {
            bool success = await _textRep.CreateText(text);
            return Ok(success);
        }
        [HttpGet]
        public async Task<ActionResult> GetTexts()
        {
            var list = await _textRep.GetAllTexts();
            return Ok(list);
        }
    }
}
