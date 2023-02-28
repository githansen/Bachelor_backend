using Bachelor_backend.Models;

namespace Bachelor_backend.DAL.Repositories;
/// <summary>
/// 
/// </summary>
public interface IVoiceRepository
{
    Task<string> SaveFile(IFormFile recording, int textId, int userId);
    Task<string> DeleteFile(string uuid);
    Task<FormFile> GetOneRecording(string uuid);
    Task<int> GetNumberOfRecordings();

    Task<List<Audiofile>> GetAllRecordings();
    
}