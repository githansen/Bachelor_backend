namespace Bachelor_backend.DAL.Repositories;

public interface IVoiceRepository
{
    Task<string> SaveFile(IFormFile recording, int textId, int userId);
    Task<string> DeleteFile(string uuid);
    Task<IFormFile> GetAudioRecording(int textId);
    Task<int> GetNumberOfRecordings();
}