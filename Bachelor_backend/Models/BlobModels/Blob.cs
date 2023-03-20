namespace Bachelor_backend.Models.BlobModels
{
    public class Blob
    {
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public string? ContentType { get; set; }
        public Stream? Content { get; set; }
    }
}
