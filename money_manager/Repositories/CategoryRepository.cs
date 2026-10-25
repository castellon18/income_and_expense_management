using Dapper;
using Microsoft.Data.SqlClient;
using money_manager.Entities;
using money_manager.Models;
using System.Data;

namespace money_manager.Repositories;

public interface ICategoryRepository
{
    Task CreateCategoryAsync(CategoryEntity category);
    Task DeleteCategoryAsync(int categoryId);
    Task<IEnumerable<CategoryEntity>> GetCategoriesAsync(int userId, PaginationViewModel pagination);
    Task<CategoryEntity?> GetCategoryByIdAsync(int categoryId, int userId);
    Task<int> GetTotalCategoriesCountAsync(int userId);
    Task<IEnumerable<CategoryEntity>> SearchCategoriesByUserAndOperationTypeAsync(int userId, OperationTypeEntity operationType);
    Task UpdateCategoryAsync(CategoryEntity category);
}

public class CategoryRepository(IConfiguration configuration, ILogger<CategoryRepository> logger) : ICategoryRepository
{
    private readonly ILogger<CategoryRepository> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private string _connectionString = string.Empty;

    private string GetConnectionString()
    {
        _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
        return _connectionString;
    }

    public async Task CreateCategoryAsync(CategoryEntity category)
    {
        using SqlConnection connection = new(GetConnectionString());
        string query = "sp_insert_category";
        int newCategoryId = await connection.QuerySingleAsync<int>(query, new
        {
            category_name = category.CategoryName,
            operation_type_id = category.OperationTypeId,
            user_id = category.UserId
        }, commandType: CommandType.StoredProcedure);
        category.CategoryId = newCategoryId;
    }

    public async Task<IEnumerable<CategoryEntity>> GetCategoriesAsync(int userId, PaginationViewModel pagination)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_get_categories_by_user_id";
            return await connection.QueryAsync<CategoryEntity>(query, new
            {
                user_id = userId,
                records_to_skip = pagination.RecordsToSkip,
                records_per_page = pagination.RecordsPerPage
            }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving categories for user {UserId}", userId);
            return [];
        }
    }

    public async Task<CategoryEntity?> GetCategoryByIdAsync(int categoryId, int userId)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            var query = "sp_get_category_by_id";
            return await connection.QuerySingleOrDefaultAsync<CategoryEntity>(query, new
            {
                category_id = categoryId,
                user_id = userId
            }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving category {CategoryId} for user {UserId}", categoryId, userId);
            return null;
        }
    }

    public async Task<IEnumerable<CategoryEntity>> SearchCategoriesByUserAndOperationTypeAsync(int userId, OperationTypeEntity operationType)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_search_categories_by_user_and_type";
            return await connection.QueryAsync<CategoryEntity>(query, new
            {
                user_id = userId,
                operation_type_id = operationType
            }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching categories for user {UserId} and operation type {OperationType}", userId, operationType);
            return [];
        }
    }

    public async Task DeleteCategoryAsync(int categoryId)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_delete_category";
            await connection.ExecuteAsync(query, new
            {
                category_id = categoryId
            }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category {CategoryId}", categoryId);
        }
    }

    public async Task UpdateCategoryAsync(CategoryEntity category)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_update_category";
            await connection.ExecuteAsync(query, new
            {
                category_id = category.CategoryId,
                category_name = category.CategoryName,
                operation_type_id = category.OperationTypeId,
            }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category {CategoryId}", category.CategoryId);
        }
    }

    public async Task<int> GetTotalCategoriesCountAsync(int userId)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "SELECT COUNT(*) FROM Categories WHERE user_id = @user_id";
            return await connection.ExecuteScalarAsync<int>(query, new { user_id = userId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving total categories count for user {UserId}", userId);
            return 0;
        }
    }
}
