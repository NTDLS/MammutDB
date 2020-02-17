using Mamoth.Common.Payload.Model;
using System.Collections.Generic;

namespace Mamoth.Common.Payload.Response
{
    public class ActionResponceUsers : ActionResponseBase
    {
        public List<User> List { get; set; }
    }
}
