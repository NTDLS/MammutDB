using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mamoth.Common;
using Mamoth.Common.Payload.Request;
using Mamoth.Common.Payload.Response;
using MamothDB.Server.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MamothDB.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SecurityController : ControllerBase
    {
        private IServerCore _core;

        private readonly ILogger<SecurityController> _logger;

        public SecurityController(ILogger<SecurityController> logger, IServerCore core)
        {
            _logger = logger;
            _core = core;
        }

        [HttpPost]
        public ActionResponceLogin Login([FromBody]ActionRequestLogin action)
        {
            _logger.LogDebug($"API:{MamothUtility.GetCurrentMethod()}");

            var result = new ActionResponceLogin();

            try
            {
                var session = _core.Security.Login(action.Login);
                result.SessionId = session.SessionId;
                result.LoginId = session.LoginId;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Call failed with an exception: " + ex.Message;
                _logger.LogError(result.Message);
            }
            return result;
        }

        [HttpPost]
        public ActionResponseBase Logout([FromBody]ActionRequestBase action)
        {
            _logger.LogDebug($"API:{MamothUtility.GetCurrentMethod()}");

            var result = new ActionResponseBase();

            try
            {
                _core.Security.Logout(action.SessionId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Call failed with an exception: " + ex.Message;
                _logger.LogError(result.Message);
            }
            return result;
        }
    }
}
