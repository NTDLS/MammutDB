using System;

namespace Mammut.Common.Payload.Response
{
    public class ActionResponseSchema : ActionResponseBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}