using Mammut.Common.Payload.Response;
using System;

namespace Mammut.Common.Payload.Model
{
    public class LoginToken
    {
        public Guid SessionId { get; set; }

        private bool isValid = false;
        public bool IsValid
        {
            get
            {
                return isValid;
            }
        }

        public LoginToken()
        {
            SessionId = Guid.Empty;
        }

        public LoginToken(ActionResponceLogin response)
        {
            if (response.Success == false)
            {
                throw new Exception("The login response does not contain a valid login token.");
            }

            this.SessionId = response.SessionId;

            this.isValid = true;
        }
    }
}
