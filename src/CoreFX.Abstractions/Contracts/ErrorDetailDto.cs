namespace CoreFX.Abstractions.Contracts
{
    public class ErrorDetailDto
    {
        public int StatusCode { get; set; }
        public object Message { get; set; }
        public object Trace { get; set; }
    }
}
