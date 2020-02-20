namespace Mammut.Common.Payload.Response
{
    class ActionResponseBoolean
    {
        public bool Value { get; set; }

        public ActionResponseBoolean()
        {
        }

        public ActionResponseBoolean(bool value)
        {
            Value = value;
        }
    }
}
