using System;

namespace CQRSBus.DI {
    public interface IInjector
    {
        TInstance Resolve<TInstance>();
        object Resolve(Type type);
    }
}
