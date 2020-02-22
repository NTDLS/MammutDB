using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Mammut.Server.Core.Constants;

namespace Mammut.Server.Core.Models
{
    public class ConditionExpression
    {
        public ConditionType LogicalConnector { get; set; } = ConditionType.None;
        public List<ConditionStatement> Statements { get; set; } = new List<ConditionStatement>();
        public List<ConditionExpression> Children { get; set; } = new List<ConditionExpression>();

        public void Add(ConditionExpression expression)
        {
            Children.Add(expression);
        }

        public ConditionExpression AddNew()
        {
            return AddNew(ConditionType.None);
        }

        public ConditionExpression AddNew(ConditionType logicalConnector)
        {
            var expression = new ConditionExpression()
            {
                LogicalConnector = logicalConnector
            };
            Children.Add(expression);
            return expression;
        }
    }
}
