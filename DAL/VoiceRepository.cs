// https://learn.microsoft.com/en-us/dotnet/api/system.guid.newguid?view=net-6.0

using Bachelor_backend.Models;

namespace Bachelor_backend.DAL;

public class VoiceRepository : IVoiceRepository
{

    private readonly DatabaseContext _db;

    public VoiceRepository(DatabaseContext db)
    {
        _db = db;
    }
    

    public async Task<string> SaveFile(IFormFile recording)
    {
        try
        {
            //Creates a recording directory if it doesnt exist
            if (!Directory.Exists($@"{Directory.GetCurrentDirectory()}\recordings"))
            {
                Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\recordings");
            }
            
            // Creates a new file name with a uuid name
            string uuid = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(recording.FileName);
            
            string newFileName = uuid + extension;
            string uploadPath = Path.Combine($@"{Directory.GetCurrentDirectory()}\recordings", newFileName);

            //TODO: Insert uuid and file path in db
            var user = new User()
            {
               // UUID = uuid
            };
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            
            //Using stream to copy the content of the recording file to disk
            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                await recording.CopyToAsync(stream);
            }
            return uuid;
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task<bool> DeleteFile(string uuid)
    {
        throw new NotImplementedException();
    }

    public async Task<IFormFile> GetAudioRecording(int textId)
    {
        throw new NotImplementedException();
    }
    
}