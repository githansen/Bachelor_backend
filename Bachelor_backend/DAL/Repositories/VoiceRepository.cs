// https://learn.microsoft.com/en-us/dotnet/api/system.guid.newguid?view=net-6.0

using Bachelor_backend.Models;

namespace Bachelor_backend.DAL.Repositories;

public class VoiceRepository : IVoiceRepository
{

    private readonly DatabaseContext _db;
    private readonly ILogger<VoiceRepository> _logger;

    public VoiceRepository(DatabaseContext db, ILogger<VoiceRepository> logger)
    {
        _db = db;
        _logger = logger;
    }


    public async Task<string> SaveFile(IFormFile recording, int textId, int userId)
    {
        try
        {
            
            //Creates a recording directory if it doesnt exist
            if (!Directory.Exists($@"{Directory.GetCurrentDirectory()}\recordings"))
            {
                Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\recordings");
            }
            
            //TODO: Calibrate file size limit
            //Checks file size and returns error if file is too big
            if (recording.Length > 1000000)
            {
                return "Audiofile is too big";
            }

            string extension = Path.GetExtension(recording.FileName);
            
            //TODO: Add/Remove accepted file extensions
            //List with allowed file extensions
            var fileExtensions = new List<string>() { ".mp3", ".wav", ".flac", ".aac" };
            
            //Checks if the file extension is allowed
            if (!fileExtensions.Contains(extension))
            {
                return "File extension not allowed";
            }
            
            var text = await _db.Texts.FindAsync(textId);
            var user = await _db.Users.FindAsync(userId);

            var audioFile = new Audiofile()
            {
                Text = text,
                User = user,
            };

            await _db.Audiofiles.AddAsync(audioFile);

            var uuid = audioFile.UUID.ToString();

            string newFileName = uuid + extension;
            string uploadPath = Path.Combine($@"{Directory.GetCurrentDirectory()}\recordings", newFileName);
            
            audioFile.Path = uploadPath;
            
            //Using stream to copy the content of the recording file to disk
            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                await recording.CopyToAsync(stream);
            }
            
            await _db.SaveChangesAsync();
            return uuid;
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            return "";
        }
    }

    public async Task<bool> DeleteFile(string uuid)
    {
        try
        {
            var audiofile = await _db.Audiofiles.FindAsync(Guid.Parse(uuid));
            if (audiofile == null)
            {
                return false;
            }
            string path = audiofile.Path;
            File.Delete(path);
            return true;

        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            return false;
        }

    }

    public async Task<IFormFile> GetAudioRecording(int textId)
    {
        throw new NotImplementedException();
    }

}