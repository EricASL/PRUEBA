namespace ParteIII.Core.Entities;

public sealed class Identity
{
    public Guid Id { get; init; }

    public Guid CompanyId { get; init; }

    public bool IsBlacklisted { get; set; }

    public string? BlacklistReason { get; set; }
}