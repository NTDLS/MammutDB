using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mamoth.Common.Payload.Model
{
    public class Session
    {
        public Guid SessionId { get; set; }
        public Guid LoginId { get; set; }
    }
}
