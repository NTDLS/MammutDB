using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mammut.Common;
using Mammut.Common.Payload.Request;
using Mammut.Common.Payload.Response;
using Mammut.Server.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Mammut.Server.Controllers
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
            _logger.LogDebug($"API:{MammutUtility.GetCurrentMethod()}");

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
            _logger.LogDebug($"API:{MammutUtility.GetCurrentMethod()}");

            var result = new ActionResponseBase();

            var session = _core.Session.ObtainSession(action.SessionId, false);

            try
            {
                _core.Security.Logout(session);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Call failed with an exception: " + ex.Message;
                _logger.LogError(result.Message);
            }
            finally
            {
                session.CommitImplicitTransaction();
            }
            return result;
        }
    }
}
