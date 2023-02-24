using Bachelor_backend.Models;
using Bachelor_backend.Models.APIModels;
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
        Task<User> GetUser(int userId);
        Task<bool> DeleteTag(int TextId);
        Task<bool> DeleteText(int TextId);
        Task<int> GetNumberOfTexts();
        Task<int> GetNumberOfUsers();
        Task<Text> GetOneText(int id);
    }
}
