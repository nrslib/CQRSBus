using System;
using CQRSBus.Commands;

namespace Sample.Commands {
    public class ConsoleLogHandler : ICommandHandler<TestCommand> {
        public void Handle(TestCommand command)
        {
            Console.WriteLine($"TestCommand received(Name:{command.Name}).");
        }
    }
}
