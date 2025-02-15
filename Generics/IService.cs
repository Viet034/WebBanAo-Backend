using WebBanAoo.Models.Status;

namespace WebBanAoo.Generics;

public interface IService<TCreate, TUpdate, TEntity, TResponse, TID>
{

    public Task<TResponse> Create(TCreate create);
    public Task<TResponse> Update(TID id,TUpdate update);
    public Task Delete(TID id);
    public Task<TResponse> FindById(TID id);
    public Task<IEnumerable<TResponse>> GetAll();
    public Task<bool> IsExistByName(string name);
    public Task<bool> ChangeStatus(TID id, Status status);
}
