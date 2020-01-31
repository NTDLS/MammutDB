using Mamoth.Common.Payload.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mamoth.Common.Payload.Response
{
    public class ActionResponceUsers : ActionResponseBase
    {
        public List<User> List { get; set; }
    }
}
