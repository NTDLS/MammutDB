namespace Mamoth.Common.Payload.Response
{
    public class ActionResponseBase
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
