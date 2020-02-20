namespace Mammut.Common.Payload.Response
{
    public class ActionResponseString : ActionResponseBase
    {
        public string Value { get; set; }

        public ActionResponseString()
        {
        }

        public ActionResponseString(string value)
        {
            Value = value;
        }

        public void Set(string value)
        {
            Value = value;
        }
    }
}
