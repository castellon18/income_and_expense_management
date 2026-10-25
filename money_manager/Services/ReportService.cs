using money_manager.Entities;
using money_manager.Models;
using money_manager.ParametersClass;
using money_manager.Repositories;

namespace money_manager.Services;

public interface IReportService
{
    Task<DetailedTransactionReportsViewModel> GetDetailedTransactionReportsByAccountAsync(int userId, int accountId, int month, int year, dynamic viewBag);
    Task<DetailedTransactionReportsViewModel> GetDetailedTransactionReportsByAccountAsync(int userId, int month, int year, dynamic viewBag);
    Task<IEnumerable<ExpectedResultsPerWeekEntity>> GetWeeklyReportAsync(int userId, int month, int year, dynamic viewBag);
}

public class ReportService(ITransactionRepository transactionsRepository, IHttpContextAccessor httpContextAccessor) : IReportService
{
    private readonly ITransactionRepository _transactionsRepository = transactionsRepository;
    private readonly HttpContext _httpContext = httpContextAccessor?.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    public async Task<DetailedTransactionReportsViewModel> GetDetailedTransactionReportsByAccountAsync(int userId, int accountId, int month, int year, dynamic viewBag)
    {
        (DateTime StartDate, DateTime EndDate) = GenerateStartAndEndDates(month, year);

        GetTransactionsByAccountParameter getTransactionByAccount = new()
        {
            AccountId = accountId,
            UserId = userId,
            StartDate = StartDate,
            EndDate = EndDate,
        };

        IEnumerable<TransactionEntity> transactions = await _transactionsRepository.GetTransactionsByAccountIdAsync(getTransactionByAccount);

        DetailedTransactionReportsViewModel viewModel = GenerateDetailedTransactionReport(StartDate, EndDate, transactions);

        AssignValuesToViewBag(viewBag, StartDate, EndDate);

        return viewModel;
    }

    public async Task<DetailedTransactionReportsViewModel> GetDetailedTransactionReportsByAccountAsync(int userId, int month, int year, dynamic viewBag)
    {
        (DateTime StartDate, DateTime EndDate) = GenerateStartAndEndDates(month, year);

        GetTransactionsByUserParameter parameter = new()
        {
            UserId = userId,
            StartDate = StartDate,
            EndDate = EndDate
        };

        IEnumerable<TransactionEntity> transactions = await _transactionsRepository.GetTransactionsByUserIdAsync(parameter);

        DetailedTransactionReportsViewModel viewModel = GenerateDetailedTransactionReport(StartDate, EndDate, transactions);

        AssignValuesToViewBag(viewBag, StartDate, EndDate);

        return viewModel;
    }

    public async Task<IEnumerable<ExpectedResultsPerWeekEntity>> GetWeeklyReportAsync(int userId, int month, int year, dynamic viewBag)
    {
        (DateTime StartDate, DateTime EndDate) = GenerateStartAndEndDates(month, year);

        GetTransactionsByUserParameter parameter = new()
        {
            UserId = userId,
            StartDate = StartDate,
            EndDate = EndDate
        };

        AssignValuesToViewBag(viewBag, StartDate, EndDate);

        return await _transactionsRepository.GetExpectedResultsPerWeekAsync(parameter);
    }

    private (DateTime StartDate, DateTime EndDate) GenerateStartAndEndDates(int month, int year)
    {
        DateTime startDate;
        DateTime endDate;

        if (month <= 0 || month > 12 || year <= 1900)
        {
            DateTime now = DateTime.Today;
            startDate = new DateTime(now.Year, now.Month, 1);
        }
        else
        {
            startDate = new DateTime(year, month, 1);
        }

        endDate = startDate.AddMonths(1).AddDays(-1);

        return (startDate, endDate);
    }

    private static DetailedTransactionReportsViewModel GenerateDetailedTransactionReport(DateTime StartDate, DateTime EndDate, IEnumerable<TransactionEntity> transactions)
    {
        List<DetailedTransactionReportsViewModel.TransactionsByDate> transactionsByDate = transactions.OrderByDescending(t => t.TransactionDate.Date)
                    .GroupBy(t => t.TransactionDate)
                    .Select(group => new DetailedTransactionReportsViewModel.TransactionsByDate
                    {
                        TransactionDate = group.Key,
                        Transactions = group.AsEnumerable()
                    }).ToList();

        DetailedTransactionReportsViewModel viewModel = new()
        {
            GroupedTransactions = transactionsByDate,
            StartDate = StartDate,
            EndDate = EndDate,
        };
        return viewModel;
    }

    private void AssignValuesToViewBag(dynamic viewBag, DateTime StartDate, DateTime EndDate)
    {
        viewBag.previousMonth = StartDate.AddMonths(-1).Month;
        viewBag.previousYear = StartDate.AddMonths(-1).Year;
        viewBag.nextMonth = EndDate.AddMonths(1).Month;
        viewBag.nextYear = EndDate.AddMonths(1).Year;
        viewBag.returnUrl = _httpContext.Request.Path + _httpContext.Request.QueryString;
    }
}
