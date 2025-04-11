namespace Lexilearn.Shared
{
    public class PaginationSettings
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public PaginationSettings()
        {
            PageNumber = 0;
            PageSize = 0;
        }
        public PaginationSettings(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 0 ? 0 : pageNumber;
            PageSize = (pageSize < 0 || pageSize > 25) ? 25 : pageSize;
        }
    }
}
