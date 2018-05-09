using CQRSBus.Commands;
using CQRSBus.DI;
using Sample.Commands;

namespace Sample {
    class Program {
        static void Main(string[] args)
        {
            var injector = new SimpleInjector();
            injector.Register<ICommandHandler<TestCommand>>(() => new ConsoleLogHandler());

            var commandBus = new CommandBus(injector);

            var command = new TestCommand("srnlib");
            commandBus.Handle(command);
        }
    }
}
