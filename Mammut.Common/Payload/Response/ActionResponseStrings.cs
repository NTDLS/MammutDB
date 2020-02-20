using System.Collections.Generic;

namespace Mammut.Common.Payload.Response
{
    public class ActionResponseStrings : ActionResponseBase
    {
        public List<string> Values { get; set; }

        public ActionResponseStrings()
        {
        }

        public ActionResponseStrings(IEnumerable<string> values)
        {
            Values.AddRange(values);
        }

        public void Add(string value)
        {
            Values.Add(value);
        }
    }
}
