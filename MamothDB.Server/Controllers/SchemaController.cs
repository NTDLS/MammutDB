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
        public ActionResponseSchema Create([FromBody]ActionRequestSchema action)
        {
            _logger.LogDebug($"API:{MamothUtility.GetCurrentMethod()}");

            var result = new ActionResponseSchema();

            try
            {
                result.Name = action.Name;
                result.Id = _core.Schema.Create(action.Name);
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
