
namespace WebBanAoo.Generics
{
    public class MapperGeneric<TEntity, TRequest, TResponse> : IMapper<TEntity, TRequest, TResponse>
    {
        public TEntity CreateToEntity(TRequest create)
        {
            throw new NotImplementedException();
        }

        public TEntity DeleteToEntity(TRequest delete)
        {
            throw new NotImplementedException();
        }

        public TResponse EntityToResponse(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public List<TResponse> EntityToResponseList(List<TResponse> entity)
        {
            throw new NotImplementedException();
        }

        public TEntity UpdateToEntity(TRequest update)
        {
            throw new NotImplementedException();
        }
    }
}
