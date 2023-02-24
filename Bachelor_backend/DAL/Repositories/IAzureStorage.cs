using Bachelor_backend.Models.BlobModels;

namespace Bachelor_backend.DAL.Repositories
{
    public interface IAzureStorage
    {
        Task<BlobResponseDto> UploadAsync(IFormFile file);

        Task<List<BlobDto>> ListAsync();
    }
}
