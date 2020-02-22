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
    public class QueryController : ControllerBase
    {
        private IServerCore _core;

        private readonly ILogger<QueryController> _logger;

        public QueryController(ILogger<QueryController> logger, IServerCore core)
        {
            _logger = logger;
            _core = core;
        }

        [HttpPost]
        public ActionResponseBase ExecuteDummy([FromBody]ActionRequestBase action)
        {
            _logger.LogDebug($"API:{MammutUtility.GetCurrentMethod()}");

            var result = new ActionResponseBase();
            var session = _core.Session.ObtainSession(action.SessionId);

            try
            {
                _core.Query.ExecuteDummy(session);

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
