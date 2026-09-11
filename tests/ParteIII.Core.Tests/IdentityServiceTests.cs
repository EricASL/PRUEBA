using AutoMapper;
using Moq;
using ParteIII.Core.Dtos;
using ParteIII.Core.Entities;
using ParteIII.Core.Repositories;
using ParteIII.Core.Services;

namespace ParteIII.Core.Tests;

public sealed class IdentityServiceTests
{
    private readonly Mock<IIdentityRepository> _repository = new();
    private readonly IdentityService _service;

    public IdentityServiceTests()
    {
        _service = new IdentityService(_repository.Object, Mock.Of<IMapper>());
    }

    [Fact]
    public async Task AddToBlacklistAsync_IdentityDoesNotExist_ReturnsError()
    {
        var identityId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        _repository
            .Setup(repository => repository.GetByIdAsync(identityId, companyId))
            .ReturnsAsync(ResultDto<Identity>.Fail("No encontrada"));

        var result = await _service.AddToBlacklistAsync(identityId, companyId, "Fraude");

        Assert.False(result.Success);
        Assert.Contains("Identidad no encontrada", result.Errors);
        _repository.Verify(repository => repository.UpdateAsync(It.IsAny<Identity>()), Times.Never);
    }

    [Fact]
    public async Task AddToBlacklistAsync_IdentityAlreadyBlacklisted_ReturnsError()
    {
        var identity = CreateIdentity(isBlacklisted: true);
        _repository
            .Setup(repository => repository.GetByIdAsync(identity.Id, identity.CompanyId))
            .ReturnsAsync(ResultDto<Identity>.Ok(identity));

        var result = await _service.AddToBlacklistAsync(identity.Id, identity.CompanyId, "Fraude");

        Assert.False(result.Success);
        Assert.Contains("La identidad ya está en blacklist", result.Errors);
        _repository.Verify(repository => repository.UpdateAsync(It.IsAny<Identity>()), Times.Never);
    }

    [Fact]
    public async Task AddToBlacklistAsync_ValidIdentity_UpdatesAndReturnsTrue()
    {
        var identity = CreateIdentity(isBlacklisted: false);
        _repository
            .Setup(repository => repository.GetByIdAsync(identity.Id, identity.CompanyId))
            .ReturnsAsync(ResultDto<Identity>.Ok(identity));

        var result = await _service.AddToBlacklistAsync(identity.Id, identity.CompanyId, "  Fraude  ");

        Assert.True(result.Success);
        Assert.True(result.Value);
        Assert.True(identity.IsBlacklisted);
        Assert.Equal("Fraude", identity.BlacklistReason);
        _repository.Verify(repository => repository.UpdateAsync(identity), Times.Once);
    }

    [Fact]
    public async Task AddToBlacklistAsync_EmptyReason_ReturnsErrorWithoutQueryingRepository()
    {
        var result = await _service.AddToBlacklistAsync(Guid.NewGuid(), Guid.NewGuid(), "   ");

        Assert.False(result.Success);
        Assert.Contains("El motivo es obligatorio", result.Errors);
        _repository.Verify(
            repository => repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
            Times.Never);
    }

    private static Identity CreateIdentity(bool isBlacklisted) => new()
    {
        Id = Guid.NewGuid(),
        CompanyId = Guid.NewGuid(),
        IsBlacklisted = isBlacklisted
    };
}