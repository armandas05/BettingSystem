namespace BettingSystem.Domain;

public sealed class BettingValidationException : Exception
{
    public BettingValidationException(string message) : base(message) { }
}

public static class Validation
{
    public static void PositiveAmount(decimal amount, string name = "Amount")
    {
        if (amount <= 0) throw new BettingValidationException($"{name} must be positive.");
    }
}

[Flags]
public enum Permissions { None = 0, Play = 1 << 0, ManageUsers = 1 << 1, ViewAnalytics = 1 << 2 }

public static class PermissionExtensions
{
    public static Permissions ToPermissions(this string? role) => role switch
    {
        "Admin" => Permissions.Play | Permissions.ManageUsers | Permissions.ViewAnalytics,
        "User" => Permissions.Play,
        _ => Permissions.None
    };
    public static bool Includes(this Permissions permissions, Permissions required) =>
        (permissions & required) == required;
}
