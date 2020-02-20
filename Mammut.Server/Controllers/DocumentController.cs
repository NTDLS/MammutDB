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
using Mammut.Server.Core.Models.Persist;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Mammut.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DocumentController : ControllerBase
    {
        private IServerCore _core;

        private readonly ILogger<DocumentController> _logger;

        public DocumentController(ILogger<DocumentController> logger, IServerCore core)
        {
            _logger = logger;
            _core = core;
        }

        [HttpPost]
        public ActionResponseId Create([FromBody]ActionRequestDocument action)
        {
            _logger.LogDebug($"API:{MammutUtility.GetCurrentMethod()}");

            var result = new ActionResponseId();
            var session = _core.Session.ObtainSession(action.SessionId);

            try
            {
                result.Id = _core.Document.Create(session, action.Path, action.Document);
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
        public ActionResponseDocument GetById([FromBody]ActionRequestDocument action)
        {
            _logger.LogDebug($"API:{MammutUtility.GetCurrentMethod()}");

            var result = new ActionResponseDocument();

            var session = _core.Session.ObtainSession(action.SessionId);

            try
            {
                var metaDocument = _core.Document.GetById(session, action.Path, action.Id);
                result.Document = MetaDocument.ToPayload(metaDocument);
                result.Id = metaDocument.Id;
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
