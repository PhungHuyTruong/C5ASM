namespace PH48831_C5_ASM.Models
{
    public class HomeQueryParameters
    {
        public string? Keyword { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
        public string? SortBy { get; set; }
        public string? Order { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 9;
    }
}
