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
            
            // Gets the text and user from the database
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
            audioFile.Path = newFileName;
            
            //Uploads file to Azure blobstorage
            var response = await _azureStorage.UploadAsync(recording, newFileName);
            
            //Checks if the file was uploaded to Azure
            if (response.Error)
            {
                _logger.LogInformation("File not uploaded to Azure");
                return "";
            }
            
            //Saves database and returns the uuid of the audiofile
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

    public async Task<IFormFile> GetAudioRecording(string uuid)
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