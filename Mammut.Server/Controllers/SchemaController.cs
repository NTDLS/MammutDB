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
    public class SchemaController : ControllerBase
    {
        private IServerCore _core;

        private readonly ILogger<SchemaController> _logger;

        public SchemaController(ILogger<SchemaController> logger, IServerCore core)
        {
            _logger = logger;
            _core = core;
        }

        [HttpPost]
        public ActionResponseBase CreateAll([FromBody]ActionRequestSchema action)
        {
            _logger.LogDebug($"API:{MammutUtility.GetCurrentMethod()}");

            var result = new ActionResponseBase();
            var session = _core.Session.ObtainSession(action.SessionId);

            try
            {
                var schemaInfo = _core.Schema.Parse(session, action.Path);
                var parts = schemaInfo.FullLogicalPath.Split(':');

                StringBuilder builtSchema = new StringBuilder();

                foreach (var part in parts)
                {
                    builtSchema.Append($"{part}:");
                    _core.Schema.Create(session, builtSchema.ToString().TrimEnd(':'));
                }

                //var schemaInfo = _core.Schema.Create(session, action.Path);
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
        public ActionResponseSchema Create([FromBody]ActionRequestSchema action)
        {
            _logger.LogDebug($"API:{MammutUtility.GetCurrentMethod()}");

            var result = new ActionResponseSchema();
            var session = _core.Session.ObtainSession(action.SessionId);

            try
            {
                var schemaInfo = _core.Schema.Create(session, action.Path);
                result.Name = schemaInfo.Name;
                result.Id = schemaInfo.Id;
                result.Path = schemaInfo.LogicalPath;
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
        public ActionResponseSchema Get([FromBody]ActionRequestSchema action)
        {
            _logger.LogDebug($"API:{MammutUtility.GetCurrentMethod()}");

            var result = new ActionResponseSchema();

            var session = _core.Session.ObtainSession(action.SessionId);

            try
            {
                var schemaInfo = _core.Schema.Get(session, action.Path);
                result.Name = schemaInfo.Name;
                result.Id = schemaInfo.Id;
                result.Path = schemaInfo.LogicalPath;
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
