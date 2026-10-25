using Dapper;
using Microsoft.Data.SqlClient;
using money_manager.Entities;
using money_manager.ParametersClass;
using System.Data;

namespace money_manager.Repositories;

public interface ITransactionRepository
{
    Task CreateTransactionAsync(TransactionEntity transaction);
    Task DeleteTransactionAsync(int transactionId);
    Task<IEnumerable<ExpectedResultsPerMonthEntity>> GetExpectedResultsPerMonthAsync(int userId, int year);
    Task<IEnumerable<ExpectedResultsPerWeekEntity>> GetExpectedResultsPerWeekAsync(GetTransactionsByUserParameter model);
    Task<TransactionEntity?> GetTransactionByIdAsync(int transactionId, int userId);
    Task<IEnumerable<TransactionEntity>> GetTransactionsByAccountIdAsync(GetTransactionsByAccountParameter model);
    Task<IEnumerable<TransactionEntity>> GetTransactionsByUserIdAsync(GetTransactionsByUserParameter model);
    Task UpdateTransactionAsync(TransactionEntity transaction, decimal previousAmount, int previousAccountId);
}

public class TransactionRepository(IConfiguration configuration, ILogger<TransactionRepository> logger) : ITransactionRepository
{
    private readonly ILogger<TransactionRepository> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private string _connectionString = string.Empty;

    private string GetConnectionString()
    {
        _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
        return _connectionString;
    }

    public async Task CreateTransactionAsync(TransactionEntity transaction)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_insert_transaction";
            int newTransactionId = await connection.QuerySingleAsync<int>(query, new
            {
                user_id = transaction.UserId,
                transaction_date = transaction.TransactionDate,
                amount = transaction.Amount,
                account_id = transaction.AccountId,
                category_id = transaction.CategoryId,
                transaction_note = transaction.TransactionNote
            }, commandType: CommandType.StoredProcedure);

            transaction.TransactionId = newTransactionId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a new transaction for user ID {UserId}.", transaction.UserId);
        }
    }

    public async Task UpdateTransactionAsync(TransactionEntity transaction, decimal previousAmount, int previousAccountId)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_update_transaction";
            await connection.ExecuteAsync(query, new
            {
                transaction_id = transaction.TransactionId,
                transaction_date = transaction.TransactionDate,
                amount = transaction.Amount,
                account_id = transaction.AccountId,
                category_id = transaction.CategoryId,
                transaction_note = transaction.TransactionNote,
                previous_amount = previousAmount,
                previous_account_id = previousAccountId
            }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating transaction with ID {TransactionId}.", transaction.TransactionId);
        }
    }

    public async Task<TransactionEntity?> GetTransactionByIdAsync(int transactionId, int userId)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_get_transaction_by_id";
            return await connection.QuerySingleOrDefaultAsync<TransactionEntity>(query, new { transaction_id = transactionId, user_id = userId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting transaction by ID.");
            return null;
        }
    }

    public async Task DeleteTransactionAsync(int transactionId)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_delete_transaction";
            await connection.ExecuteAsync(query, new { transaction_id = transactionId }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting transaction with ID {TransactionId}.", transactionId);
        }

    }

    public async Task<IEnumerable<TransactionEntity>> GetTransactionsByAccountIdAsync(GetTransactionsByAccountParameter model)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_get_transactions_by_account";
            return await connection.QueryAsync<TransactionEntity>(query, new
            {
                user_id = model.UserId,
                account_id = model.AccountId,
                start_date = model.StartDate,
                end_date = model.EndDate
            }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting transactions by account ID.");
            return [];
        }

    }

    public async Task<IEnumerable<TransactionEntity>> GetTransactionsByUserIdAsync(GetTransactionsByUserParameter model)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_get_transactions_by_user";
            return await connection.QueryAsync<TransactionEntity>(query, new
            {
                user_id = model.UserId,
                start_date = model.StartDate,
                end_date = model.EndDate
            }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting transactions by user ID.");
            return [];
        }
    }

    public async Task<IEnumerable<ExpectedResultsPerWeekEntity>> GetExpectedResultsPerWeekAsync(GetTransactionsByUserParameter model)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_get_transactions_by_week";
            return await connection.QueryAsync<ExpectedResultsPerWeekEntity>(query, new
            {
                user_id = model.UserId,
                start_date = model.StartDate,
                end_date = model.EndDate
            }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting expected results per week for user ID {UserId}.", model.UserId);
            return [];
        }
    }

    public async Task<IEnumerable<ExpectedResultsPerMonthEntity>> GetExpectedResultsPerMonthAsync(int userId, int year)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_get_transactions_by_month";
            return await connection.QueryAsync<ExpectedResultsPerMonthEntity>(query, new
            {
                user_id = userId,
                year
            }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting expected results per month for user ID {UserId} and year {Year}.", userId, year);
            return [];
        }
    }
}
