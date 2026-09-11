using AutoMapper;
using ParteIII.Core.Dtos;
using ParteIII.Core.Repositories;

namespace ParteIII.Core.Services;

public sealed class IdentityService
{
    private readonly IIdentityRepository _identityRepo;
    private readonly IMapper _mapper;

    public IdentityService(IIdentityRepository identityRepo, IMapper mapper)
    {
        _identityRepo = identityRepo;
        _mapper = mapper;
    }

    public async Task<ResultDto<IdentityDto>> GetIdentityAsync(Guid id, Guid companyId)
    {
        var result = await _identityRepo.GetByIdAsync(id, companyId);

        if (!result.Success || result.Value is null)
        {
            return ResultDto<IdentityDto>.Fail(result.Errors);
        }

        if (result.Value.CompanyId != companyId)
        {
            return ResultDto<IdentityDto>.Fail("Acceso no autorizado");
        }

        return ResultDto<IdentityDto>.Ok(_mapper.Map<IdentityDto>(result.Value));
    }

    public async Task<ResultDto<bool>> AddToBlacklistAsync(
        Guid identityId,
        Guid companyId,
        string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            return ResultDto<bool>.Fail("El motivo es obligatorio");
        }

        var identity = await _identityRepo.GetByIdAsync(identityId, companyId);

        if (!identity.Success || identity.Value is null)
        {
            return ResultDto<bool>.Fail("Identidad no encontrada");
        }

        if (identity.Value.CompanyId != companyId)
        {
            return ResultDto<bool>.Fail("Acceso no autorizado");
        }

        if (identity.Value.IsBlacklisted)
        {
            return ResultDto<bool>.Fail("La identidad ya está en lista negra");
        }

        identity.Value.IsBlacklisted = true;
        identity.Value.BlacklistReason = reason.Trim();

        await _identityRepo.UpdateAsync(identity.Value);

        return ResultDto<bool>.Ok(true);
    }
}