using System;
using System.Collections.Generic;

namespace CQRSBus.DI {
    public class SimpleInjector : IInjector {
        private readonly Dictionary<Type, Func<object>> createMethodMap = new Dictionary<Type, Func<object>>();
        private readonly Dictionary<Type, object> instanceMap = new Dictionary<Type, object>();

        public void Register<TInstance>(Func<TInstance> createMethod)
            where TInstance : class
        {
            var instanceType = typeof(TInstance);
            createMethodMap[instanceType] = createMethod;
        }

        public void RegisterInstance<TInstance>(TInstance instance)
        {
            var instanceType = instance.GetType();
            instanceMap[instanceType] = instance;
        }

        public TInstance Resolve<TInstance>()
        {
            var instanceType = typeof(TInstance);
            return (TInstance) Resolve(instanceType);
        }

        public object Resolve(Type type)
        {
            if (instanceMap.TryGetValue(type, out var instance)) {
                return instance;
            }

            if (createMethodMap.TryGetValue(type, out var createMethod)) {
                return createMethod();
            }

            throw new Exception($"Unregistered type(Type:{type}).");
        }
    }
}
