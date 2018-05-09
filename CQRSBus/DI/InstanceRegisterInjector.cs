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
            if (instanceMap.TryGetValue(instanceType, out var instance))
            {
                return (TInstance)instance;
            }

            if (createMethodMap.TryGetValue(instanceType, out var createMethod))
            {
                return (TInstance) createMethod();
            }

            throw new Exception($"Unregistered type(Type:{instanceType}).");
        }
    }
}
