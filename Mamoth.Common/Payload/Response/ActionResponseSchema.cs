using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mamoth.Common.Payload.Response
{
    public class ActionResponseSchema : ActionResponseBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

    }
}