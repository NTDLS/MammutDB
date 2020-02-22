using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Mammut.Server.Core.Constants;

namespace Mammut.Server.Core.Models
{
    public class WorkingConditionExpression
    {
        public ConditionType LogicalConnector { get; set; } = ConditionType.None;
        public List<WorkingConditionStatement> Statements { get; set; } = new List<WorkingConditionStatement>();
        public List<WorkingConditionExpression> Children { get; set; } = new List<WorkingConditionExpression>();

        public void Add(WorkingConditionExpression expression)
        {
            Children.Add(expression);
        }

        public WorkingConditionExpression CreateChild()
        {
            return CreateChild(ConditionType.None);
        }

        public WorkingConditionExpression CreateChild(ConditionType logicalConnector)
        {
            var expression = new WorkingConditionExpression()
            {
                LogicalConnector = logicalConnector
            };
            Children.Add(expression);
            return expression;
        }
    }
}
