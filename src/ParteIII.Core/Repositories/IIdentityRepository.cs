using ParteIII.Core.Dtos;
using ParteIII.Core.Entities;

namespace ParteIII.Core.Repositories;

public interface IIdentityRepository
{
    Task<ResultDto<Identity>> GetByIdAsync(Guid id, Guid companyId);

    Task UpdateAsync(Identity identity);
}