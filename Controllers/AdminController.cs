using Microsoft.AspNetCore.Mvc;

namespace Bachelor_backend.Controllers
{
    public class AdminController : ControllerBase
    {


        // AzureAD 
        public async Task<ActionResult> Login()
        {
            throw new InvalidOperationException("Not created yet");
        }
        public async Task<ActionResult> CreateTag()
        {
            throw new InvalidOperationException("Not created yet");
        }
        public async Task<ActionResult> GetAllTags()
        {
            throw new InvalidOperationException("Not created yet");
        }

        // Not yet decided how this will be done
        public async Task<ActionResult> CreateTargetAudience()
        {
            throw new InvalidOperationException("Not created yet");
        }

        // Input for this function must be decided, file ID, UUID?
        public async Task<ActionResult> GetAudioFile()
        {
            throw new InvalidOperationException("Not created yet");
        }
        public async Task<ActionResult> CreateText()
        {
            throw new InvalidOperationException("Not created yet");
        }
        public async Task<ActionResult> GetAllTexts()
        {
            throw new InvalidOperationException("Not created yet");
        }

    }
}
