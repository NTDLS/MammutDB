using System;

namespace Mamoth.Common.Payload.Response
{
    public class ActionResponseDocument : ActionResponseBase
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
    }
}