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

        public async Task<BlobResponseDto> UploadAsync(IFormFile blob)
        {
            BlobResponseDto response = new();

            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            try
            {
                BlobClient client = container.GetBlobClient(blob.FileName);

                // Open a stream for the file we want to upload
                await using (Stream? data = blob.OpenReadStream())
                {
                    await client.UploadAsync(data, new BlobHttpHeaders { ContentType = blob.ContentType });
                }

                response.Status = $"File {blob.FileName} Uploaded Sucessfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;

            }
            catch (RequestFailedException e)
                when (e.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogInformation($"File {blob.FileName} already exists. Set another name to store the file in the container");
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
        public async Task<List<BlobDto>> ListAsync()
        {
            // Get a reference to a container named in appsettings.json
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            // Create a new list object for 
            List<BlobDto> files = new List<BlobDto>();

            await foreach (BlobItem file in container.GetBlobsAsync())
            {
                // Add each file retrieved from the storage container to the files list by creating a BlobDto object
                string uri = container.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }

            // Return all files to the requesting method
            return files;
        }
    }
}
