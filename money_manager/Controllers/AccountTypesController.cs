using Microsoft.AspNetCore.Mvc;
using money_manager.Entities;
using money_manager.Repositories;
using money_manager.Services;

namespace money_manager.Controllers;

public class AccountTypesController(IAccountTypeRepository accountTypesRepository, IUsersService usersService) : Controller
{
    private readonly IAccountTypeRepository _accountTypesRepository = accountTypesRepository;
    private readonly IUsersService _usersService = usersService;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        int userId = _usersService.GetUserId();
        IEnumerable<AccountTypeEntity> accountTypes = await _accountTypesRepository.GetAccountTypesAsync(userId);
        return View(accountTypes);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AccountTypeEntity accountType)
    {
        if (!ModelState.IsValid)
        {
            return View(accountType);
        }

        accountType.UserId = _usersService.GetUserId(); // TODO: Get the user ID from the authenticated user

        bool accountTypeExists = await _accountTypesRepository.AccountTypeExistsAsync(accountType.AccountTypeName, accountType.UserId);

        if (accountTypeExists)
        {
            ModelState.AddModelError(nameof(accountType.AccountTypeName), "An account type with that name already exists.");
            return View(accountType);
        }

        await _accountTypesRepository.CreateAccountTypeAsync(accountType);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> VerifyAccountTypeExists(string accountTypeName, int accountTypeId = 0)
    {
        int userId = _usersService.GetUserId(); // TODO: Get the user ID from the authenticated user
        bool exists = await _accountTypesRepository.AccountTypeExistsAsync(accountTypeName, userId, accountTypeId);
        if (exists)
        {
            return Json($"An account type with the name '{accountTypeName}' already exists.");
        }

        return Json(true);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        int userId = _usersService.GetUserId();
        AccountTypeEntity? accountType = await _accountTypesRepository.GetAccountTypeByIdAsync(id, userId);
        if (accountType == null)
        {
            return RedirectToAction(actionName: "NotFoundPage", controllerName: "Home");
        }

        return View(accountType);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AccountTypeEntity accountType)
    {
        if (!ModelState.IsValid)
        {
            return View(accountType);
        }

        int userId = _usersService.GetUserId();
        AccountTypeEntity? existingAccountType = await _accountTypesRepository.GetAccountTypeByIdAsync(accountType.AccountTypeId, userId);
        if (existingAccountType == null)
        {
            return RedirectToAction(actionName: "NotFoundPage", controllerName: "Home");
        }

        accountType.UserId = userId;

        await _accountTypesRepository.UpdateAccountTypeAsync(accountType);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = _usersService.GetUserId();
        AccountTypeEntity? existingAccountType = await _accountTypesRepository.GetAccountTypeByIdAsync(id, userId);
        if (existingAccountType == null)
        {
            return NotFound("Account type not found");
        }
        await _accountTypesRepository.DeleteAccountTypeAsync(id, userId);
        return Ok();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reorder([FromBody] int[] accountTypeIds)
    {
        int userId = _usersService.GetUserId();

        IEnumerable<AccountTypeEntity> accountTypes = await _accountTypesRepository.GetAccountTypesAsync(userId);

        IEnumerable<int> accountTypeIdsArray = accountTypes.Select(at => at.AccountTypeId);

        List<int> accountTypeIdsDoNotBelongToUser = [.. accountTypeIds.Except(accountTypeIdsArray)];

        if (accountTypeIdsDoNotBelongToUser.Count != 0)
        {
            return Forbid("One or more account type IDs do not belong to the user.");
        }

        IEnumerable<AccountTypeEntity> accontTypesReorder = accountTypeIds.Select(static (accountTypeId, index) => new AccountTypeEntity { AccountTypeId = accountTypeId, OrderBy = index + 1 });

        await _accountTypesRepository.UpdateAccountTypeOrderByAsync(accontTypesReorder);

        return Ok();
    }
}
