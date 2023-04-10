using Bachelor_backend.Models;

namespace Bachelor_backend.DAL.Repositories;

public interface IVoiceRepository
{
    Task<string> SaveFile(IFormFile recording, int textId, Guid userId);
    Task<string> DeleteFile(string uuid);
    Task<FormFile> GetOneRecording(string uuid);
    Task<int> GetNumberOfRecordings();

    Task<List<Audiofile>> GetAllRecordings();
    
}