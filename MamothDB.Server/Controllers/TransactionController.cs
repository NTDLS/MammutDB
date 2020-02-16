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
    public class TransactionController : ControllerBase
    {
        private IServerCore _core;

        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ILogger<TransactionController> logger, IServerCore core)
        {
            _logger = logger;
            _core = core;
        }

        [HttpPost]
        public ActionResponseBase Enlist([FromBody]ActionRequestBase action)
        {
            _logger.LogDebug($"API:{MamothUtility.GetCurrentMethod()}");

            var result = new ActionResponseBase();
            var session = _core.Session.ObtainSession(action.SessionId, false);

            try
            {
                _core.Transaction.Enlist(session);
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

        [HttpPost]
        public ActionResponseBase Commit([FromBody]ActionRequestBase action)
        {
            _logger.LogDebug($"API:{MamothUtility.GetCurrentMethod()}");

            var result = new ActionResponseBase();
            var session = _core.Session.ObtainSession(action.SessionId, false);

            try
            {
                _core.Transaction.Commit(session);
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

        [HttpPost]
        public ActionResponseBase Rollback([FromBody]ActionRequestBase action)
        {
            _logger.LogDebug($"API:{MamothUtility.GetCurrentMethod()}");

            var result = new ActionResponseBase();
            var session = _core.Session.ObtainSession(action.SessionId, false);

            try
            {
                _core.Transaction.Rollback(session);
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
