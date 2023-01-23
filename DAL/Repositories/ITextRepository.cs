using Bachelor_backend.Models;

namespace Bachelor_backend.DAL.Repositories
{
    public interface ITextRepository
    {
        Task<List<Text>> GetTexts();
        Task<bool> CreateText();
        Task<bool> CreateTag();
        Task<List<Tag>> GetTags();
    }
}
