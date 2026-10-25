using Dapper;
using Microsoft.Data.SqlClient;
using money_manager.Entities;
using System.Data;

namespace money_manager.Repositories;

public interface IAccountTypeRepository
{
    Task<bool> AccountTypeExistsAsync(string accountTypeName, int userId, int accountTypeId = 0);
    Task CreateAccountTypeAsync(AccountTypeEntity accountType);
    Task DeleteAccountTypeAsync(int accountTypeId, int userId);
    Task<AccountTypeEntity?> GetAccountTypeByIdAsync(int accountTypeId, int userId);
    Task<IEnumerable<AccountTypeEntity>> GetAccountTypesAsync(int userId);
    Task UpdateAccountTypeAsync(AccountTypeEntity accountType);
    Task UpdateAccountTypeOrderByAsync(IEnumerable<AccountTypeEntity> accountTypes);
}

public class AccountTypeRepository(IConfiguration configuration, ILogger<AccountTypeRepository> logger) : IAccountTypeRepository
{
    private readonly ILogger<AccountTypeRepository> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private string _connectionString = string.Empty;

    private string GetConnectionString()
    {
        _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
        return _connectionString;
    }

    public async Task CreateAccountTypeAsync(AccountTypeEntity accountType)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_insert_account_type";
            int newAccountTypeId = await connection.QuerySingleAsync<int>(query, new { account_type_name = accountType.AccountTypeName, user_id = accountType.UserId }, commandType: CommandType.StoredProcedure);
            accountType.AccountTypeId = newAccountTypeId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating account type for user {UserId} with name {AccountTypeName}", accountType.UserId, accountType.AccountTypeName);
        }

    }

    public async Task<bool> AccountTypeExistsAsync(string accountTypeName, int userId, int accountTypeId = 0)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "SELECT 1 FROM account_types WHERE account_type_name = @AccountTypeName AND user_id = @UserId AND account_type_id <> @AccountTypeId";
            bool exists = await connection.QueryFirstOrDefaultAsync<bool>(query, new { AccountTypeName = accountTypeName, UserId = userId, AccountTypeId = accountTypeId });
            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if account type exists for user {UserId} with name {AccountTypeName}", userId, accountTypeName);
            return false;
        }
    }

    public async Task<IEnumerable<AccountTypeEntity>> GetAccountTypesAsync(int userId)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "SELECT account_type_id AS AccountTypeId, account_type_name AS AccountTypeName, user_id AS UserId, order_by AS OrderBy FROM account_types WHERE user_id = @UserId ORDER BY order_by";
            IEnumerable<AccountTypeEntity> accountTypes = await connection.QueryAsync<AccountTypeEntity>(query, new { UserId = userId });
            return accountTypes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving account types for user {UserId}", userId);
            return [];
        }
    }

    public async Task UpdateAccountTypeAsync(AccountTypeEntity accountType)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_update_account_type";
            await connection.ExecuteAsync(query, new
            {
                account_type_id = accountType.AccountTypeId,
                account_type_name = accountType.AccountTypeName,
                order_by = accountType.OrderBy,
                user_id = accountType.UserId
            }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating account type for user {UserId} with ID {AccountTypeId}", accountType.UserId, accountType.AccountTypeId);
        }
    }

    public async Task DeleteAccountTypeAsync(int accountTypeId, int userId)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "DELETE FROM account_types WHERE account_type_id = @AccountTypeId AND user_id = @UserId";
            await connection.ExecuteAsync(query, new { AccountTypeId = accountTypeId, UserId = userId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting account type for user {UserId} with ID {AccountTypeId}", userId, accountTypeId);
        }
    }

    public async Task<AccountTypeEntity?> GetAccountTypeByIdAsync(int accountTypeId, int userId)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "SELECT account_type_id AS AccountTypeId, account_type_name AS AccountTypeName, user_id AS UserId, order_by AS OrderBy FROM account_types WHERE account_type_id = @AccountTypeId AND user_id = @UserId";
            AccountTypeEntity? accountType = await connection.QueryFirstOrDefaultAsync<AccountTypeEntity>(query, new { AccountTypeId = accountTypeId, UserId = userId });
            return accountType;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving account type for user {UserId} with ID {AccountTypeId}", userId, accountTypeId);
            return null;
        }
    }

    public async Task UpdateAccountTypeOrderByAsync(IEnumerable<AccountTypeEntity> accountTypes)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "UPDATE account_types SET order_by = @OrderBy WHERE account_type_id = @AccountTypeId";
            foreach (AccountTypeEntity accountType in accountTypes)
            {
                await connection.ExecuteAsync(query, new { accountType.OrderBy, accountType.AccountTypeId, accountType.UserId });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating account type order for user {UserId}", accountTypes.FirstOrDefault()?.UserId);
        }
    }
}
