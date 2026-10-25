namespace money_manager.Entities;

public class UserEntity
{
    public int UserId { get; set; }
    public string UserAccountName { get; set; } = string.Empty;
    public string NormalizedUserAccountName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NormalizedEmail { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}
