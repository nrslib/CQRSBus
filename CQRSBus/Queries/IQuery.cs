namespace CQRSBus.Queries {
    public interface IQuery<out TResponse> where TResponse : IResponse {
    }
}
