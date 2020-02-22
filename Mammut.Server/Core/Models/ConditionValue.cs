using System;
using System.Collections.Generic;
using System.Text;
using static Mammut.Server.Core.Constants;

namespace Mammut.Server.Core.Models
{
    public class ConditionValue
    {
        public ConditionValueType ValueType { get; set; }
        public object Value { get; set; }

        public ConditionValue()
        {
        }

        public ConditionValue(ConditionValueType valueType, object value)
        {
            ValueType = valueType;
            Value = value;
        }

        public WorkingConditionValue ToWorkingConditionValue()
        {
            return new WorkingConditionValue()
            {
                Value = this.Value,
                ValueType = this.ValueType
            };
        }
    }
}
