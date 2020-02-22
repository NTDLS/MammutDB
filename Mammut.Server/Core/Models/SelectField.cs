using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mammut.Server.Core.Models
{
    public class SelectField
    {
        public string Name { get; set; }
        public bool IsConstant { get; set; }
        public string Value { get; set; }
    }
}
