using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using money_manager.Entities;
using money_manager.Mappings;
using money_manager.Models;
using money_manager.Repositories;
using money_manager.Services;

namespace money_manager.Controllers;

public class AccountsController(IAccountTypeRepository accountTypesRepository, IUsersService usersService, ITransactionRepository transactionsRepository, IAccountRepository accountsRepository, IReportService reportService) : Controller
{
    private readonly IAccountTypeRepository _accountTypesRepository = accountTypesRepository;
    private readonly IUsersService _usersService = usersService;
    private readonly ITransactionRepository _transactionsRepository = transactionsRepository;
    private readonly IAccountRepository _accountsRepository = accountsRepository;
    private readonly IReportService _reportService = reportService;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        int userId = _usersService.GetUserId();

        IEnumerable<AccountEntity> accounts = await _accountsRepository.SearchAccountsByUserAsync(userId);

        List<AccountIndexViewModel> viewModel = accounts.GroupBy(a => a.AccountTypeName)
            .Select(static g => new AccountIndexViewModel
            {
                AccountType = g.Key,
                Accounts = g.AsEnumerable(),
            }).ToList();
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        int userId = _usersService.GetUserId();
        CreateAccountViewModel accountModel = new()
        {
            AccountTypes = await GetAccountTypesSelectListItemsAsync(userId)
        };
        return View(accountModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateAccountViewModel viewModel)
    {
        int userId = _usersService.GetUserId();

        AccountTypeEntity? accountTypes = await _accountTypesRepository.GetAccountTypeByIdAsync(viewModel.AccountTypeId, userId);

        if (accountTypes is null)
        {
            return RedirectToAction(actionName: "NotFoundPage", controllerName: "Home");
        }

        if (!ModelState.IsValid)
        {
            viewModel.AccountTypes = await GetAccountTypesSelectListItemsAsync(userId);
            return View(viewModel);
        }

        AccountEntity accountModel = viewModel.ToModel();

        await _accountsRepository.CreateAccountAsync(accountModel);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        int userId = _usersService.GetUserId();

        AccountEntity? account = await _accountsRepository.GetAccountByIdAsync(id, userId);

        if (account is null)
        {
            return RedirectToAction(actionName: "NotFoundPage", controllerName: "Home");
        }

        CreateAccountViewModel viewModel = account.ToViewModel();

        viewModel.AccountTypes = await GetAccountTypesSelectListItemsAsync(userId);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CreateAccountViewModel viewModel)
    {
        int userId = _usersService.GetUserId();

        AccountEntity? existingAccount = await _accountsRepository.GetAccountByIdAsync(viewModel.AccountId, userId);
        if (existingAccount is null)
        {
            return RedirectToAction(actionName: "NotFoundPage", controllerName: "Home");
        }

        AccountTypeEntity? accountTypes = await _accountTypesRepository.GetAccountTypeByIdAsync(viewModel.AccountTypeId, userId);
        if (accountTypes is null)
        {
            return RedirectToAction(actionName: "NotFoundPage", controllerName: "Home");
        }

        if (!ModelState.IsValid)
        {
            viewModel.AccountTypes = await GetAccountTypesSelectListItemsAsync(userId);
            return View(viewModel);
        }

        AccountEntity accountModel = viewModel.ToModel();
        await _accountsRepository.UpdateAccountAsync(accountModel);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = _usersService.GetUserId();
        AccountEntity? existingAccount = await _accountsRepository.GetAccountByIdAsync(id, userId);
        if (existingAccount is null)
        {
            return NotFound("Account not found");
        }
        await _accountsRepository.DeleteAccountAsync(id);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, int month, int year)
    {
        int userId = _usersService.GetUserId();

        AccountEntity? account = await _accountsRepository.GetAccountByIdAsync(id, userId);

        if (account is null)
        {
            return RedirectToAction(actionName: "NotFoundPage", controllerName: "Home");
        }

        ViewBag.AccountName = account.AccountName;

        dynamic viewModel = await _reportService.GetDetailedTransactionReportsByAccountAsync(userId, id, month, year, ViewBag);

        return View(viewModel);
    }

    private async Task<IEnumerable<SelectListItem>> GetAccountTypesSelectListItemsAsync(int userId)
    {
        IEnumerable<AccountTypeEntity> accountTypes = await _accountTypesRepository.GetAccountTypesAsync(userId);

        return accountTypes.Select(at => new SelectListItem
        {
            Text = at.AccountTypeName,
            Value = at.AccountTypeId.ToString()
        });
    }
}
