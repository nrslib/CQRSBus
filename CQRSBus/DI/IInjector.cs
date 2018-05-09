namespace CQRSBus.DI {
    public interface IInjector
    {
        TInstance Resolve<TInstance>();
    }
}
