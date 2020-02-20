using System;

namespace Mammut.Common.Payload.Model
{
    public class Session
    {
        public Guid SessionId { get; set; }
        public Guid LoginId { get; set; }
        public string Username { get; set; }
    }
}
