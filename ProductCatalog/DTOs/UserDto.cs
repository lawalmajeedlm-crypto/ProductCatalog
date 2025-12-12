namespace ProductCatalog.DTOs
{
    public record RegisterRequest(
        string Email,
        string FullName,
        string Password
    );

    public record LoginRequest(
        string Email,
        string Password
    );

    public record LogoutRequest(
        string RefreshToken
    );

    public record LoginResponse(
        string AccessToken,
        string RefreshToken,
        DateTime ExpiresAt
    );

    public record UserDto(
        Guid Id,
        string Email,
        string FullName,
        string Role
    );

    public record UpdateUserRequest(
        string? Email,
        string? FullName,
        string? Role
        );
}
