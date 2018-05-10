using CQRSBus.Queries;

namespace Sample.Queries {
    public class TestQuery : IQuery<TestResponse>
    {
        public TestQuery(string calledBy)
        {
            CalledBy = calledBy;
        }

        public string CalledBy { get; }
    }
}
