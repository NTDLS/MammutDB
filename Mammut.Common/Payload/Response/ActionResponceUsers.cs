using Mammut.Common.Payload.Model;
using System.Collections.Generic;

namespace Mammut.Common.Payload.Response
{
    public class ActionResponceUsers : ActionResponseBase
    {
        public List<User> List { get; set; }
    }
}
