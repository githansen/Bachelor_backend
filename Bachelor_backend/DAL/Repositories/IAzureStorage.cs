using Bachelor_backend.Models.BlobModels;

namespace Bachelor_backend.DAL.Repositories
{

    public interface IAzureStorage
    {
        Task<BlobResponse> UploadAsync(IFormFile file, string newFileName);

        Task<BlobResponse> DeleteAsync(string filename);
    }
}
