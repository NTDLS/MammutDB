using System;
using System.Collections.Generic;
using System.Text;
using static Mammut.Server.Core.Constants;

namespace Mammut.Server.Core.Models
{
	public class WorkingConditionStatement
	{
		public ConditionType LogicalConnector { get; set; } = ConditionType.None;
		public WorkingConditionValue LeftValue { get; set; }
		public ConditionQualifier Qualifier { get; set; }
		public WorkingConditionValue RightValue { get; set; }

		public WorkingConditionStatement()
		{
		}

		public WorkingConditionStatement(ConditionType logicalConnector, WorkingConditionValue leftValue, ConditionQualifier qualifier, WorkingConditionValue rightValue)
		{
			LogicalConnector = logicalConnector;
			LeftValue = leftValue;
			Qualifier = qualifier;
			RightValue = rightValue;
		}
	}
}