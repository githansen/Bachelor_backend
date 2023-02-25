using Bachelor_backend.Models;

namespace Bachelor_backend.DAL.Repositories;

public interface IVoiceRepository
{
    Task<string> SaveFile(IFormFile recording, int textId, int userId);
    Task<string> DeleteFile(string uuid);
    Task<IFormFile> GetAudioRecording(string uuid);
    Task<int> GetNumberOfRecordings();

    Task<List<Audiofile>> GetAllRecordings();
    
}