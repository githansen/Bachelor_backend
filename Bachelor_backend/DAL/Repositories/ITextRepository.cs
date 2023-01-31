using Bachelor_backend.Models;

namespace Bachelor_backend.DAL.Repositories
{
    public interface ITextRepository
    {
        Task<bool> CreateTag(string text);
        Task<bool> CreateText(Text text);
        Task<List<Text>> GetAllTexts();
        Task<Text> GetText(User user);
        Task<List<Tag>> GetAllTags();
        Task<User> RegisterUserInfo(User user);
        Task<User> getUser(int userId);
    }
}
