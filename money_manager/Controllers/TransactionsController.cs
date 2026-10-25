using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using money_manager.Entities;
using money_manager.Mappings;
using money_manager.Models;
using money_manager.ParametersClass;
using money_manager.Repositories;
using money_manager.Services;
using System.Data;

namespace money_manager.Controllers;

public class TransactionsController(ITransactionRepository transactionRepository, ICategoryRepository categoryRepository, IAccountRepository accountRepository, IUsersService usersService, IReportService reportService) : Controller
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IUsersService _usersService = usersService;
    private readonly IReportService _reportService = reportService;

    [HttpGet]
    public async Task<IActionResult> Index(int month, int year)
    {
        int userId = _usersService.GetUserId();

        DetailedTransactionReportsViewModel viewModel = await _reportService.GetDetailedTransactionReportsByAccountAsync(userId, month, year, ViewBag);

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        int userId = _usersService.GetUserId();
        CreateTransactionViewModel viewModel = new()
        {
            Accounts = await GetAccounts(userId),
            Categories = await GetCategoriesAsync(userId, OperationTypeEntity.Ingreso)
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTransactionViewModel viewModel)
    {
        int userId = _usersService.GetUserId();

        if (!ModelState.IsValid)
        {
            viewModel.Accounts = await GetAccounts(userId);
            viewModel.Categories = await GetCategoriesAsync(userId, viewModel.OperationTypeId);
            return View(viewModel);
        }

        AccountEntity? account = await _accountRepository.GetAccountByIdAsync(viewModel.AccountId, userId);

        if (account == null)
        {
            return RedirectToAction(actionName: "NotFoundPage", controllerName: "Home");
        }

        CategoryEntity? category = await _categoryRepository.GetCategoryByIdAsync(viewModel.CategoryId, userId);

        if (category == null)
        {
            return RedirectToAction(actionName: "NotFoundPage", controllerName: "Home");
        }

        if (viewModel.OperationTypeId == OperationTypeEntity.Gasto)
        {
            viewModel.Amount *= -1;
        }

        TransactionEntity transaction = new()
        {
            UserId = userId,
            Amount = viewModel.Amount,
            TransactionDate = viewModel.TransactionDate,
            TransactionNote = viewModel.TransactionNote,
            AccountId = viewModel.AccountId,
            CategoryId = viewModel.CategoryId
        };
        await _transactionRepository.CreateTransactionAsync(transaction);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, string? returnUrl)
    {
        int userId = _usersService.GetUserId();
        TransactionEntity? transaction = await _transactionRepository.GetTransactionByIdAsync(id, userId);

        if (transaction == null)
        {
            return RedirectToAction(actionName: "NotFoundPage", controllerName: "Home");
        }

        EditTransactionViewModel viewModel = transaction.ToViewModel();

        viewModel.PreviousAmount = viewModel.Amount;

        if (viewModel.OperationTypeId == OperationTypeEntity.Gasto)
        {
            viewModel.Amount *= -1;
        }

        viewModel.PreviousAccountId = transaction.AccountId;
        viewModel.Categories = await GetCategoriesAsync(userId, viewModel.OperationTypeId);
        viewModel.Accounts = await GetAccounts(userId);
        viewModel.ReturnUrl = returnUrl ?? string.Empty;

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditTransactionViewModel viewModel)
    {
        int userId = _usersService.GetUserId();

        if (!ModelState.IsValid)
        {
            viewModel.Categories = await GetCategoriesAsync(userId, viewModel.OperationTypeId);
            viewModel.Accounts = await GetAccounts(userId);
            return View(viewModel);
        }

        AccountEntity? account = await _accountRepository.GetAccountByIdAsync(viewModel.AccountId, userId);

        if (account == null)
        {
            return RedirectToAction(actionName: "NotFoundPage", controllerName: "Home");
        }

        CategoryEntity? category = await _categoryRepository.GetCategoryByIdAsync(viewModel.CategoryId, userId);

        if (category == null)
        {
            return RedirectToAction(actionName: "NotFoundPage", controllerName: "Home");
        }

        TransactionEntity transaction = viewModel.ToModel();

        if (viewModel.OperationTypeId == OperationTypeEntity.Gasto)
        {
            transaction.Amount *= -1;
        }
        else
        {
            viewModel.PreviousAmount *= -1;
        }

        await _transactionRepository.UpdateTransactionAsync(transaction, viewModel.PreviousAmount, viewModel.PreviousAccountId);

        if (string.IsNullOrWhiteSpace(viewModel.ReturnUrl))
        {
            return RedirectToAction(nameof(Index));
        }

        return LocalRedirect(viewModel.ReturnUrl);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = _usersService.GetUserId();

        TransactionEntity? transaction = await _transactionRepository.GetTransactionByIdAsync(id, userId);

        if (transaction == null)
        {
            return NotFound();
        }

        await _transactionRepository.DeleteTransactionAsync(id);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> GetCategories([FromBody] OperationTypeEntity operationType)
    {
        int userId = _usersService.GetUserId();
        IEnumerable<SelectListItem> categories = await GetCategoriesAsync(userId, operationType);
        return Ok(categories);
    }

    public async Task<IActionResult> Weekly(int month, int year)
    {
        int userId = _usersService.GetUserId();

        IEnumerable<ExpectedResultsPerWeekEntity> expectedResults = await _reportService.GetWeeklyReportAsync(userId, month, year, ViewBag);

        List<ExpectedResultsPerWeekEntity> grouped = expectedResults.GroupBy(er => er.Weeks).Select(static er => new ExpectedResultsPerWeekEntity
        {
            Weeks = er.Key,
            Income = er.Where(e => e.OperationTypeId == OperationTypeEntity.Ingreso).Select(e => e.Amount).FirstOrDefault(),
            Bills = er.Where(e => e.OperationTypeId == OperationTypeEntity.Gasto).Select(e => e.Amount).FirstOrDefault()
        }).ToList();

        if (month == 0 || year == 0)
        {
            DateTime now = DateTime.Now;
            year = now.Year;
            month = now.Month;
        }

        DateTime referenceDate = new(year, month, 1);

        IEnumerable<int> daysOfTheMonth = Enumerable.Range(1, referenceDate.AddMonths(1).AddDays(-1).Day);

        List<int[]> segmentedDays = daysOfTheMonth.Chunk(7).ToList();

        for (int i = 0; i < segmentedDays.Count; i++)
        {
            int week = i + 1;
            DateTime startDate = new(year, month, segmentedDays[i].FirstOrDefault());
            DateTime endDate = new(year, month, segmentedDays[i].LastOrDefault());
            ExpectedResultsPerWeekEntity? groupWeek = grouped.FirstOrDefault(g => g.Weeks == week);
            if (groupWeek == null)
            {
                grouped.Add(new ExpectedResultsPerWeekEntity
                {
                    Weeks = week,
                    Income = 0,
                    Bills = 0,
                    Amount = 0,
                    StartDate = startDate,
                    EndDate = endDate,
                });
            }
            else
            {
                groupWeek.StartDate = startDate;
                groupWeek.EndDate = endDate;
            }
        }

        grouped = grouped.OrderByDescending(g => g.Weeks).ToList();

        WeeklyReportViewModel viewModel = new()
        {
            TransactionsByWeek = grouped,
            ReferenceDate = referenceDate,
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Monthly(int year)
    {
        int userId = _usersService.GetUserId();

        if (year == 0)
        {
            year = DateTime.Now.Year;
        }

        IEnumerable<ExpectedResultsPerMonthEntity> expectedResults = await _transactionRepository.GetExpectedResultsPerMonthAsync(userId, year);

        List<ExpectedResultsPerMonthEntity> grouped = expectedResults.GroupBy(er => er.Months).Select(static er => new ExpectedResultsPerMonthEntity
        {
            Months = er.Key,
            Income = er.Where(e => e.OperationTypeId == OperationTypeEntity.Ingreso).Select(e => e.Amount).FirstOrDefault(),
            Bills = er.Where(e => e.OperationTypeId == OperationTypeEntity.Gasto).Select(e => e.Amount).FirstOrDefault()
        }).ToList();

        for (int month = 1; month <= 12; month++)
        {
            DateTime referenceDate = new(year, month, 1);
            ExpectedResultsPerMonthEntity? groupMonth = grouped.FirstOrDefault(g => g.Months == month);
            if (groupMonth == null)
            {
                grouped.Add(new ExpectedResultsPerMonthEntity
                {
                    Months = month,
                    Income = 0,
                    Bills = 0,
                    Amount = 0,
                    ReferenceDate = referenceDate
                });
            }
            else
            {
                groupMonth.ReferenceDate = referenceDate;
            }
        }

        grouped = grouped.OrderByDescending(g => g.Months).ToList();

        MonthlyReportViewModel viewModel = new()
        {
            TransactionsByMonth = grouped,
            Year = year,
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult ExcelReport()
    {
        return View();
    }

    [HttpGet]
    public async Task<FileResult> ExportExcelByMonth(int month, int year)
    {
        DateTime startDate = new(year, month, 1);
        DateTime endDate = startDate.AddMonths(1).AddDays(-1);
        int userId = _usersService.GetUserId();

        IEnumerable<TransactionEntity> transactions = await _transactionRepository.GetTransactionsByUserIdAsync(new GetTransactionsByUserParameter
        {
            UserId = userId,
            StartDate = startDate,
            EndDate = endDate
        });

        string nameFile = $"money-manager-report_month_{month}_{year}.xlsx";

        return GenerateExcel(nameFile, transactions);
    }

    [HttpGet]
    public async Task<FileResult> ExportExcelByYear(int year)
    {
        DateTime startDate = new(year, 1, 1);
        DateTime endDate = startDate.AddYears(1).AddDays(-1);
        int userId = _usersService.GetUserId();
        IEnumerable<TransactionEntity> transactions = await _transactionRepository.GetTransactionsByUserIdAsync(new GetTransactionsByUserParameter
        {
            UserId = userId,
            StartDate = startDate,
            EndDate = endDate
        });
        string nameFile = $"money-manager-report_year_{year}.xlsx";
        return GenerateExcel(nameFile, transactions);
    }

    [HttpGet]
    public async Task<FileResult> ExportExcelAll()
    {
        DateTime startDate = DateTime.Today.AddYears(-100);
        DateTime endDate = DateTime.Today.AddYears(1000);
        int userId = _usersService.GetUserId();
        IEnumerable<TransactionEntity> transactions = await _transactionRepository.GetTransactionsByUserIdAsync(new GetTransactionsByUserParameter
        {
            UserId = userId,
            StartDate = startDate,
            EndDate = endDate
        });
        string nameFile = "money-manager-report_all.xlsx";
        return GenerateExcel(nameFile, transactions);
    }

    [HttpGet]
    public IActionResult Calendar()
    {
        return View();
    }

    [HttpGet]
    public async Task<JsonResult> GetTransactionsByCalendar(DateTime start, DateTime end)
    {
        int userId = _usersService.GetUserId();

        IEnumerable<TransactionEntity> transactions = await _transactionRepository.GetTransactionsByUserIdAsync(new GetTransactionsByUserParameter
        {
            UserId = userId,
            StartDate = start,
            EndDate = end
        });

        IEnumerable<CalendarEventViewModel> viewModel = transactions.Select(static t => new CalendarEventViewModel()
        {
            Title = t.Amount.ToString("C"),
            Start = t.TransactionDate.ToString("yyyy-MM-dd"),
            End = t.TransactionDate.ToString("yyyy-MM-dd"),
            Color = t.OperationTypeId == OperationTypeEntity.Gasto ? "Red" : "Green"
        });

        return Json(viewModel);
    }

    [HttpGet]
    public async Task<JsonResult> GetTransactionsByDate(DateTime date)
    {
        int userId = _usersService.GetUserId();

        IEnumerable<TransactionEntity> transactions = await _transactionRepository.GetTransactionsByUserIdAsync(new GetTransactionsByUserParameter
        {
            UserId = userId,
            StartDate = date,
            EndDate = date
        });

        return Json(transactions);
    }

    // Helper methods
    private async Task<IEnumerable<SelectListItem>> GetAccounts(int userId)
    {
        IEnumerable<AccountEntity> accounts = await _accountRepository.SearchAccountsByUserAsync(userId);
        return accounts.Select(a => new SelectListItem(a.AccountName, a.AccountId.ToString()));
    }

    private async Task<IEnumerable<SelectListItem>> GetCategoriesAsync(int userId, OperationTypeEntity operationType)
    {
        IEnumerable<CategoryEntity> categories = await _categoryRepository.SearchCategoriesByUserAndOperationTypeAsync(userId, operationType);
        List<SelectListItem> newSelectListItem = [.. categories.Select(c => new SelectListItem(c.CategoryName, c.CategoryId.ToString()))];
        SelectListItem newOption = new("--- Seleccione una categoría ---", "0", true);
        newSelectListItem.Insert(0, newOption);
        return newSelectListItem;
    }

    private FileResult GenerateExcel(string nameFile, IEnumerable<TransactionEntity> transactions)
    {
        using DataTable dt = new("Transactions");
        dt.Columns.AddRange(
        [
            new("Fecha"),
            new("Cuenta"),
            new("Categoría"),
            new("Monto"),
            new("Ingreso/Gasto"),
            new("Nota")
        ]);

        foreach (TransactionEntity transaction in transactions)
        {
            dt.Rows.Add(
                transaction.TransactionDate.ToString("dd/MM/yyyy"),
                transaction.AccountName,
                transaction.CategoryName,
                transaction.Amount.ToString("C"),
                transaction.OperationTypeId == OperationTypeEntity.Ingreso ? "Ingreso" : "Gasto",
                transaction.TransactionNote
            );
        }

        using XLWorkbook wb = new();
        wb.Worksheets.Add(dt);
        using MemoryStream stream = new();
        wb.SaveAs(stream);
        stream.Position = 0;
        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nameFile);
    }
}
