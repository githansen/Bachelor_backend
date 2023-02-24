// https://learn.microsoft.com/en-us/dotnet/api/system.guid.newguid?view=net-6.0

using Bachelor_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bachelor_backend.DAL.Repositories;

public class VoiceRepository : IVoiceRepository
{

    private readonly DatabaseContext _db;
    private readonly IAzureStorage _azureStorage;
    private readonly ILogger<VoiceRepository> _logger;

    public VoiceRepository(DatabaseContext db, IAzureStorage azureStorage, ILogger<VoiceRepository> logger)
    {
        _db = db;
        _azureStorage = azureStorage;
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
            if (recording.Length > 10000000)
            {
                return "Audiofile is too big";
            }

            string extension = Path.GetExtension(recording.FileName);
            
            //TODO: Add/Remove accepted file extensions
            //List with allowed file extensions
            var fileExtensions = new List<string>() { ".mp3", ".wav", ".flac", ".aac",".m4a", ".mp4" };
            
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
            /*
            string newFileName = uuid + extension;
            //string uploadPath = Path.Combine(@"/", newFileName);
            
            audioFile.Path = newFileName;
            IFormFile newFile;
            //Using stream to copy the content of the recording file to disk
            using (var stream = new MemoryStream())
            {
                //await recording.CopyToAsync(stream);
                newFile = new FormFile(stream, 0, recording.Length, recording.Name, newFileName);
            }
*/
            var response = await _azureStorage.UploadAsync(recording);
            
            if (response.Error)
            {
                _logger.LogInformation("File not uploaded to Azure");
                return "";
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
    
    public async Task<string> DeleteFile(string uuid)
    {
        try
        {

            var audiofile = await _db.Audiofiles.FindAsync(Guid.Parse(uuid));
            if (audiofile == null)
            {
                return "Audiofile not found";
            }
            string path = audiofile.Path;
            _db.Audiofiles.Remove(audiofile);
            File.Delete(path);
            await _db.SaveChangesAsync();
            return "Audiofile deleted";

        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            Console.WriteLine(e.Message);
            return "Audiofile not deleted";
        }

    }

    public async Task<IFormFile> GetAudioRecording(int textId)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetNumberOfRecordings()
    {
        throw new NotImplementedException();
    }

    public Task<List<Audiofile>> GetAllRecordings()
    {
        throw new NotImplementedException();
    }
}