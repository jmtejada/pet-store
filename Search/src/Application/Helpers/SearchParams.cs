namespace PetStore.Search.Application.Helpers
{
    public class SearchParams
    {
        public string? SearchBy { get; set; }
        public string? SearchValue { get; set; }
        public int? Page { get; set; }
        public int? Size { get; set; }
        public string? OrderBy { get; set; }
        public string? OrderDir { get; set; }
    }
}