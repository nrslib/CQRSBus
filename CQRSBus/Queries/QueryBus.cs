using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSBus.DI;

namespace CQRSBus.Queries {
    public class QueryBus {
        private readonly IInjector injector;
        private readonly Dictionary<Type, Type> handlerTypes = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Func<object>> handlers = new Dictionary<Type, Func<object>>();
        private readonly Dictionary<Type, Func<object, object>> queryHandler = new Dictionary<Type, Func<object, object>>();

        public QueryBus(IInjector container) {
            this.injector = container;
        }

        public void Register<TRequest, TUseCase>()
            where TRequest : IQuery<IResponse>
            where TUseCase : IQueryHandler<TRequest, IResponse> {
            handlerTypes.Add(typeof(TRequest), typeof(TUseCase));
        }

        public void RegisterInstance<TQuery>(IQueryHandler<TQuery, IResponse> handler)
            where TQuery : IQuery<IResponse>
        {
            var queryType = typeof(TQuery);
            Func<object, object> callerHandler = query => handler.Handle((TQuery) query);
            queryHandler[queryType] = callerHandler;
        }

        public TResponse Handle<TResponse>(IQuery<TResponse> query)
            where TResponse : IResponse {
            var invokeMethod = InvokeHandler(query);
            var result = invokeMethod();
            return (TResponse)result;
        }

        public async Task<TResponse> HandleAync<TResponse>(IQuery<TResponse> query)
            where TResponse : IResponse {
            var invokeMethod = InvokeHandler(query);
            var result = await Task.Run(invokeMethod);
            return (TResponse)result;
        }

        private Func<object> InvokeHandler<TResponse>(IQuery<TResponse> query)
            where TResponse : IResponse {
            var queryType = query.GetType();
            if (handlers.TryGetValue(queryType, out var searchedHandler)) {
                return searchedHandler;
            }

            if (queryHandler.TryGetValue(queryType, out var searchedQueryHandler))
            {
                return () => searchedQueryHandler(query);
            }

            if (handlerTypes.TryGetValue(queryType, out var handlerType))
            {
                var handlerObject = injector.Resolve(handlerType);
                var method = handlerObject.GetType().GetMethod("Handle");

                Func<object> handler = () => {
                    var result = method.Invoke(handlerObject, new object[] { query });
                    return (TResponse)result;
                };
                handlers[queryType] = handler;
                return handler;
            }

            throw new Exception($"Unregistered Handler(Type:{queryType}).");
        }
    }
}
