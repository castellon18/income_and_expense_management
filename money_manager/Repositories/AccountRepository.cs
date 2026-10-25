using Dapper;
using Microsoft.Data.SqlClient;
using money_manager.Entities;
using System.Data;

namespace money_manager.Repositories;

public interface IAccountRepository
{
    Task CreateAccountAsync(AccountEntity accountModel);
    Task DeleteAccountAsync(int accountId);
    Task<AccountEntity?> GetAccountByIdAsync(int accountId, int userId);
    Task<IEnumerable<AccountEntity>> SearchAccountsByUserAsync(int userId);
    Task UpdateAccountAsync(AccountEntity accountModel);
}

public class AccountRepository(ILogger<AccountRepository> logger, IConfiguration configuration) : IAccountRepository
{
    private readonly ILogger<AccountRepository> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private string _connectionString = string.Empty;

    private string GetConnectionString()
    {
        _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
        return _connectionString;
    }

    public async Task CreateAccountAsync(AccountEntity accountModel)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_insert_account";
            int newAccountId = await connection.QuerySingleAsync<int>(query, new
            {
                account_name = accountModel.AccountName,
                account_type_id = accountModel.AccountTypeId,
                account_balance = accountModel.AccountBalance,
                account_description = accountModel.AccountDescription
            }, commandType: CommandType.StoredProcedure);
            accountModel.AccountId = newAccountId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating account.");
        }

    }

    public async Task<IEnumerable<AccountEntity>> SearchAccountsByUserAsync(int userId)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_search_accounts_by_user";
            return await connection.QueryAsync<AccountEntity>(query, new { user_id = userId }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching accounts by user.");
            return [];
        }

    }

    public async Task<AccountEntity?> GetAccountByIdAsync(int accountId, int userId)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_get_account_by_id";
            return await connection.QueryFirstOrDefaultAsync<AccountEntity>(query, new { account_id = accountId, user_id = userId }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting account by ID.");
            return null;
        }
    }

    public async Task UpdateAccountAsync(AccountEntity accountModel)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_update_account";
            await connection.ExecuteAsync(query, new
            {
                account_id = accountModel.AccountId,
                account_name = accountModel.AccountName,
                account_type_id = accountModel.AccountTypeId,
                account_balance = accountModel.AccountBalance,
                account_description = accountModel.AccountDescription
            }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating account.");
        }
    }

    public async Task DeleteAccountAsync(int accountId)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_delete_account";
            await connection.ExecuteAsync(query, new { account_id = accountId }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting account.");
        }

    }
}
