using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mammut.Server.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Mammut.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiagController : ControllerBase
    {
        private IServerCore _core;

        private readonly ILogger<DiagController> _logger;

        public DiagController(ILogger<DiagController> logger, IServerCore core)
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
            table.Append(" <td><strong>SchemaPath</strong></td>");
            table.Append($" <td>{_core.Settings.SchemaPath}</td>");
            table.Append("</tr>");
            table.Append("<tr>");
            table.Append(" <td><strong>TransactionPath</strong></td>");
            table.Append($" <td>{_core.Settings.TransactionPath}</td>");
            table.Append("</tr>");
            table.Append("</table>");

            return Content(table.ToString(), "text/html");
        }
    }
}
