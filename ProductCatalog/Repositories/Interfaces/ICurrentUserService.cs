namespace ProductCatalog.Abstractions
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
    }
}