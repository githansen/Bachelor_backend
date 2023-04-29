using Bachelor_backend.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Bachelor_backend.Controller
{
    [Route("[controller]/[action]")]
    public class InitDBController : ControllerBase
    {
        private readonly InitDB _initDb;
        
        public InitDBController(InitDB initDb)
        {
            _initDb = initDb;
        }
        
        [HttpGet]
        public async Task<bool> InitDB()
        {
            return await _initDb.initDB();
        }
    }
}