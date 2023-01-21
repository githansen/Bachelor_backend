namespace Bachelor_backend.DAL;

public interface IVoiceRepository
{
    Task<string> SaveFile(IFormFile recording);
    Task<bool> DeleteFile(string uuid);
    Task<IFormFile> GetAudioRecording(int textId);
}