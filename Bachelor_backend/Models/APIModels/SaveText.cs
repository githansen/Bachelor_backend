namespace Bachelor_backend.Models.APIModels
{
    public class SaveText
    {
        public string TextText { get; set; }
        public List<int> TagIds { get; set; }
        public string? NativeLanguage { get; set; }
        public string? AgeGroup { get; set; }
        public string? Dialect { get; set; }

    }
}
