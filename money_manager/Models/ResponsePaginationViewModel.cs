namespace money_manager.Models;

public class ResponsePaginationViewModel
{
    public int Page { get; set; } = 1;
    public int RecordsPerPage { get; set; } = 10;
    public int TotalRecords { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / RecordsPerPage);
    public string UrlBase { get; set; } = string.Empty;
}

public class ResponsePaginationViewModel<T> : ResponsePaginationViewModel
{
    public IEnumerable<T> Records { get; set; } = [];
}
