using CQRSBus.Queries;

namespace Sample.Queries {
    public class TestResponse : IResponse {
        public TestResponse(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
