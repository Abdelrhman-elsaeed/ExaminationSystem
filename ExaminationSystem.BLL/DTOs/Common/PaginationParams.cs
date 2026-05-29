namespace ExaminationSystem.BLL.DTOs.Common
{
    public class PaginationParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : (value < 1 ? 1 : value);
        }

        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; }
        public bool IsDescending { get; set; }
    }
}
