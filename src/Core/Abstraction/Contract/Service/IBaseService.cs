using System.Linq.Expressions;
using Abstraction.Base.Dto;
using Abstraction.Base.Response;
using Abstraction.Common.Wappers;

namespace Abstraction.Contracts.Service
{
    public interface IBaseService<TEntity>
    {
        Task<BaseResponse<string>> AddAsync<TDto>(TDto entity, CancellationToken cancellationToken = default) where TDto : BaseDto;
        Task<BaseResponse<string>> UpdateAsync<TDto>(TDto entity) where TDto : BaseDto;
        Task<BaseResponse<string>> DeleteAsync(int id);
        Task<BaseResponse<TDto>> GetByIdAsync<TDto>(int id, bool trackChanges) where TDto : BaseDto;
        Task<PaginatedResult<TDto>> GetAllPagedAsync<TDto>(int pageNumber, int pageSize, string? orderBy, bool trackChanges) where TDto : BaseDto;
        Task<BaseResponse<PaginatedResult<TDto>>> GetAllByConditionPagedAsync<TDto>(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize, string? orderBy, bool trackChanges) where TDto : BaseDto;
        IQueryable<TEntity> GetAllAsync(bool trackChanges);
        //Task<TEntity> GetByIdAsync(int id, bool trackChanges);
        IQueryable<TEntity> GetAllByConditionAsync(Expression<Func<TEntity, bool>> expression,  bool trackChanges);
    }
}
