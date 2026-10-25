namespace money_manager.Models;

public class PaginationViewModel
{
    public int Page { get; set; } = 1;

    private int recordsPerPage = 3;

    private readonly int maximumRecordsPerPage = 25;

    public int RecordsPerPage { get => recordsPerPage; set => recordsPerPage = value > maximumRecordsPerPage ? maximumRecordsPerPage : value; }

    public int RecordsToSkip => RecordsPerPage * (Page - 1);
}
