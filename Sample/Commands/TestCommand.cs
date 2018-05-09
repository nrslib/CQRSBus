using CQRSBus.Commands;

namespace Sample.Commands {
    public class TestCommand : ICommand {
        public TestCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
