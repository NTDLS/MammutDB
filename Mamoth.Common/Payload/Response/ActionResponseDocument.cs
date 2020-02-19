using Mamoth.Common.Payload.Model;
using System;

namespace Mamoth.Common.Payload.Response
{
    public class ActionResponseDocument : ActionResponseBase
    {
        public Guid Id { get; set; }
        public Document Document { get; set; }
    }
}