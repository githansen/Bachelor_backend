using Bachelor_backend.Models;

namespace Bachelor_backend.DAL.Repositories
{
    public interface ITextRepository
    {
        Task<bool> CreateTag();
        Task<bool> CreateText();
        Task<List<Text>> GetAllTexts();
        Task<Text> GetText();
        Task<List<Tag>> GetAllTags();

    }
}
