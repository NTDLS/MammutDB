using System;
using System.Collections.Generic;
using System.Text;
using static Mammut.Server.Core.Constants;

namespace Mammut.Server.Core.Models
{
	public class ConditionStatement
	{
		public ConditionType LogicalConnector { get; set; } = ConditionType.None;
		public ConditionValue LeftValue { get; set; }
		public ConditionQualifier Qualifier { get; set; }
		public ConditionValue RightValue { get; set; }

		public ConditionStatement()
		{
		}

		public ConditionStatement(ConditionType logicalConnector, ConditionValue leftValue, ConditionQualifier qualifier, ConditionValue rightValue)
		{
			LogicalConnector = logicalConnector;
			LeftValue = leftValue;
			Qualifier = qualifier;
			RightValue = rightValue;
		}

		public WorkingConditionStatement ToWorkingConditionStatement()
		{
			return new WorkingConditionStatement()
			{
				LogicalConnector = this.LogicalConnector,
				Qualifier = this.Qualifier,
				LeftValue = this.LeftValue.ToWorkingConditionValue(),
				RightValue = this.RightValue.ToWorkingConditionValue()
			};
		}
	}
}