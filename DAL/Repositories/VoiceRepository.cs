// https://learn.microsoft.com/en-us/dotnet/api/system.guid.newguid?view=net-6.0

using Bachelor_backend.Models;

namespace Bachelor_backend.DAL.Repositories;

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
            
            
            string extension = Path.GetExtension(recording.FileName);

            var audioFile = new Audiofile();
            
            await _db.Audiofiles.AddAsync(audioFile);
            
            var uuid = audioFile.UUID.ToString();
            
            string newFileName = uuid + extension;
            string uploadPath = Path.Combine($@"{Directory.GetCurrentDirectory()}\recordings", newFileName);

            //TODO: Insert uuid and file path in db
            
            
            
            await _db.SaveChangesAsync();

            //Using stream to copy the content of the recording file to disk
            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                await recording.CopyToAsync(stream);
            }
            return uuid;
        }
        catch (Exception e)
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