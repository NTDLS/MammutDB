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
        public ActionResponseDocument Create([FromBody]ActionRequestDocument action)
        {
            _logger.LogDebug($"API:{MamothUtility.GetCurrentMethod()}");

            var result = new ActionResponseDocument();
            var session = _core.Session.ObtainSession(action.SessionId);

            try
            {
                var documentInfo = _core.Document.Create(session, action.Path, action.Document);
                result.Id = documentInfo.Id;
                result.Path = documentInfo.LogicalPath;
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
            _logger.LogDebug($"API:{MamothUtility.GetCurrentMethod()}");

            var result = new ActionResponseDocument();

            var session = _core.Session.ObtainSession(action.SessionId);

            try
            {
                var documentInfo = _core.Document.GetById(session, action.Path, action.Id);
                result.Id = documentInfo.Id;
                result.Path = documentInfo.LogicalPath;
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
