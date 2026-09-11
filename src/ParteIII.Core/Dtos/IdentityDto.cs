namespace ParteIII.Core.Dtos;

public sealed class IdentityDto
{
    public Guid Id { get; init; }

    public Guid CompanyId { get; init; }

    public bool IsBlacklisted { get; init; }

    public string? BlacklistReason { get; init; }
}