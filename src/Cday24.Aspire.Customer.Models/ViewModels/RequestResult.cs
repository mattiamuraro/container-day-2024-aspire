namespace Cday24.Aspire.Customer.Models.ViewModels
{
    public class RequestResult<TValue>
    {
        public bool Result { get; set; }

        public string? Message { get; set; }

        public TValue? Value { get; set; }

        public static RequestResult<TValue> Success(TValue value)
        {
            return new RequestResult<TValue>
            {
                Result = true,
                Value = value
            };
        }

        public static RequestResult<TValue> Fail(string message)
        {
            return new RequestResult<TValue>
            {
                Result = false,
                Message = message
            };
        }
    }
}
