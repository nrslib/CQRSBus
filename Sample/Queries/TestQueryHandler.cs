using CQRSBus.Queries;

namespace Sample.Queries {
    public class TestQueryHandler : IQueryHandler<TestQuery, TestResponse> {
        public TestResponse Handle(TestQuery query)
        {
            var calledBy = query.CalledBy;
            var message = "TestQueryHandler called by " + calledBy;

            return new TestResponse(message);
        }
    }
}
