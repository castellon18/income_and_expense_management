using Microsoft.AspNetCore.Mvc;
using money_manager.Entities;
using money_manager.Models;
using money_manager.Repositories;
using money_manager.Services;

namespace money_manager.Controllers;

public class CategoriesController(ICategoryRepository categoryRepository, IUsersService usersService) : Controller
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IUsersService _usersService = usersService;

    [HttpGet]
    public async Task<IActionResult> Index(PaginationViewModel paginationViewModel)
    {
        int userId = _usersService.GetUserId();
        IEnumerable<CategoryEntity> categories = await _categoryRepository.GetCategoriesAsync(userId, paginationViewModel);
        int totalCategoriesCount = await _categoryRepository.GetTotalCategoriesCountAsync(userId);
        ResponsePaginationViewModel<CategoryEntity> response = new()
        {
            Records = categories,
            Page = paginationViewModel.Page,
            RecordsPerPage = paginationViewModel.RecordsPerPage,
            TotalRecords = totalCategoriesCount,
            UrlBase = Url.Action(nameof(Index)) ?? string.Empty
        };
        return View(response);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryEntity category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        int userId = _usersService.GetUserId();
        category.UserId = userId;
        await _categoryRepository.CreateCategoryAsync(category);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        int userId = _usersService.GetUserId();
        CategoryEntity? category = await _categoryRepository.GetCategoryByIdAsync(id, userId);
        if (category is null)
        {
            return RedirectToAction(actionName: "NotFoundPage", controllerName: "Home");
        }
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryEntity category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        int userId = _usersService.GetUserId();
        CategoryEntity? categoryExists = await _categoryRepository.GetCategoryByIdAsync(category.CategoryId, userId);

        if (categoryExists is null)
        {
            return RedirectToAction(actionName: "NotFoundPage", controllerName: "Home");
        }

        category.UserId = userId;
        await _categoryRepository.UpdateCategoryAsync(category);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = _usersService.GetUserId();
        CategoryEntity? category = await _categoryRepository.GetCategoryByIdAsync(id, userId);
        if (category == null)
        {
            return NotFound("Category not found");
        }
        await _categoryRepository.DeleteCategoryAsync(id);
        return Ok();
    }
}
