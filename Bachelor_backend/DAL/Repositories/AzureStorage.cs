using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Bachelor_backend.Models.BlobModels;


namespace Bachelor_backend.DAL.Repositories
{

    public class AzureStorage : IAzureStorage
    {
        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;
        private readonly ILogger<AzureStorage> _logger;

        public AzureStorage(IConfiguration configuration, ILogger<AzureStorage> logger)
        {
            _storageConnectionString = configuration.GetValue<string>("AzureBlobStorageConnectionString");
            _storageContainerName = configuration.GetValue<string>("AzureBlobStorageContainerName");
            _logger = logger;
        }

        public async Task<BlobResponse> UploadAsync(IFormFile file, string newFileName)
        {
            BlobResponse response = new();

            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            try
            {
                BlobClient client = container.GetBlobClient(newFileName);

                // Open a stream for the file we want to upload
                await using (Stream? data = file.OpenReadStream())
                {
                    await client.UploadAsync(data, new BlobHttpHeaders { ContentType = file.ContentType });
                }

                response.Status = $"File {file.FileName} Uploaded Sucessfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;

            }
            catch (RequestFailedException e)
                when (e.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogInformation($"File {file.FileName} already exists. Set another name to store the file in the container");
                response.Error = true;
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                response.Status = $"Unexpected error: {e.StackTrace} Check Log.";
                response.Error = true;
                return response;

            }

            return response;
        }

        public async Task<BlobResponse> DeleteAsync(string filename)
        {
            
            BlobResponse response = new();
            // Get a reference to a container
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            
            try
            {
                // Get a reference to the blob
                BlobClient client = container.GetBlobClient(filename);
            
                // Delete the blob
                var responseFromAzure = await client.DeleteAsync();
                response.Error = responseFromAzure.IsError;
                response.Status = responseFromAzure.Status.ToString();
                return response;
            }
            catch (Exception e)
            {
                response.Error = true;
                response.Status = $"{filename} could not be deleted. {e.Message}";
                return response;
            }
            
        }
    }
}
