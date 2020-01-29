using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MamothDB.Server.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MamothDB.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiagController : ControllerBase
    {
        private IServerCore _core;

        private readonly ILogger<DiagController> _logger;

        public DiagController(ILogger<DiagController> logger, IServerCore core, IServerCoreSettings settings)
        {
            _logger = logger;
            _core = core;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var table = new StringBuilder();

            table.Append("<table border=\"1\">");
            table.Append("<tr>");
            table.Append(" <td><strong>Setting</strong></td>");
            table.Append(" <td><strong>Value</strong></td>");
            table.Append("</tr>");
            table.Append("<tr>");
            table.Append(" <td><strong>RootPath</strong></td>");
            table.Append($" <td>{_core.Settings.RootPath}</td>");
            table.Append("</tr>");
            table.Append("<tr>");
            table.Append(" <td><strong>ConfigFile</strong></td>");
            table.Append($" <td>{_core.Settings.ConfigFile}</td>");
            table.Append("</tr>");
            table.Append("<tr>");
            table.Append(" <td><strong>DataPath</strong></td>");
            table.Append($" <td>{_core.Settings.DataPath}</td>");
            table.Append("</tr>");
            table.Append("<tr>");
            table.Append(" <td><strong>UndoPath</strong></td>");
            table.Append($" <td>{_core.Settings.UndoPath}</td>");
            table.Append("</tr>");
            table.Append("</table>");

            return Content(table.ToString(), "text/html");
        }
    }
}
