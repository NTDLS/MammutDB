using System;
using System.Collections.Generic;
using System.Text;
using static Mammut.Server.Core.Constants;

namespace Mammut.Server.Core.Models
{
    public class WorkingConditionValue
    {
        public ConditionValueType ValueType { get; set; }
        public object Value { get; set; }

        public WorkingConditionValue()
        {
        }

        public WorkingConditionValue(ConditionValueType valueType, object value)
        {
            ValueType = valueType;
            Value = value;
        }
    }
}
