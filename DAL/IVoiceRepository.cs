namespace Bachelor_backend.DAL;

public interface IVoiceRepository
{
    Task<string> SaveFile(IFormFile recording);
}