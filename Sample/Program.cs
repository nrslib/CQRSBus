using System;
using CQRSBus.Commands;
using CQRSBus.DI;
using CQRSBus.Queries;
using Sample.Commands;
using Sample.Queries;

namespace Sample {
    class Program {
        static void Main(string[] args)
        {
            RunTest(SampleCommandHandler, "Command handler with dependency injection.");
            RunTest(SampleCommandHandlerWithInstance, "Command handler with create instance.");
            RunTest(SampleQueryHandler, "Query handler with create instance.");
            RunTest(SampleQueryHandlerWithInstance, "Query handler with create instance.");
            
            Console.WriteLine("Any key press to exit.");
            Console.ReadKey();
        }

        private static void RunTest(Action testFunc, string startMessage)
        {
            Console.WriteLine("--- " + startMessage);
            testFunc();
            Console.WriteLine();
        }

        private static void SampleCommandHandler()
        {
            var injector = new SimpleInjector();
            injector.Register<ICommandHandler<TestCommand>>(() => new ConsoleLogHandler());

            var commandBus = new CommandBus(injector);
            commandBus.Register<TestCommand, ICommandHandler<TestCommand>>();

            var command = new TestCommand("srnlib");
            commandBus.Handle(command);
        }

        private static void SampleCommandHandlerWithInstance() {
            var injector = new SimpleInjector();
            var commandBus = new CommandBus(injector);
            commandBus.RegisterInstance(new ConsoleLogHandler());

            var command = new TestCommand("srnlib");
            commandBus.Handle(command);
        }

        private static void SampleQueryHandler()
        {
            var injector = new SimpleInjector();
            injector.Register<IQueryHandler<TestQuery, TestResponse>>(() => new TestQueryHandler());

            var queryBus = new QueryBus(injector);
            queryBus.Register<TestQuery, IQueryHandler<TestQuery, TestResponse>>();

            var query = new TestQuery("srn");
            var response = queryBus.Handle(query);

            Console.WriteLine(response.Message);
        }

        private static void SampleQueryHandlerWithInstance()
        {
            var injector = new SimpleInjector();

            var queryBus = new QueryBus(injector);
            queryBus.RegisterInstance(new TestQueryHandler());

            var query = new TestQuery("srn");
            var response = queryBus.Handle(query);

            Console.WriteLine(response.Message);
        }
    }
}
