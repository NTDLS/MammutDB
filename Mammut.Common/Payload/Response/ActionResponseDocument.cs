using Mammut.Common.Payload.Model;
using System;

namespace Mammut.Common.Payload.Response
{
    public class ActionResponseDocument : ActionResponseBase
    {
        public Guid Id { get; set; }
        public Document Document { get; set; }
    }
}