using Bachelor_backend.Models;
using Microsoft.AspNetCore.Mvc;

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
        Task<User> GetUser(Guid userId);
        Task<bool> DeleteTag(int tagId);
        Task<bool> DeleteText(int textId);
        Task<int> GetNumberOfTexts();
        Task<int> GetNumberOfUsers();
        Task<Text> GetOneText(int textId);
        Task<List<User>> GetAllUsers();
        Task<bool> EditText(Text text);
        Task<bool> EditTag(Tag tag);
    }
}
