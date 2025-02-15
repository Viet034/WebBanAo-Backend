namespace WebBanAoo.Generics
{
    public interface IMapper<TEntity,TRequest, TResponse>
    {
        //request to entity
        public TEntity CreateToEntity(TRequest create);
        public TEntity UpdateToEntity(TRequest update);
        public TEntity DeleteToEntity(TRequest delete);
        //entity to response
        public TResponse EntityToResponse(TEntity entity);
        public List<TResponse> EntityToResponseList(List<TResponse> entity);
    }
}
