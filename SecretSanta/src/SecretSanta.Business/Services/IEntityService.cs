using System.Collections.Generic;
using System.Threading.Tasks;
using SecretSanta.Business.Dto;

namespace SecretSanta.Business.Services
{
    public interface IEntityService<TDto, TInputDto>
        where TInputDto : class
        where TDto : class, TInputDto, IEntity
    {
        Task<List<TDto>> FetchAllAsync();
        Task<TDto> FetchByIdAsync(int id);
        Task<TDto> InsertAsync(TInputDto entity);
        Task<TDto?> UpdateAsync(int id, TInputDto entity);
        Task<bool> DeleteAsync(int id);
    }
}
