namespace CQRSBus.Queries {
    public interface IQueryHandler<in TQuery, out TResponse> 
        where TQuery : IQuery<TResponse> 
        where TResponse : IResponse
    {
        TResponse Handle(TQuery query);
    }
}
