using Mamoth.Common.Payload.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mamoth.Common.Payload.Response
{
    public class ActionResponceLogin : ActionResponseBase
    {
        public Guid SessionId { get; set; }
        public Guid LoginId { get; set; }

        public LoginToken ToToken()
        {
            return new LoginToken(this);
        }
    }
}
