using Dapper;
using Microsoft.Data.SqlClient;
using money_manager.Entities;
using System.Data;

namespace money_manager.Repositories;

public interface IUserRepository
{
    Task<int> CreateUserAsync(UserEntity user);
    Task<UserEntity?> FindUserByEmailAsync(string normalizedEmail);
    Task<UserEntity?> FindUserByUserNameAsync(string normalizedUserName);
    Task UpdatePasswordUserAsync(UserEntity user);
}

public class UserRepository(IConfiguration configuration, ILogger<UserRepository> logger) : IUserRepository
{
    private readonly ILogger<UserRepository> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private string _connectionString = string.Empty;

    private string GetConnectionString()
    {
        _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Cadena de conexión no encontrada");
        return _connectionString;
    }

    public async Task<int> CreateUserAsync(UserEntity user)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());

            string query = "sp_insert_user";

            int newUserId = await connection.QuerySingleAsync<int>(query, new
            {
                user_account_name = user.UserAccountName,
                normalized_user_account_name = user.NormalizedUserAccountName,
                email = user.Email,
                normalized_email = user.NormalizedEmail,
                password_hash = user.PasswordHash,
            }, commandType: CommandType.StoredProcedure);

            user.UserId = newUserId;

            _ = await connection.ExecuteAsync("sp_create_data_for_new_user", new { user_id = newUserId }, commandType: CommandType.StoredProcedure);

            return user.UserId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al insertar el nuevo usuario");
            return -1;
        }

    }

    public async Task<UserEntity?> FindUserByEmailAsync(string normalizedEmail)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_get_user_by_email";
            return await connection.QuerySingleOrDefaultAsync<UserEntity>(query, new
            {
                normalized_email = normalizedEmail
            }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar al usuario por el email");
            return null;
        }

    }

    public async Task<UserEntity?> FindUserByUserNameAsync(string normalizedUserName)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_get_user_by_account_name";
            return await connection.QuerySingleOrDefaultAsync<UserEntity>(query, new
            {
                normalized_user_account_name = normalizedUserName
            }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar al usuario por el nombre de usuario");
            return null;
        }

    }

    public async Task UpdatePasswordUserAsync(UserEntity user)
    {
        try
        {
            using SqlConnection connection = new(GetConnectionString());
            string query = "sp_update_password_user";
            await connection.ExecuteAsync(query, new
            {
                user_id = user.UserId,
                password_hash = user.PasswordHash
            }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el usuario");
        }
    }
}
