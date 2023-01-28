using Bachelor_backend.Models;

namespace Bachelor_backend.DAL.Repositories
{
    public interface ITextRepository
    {
        Task<bool> CreateTag(string text);
        Task<bool> CreateText(string text);
        Task<List<Text>> GetAllTexts();
        Task<Text> GetText();
        Task<List<Tag>> GetAllTags();
        Task<bool> login();
    }
}
