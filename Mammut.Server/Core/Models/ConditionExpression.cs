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
        public ConditionResult Result { get; set; } = ConditionResult.Undefined;

        public void Add(ConditionExpression expression)
        {
            Children.Add(expression);
        }

        public ConditionExpression CreateChild()
        {
            return CreateChild(ConditionType.None);
        }

        public ConditionExpression CreateChild(ConditionType logicalConnector)
        {
            var expression = new ConditionExpression()
            {
                LogicalConnector = logicalConnector
            };
            Children.Add(expression);
            return expression;
        }

        public WorkingConditionExpression ToWorkingConditionExpression()
        {
            var working = new WorkingConditionExpression();

            working.LogicalConnector = this.LogicalConnector;

            foreach (var statement in this.Statements)
            {
                working.Statements.Add(statement.ToWorkingConditionStatement());
            }

            foreach (var child in this.Children)
            {
                var newChild = working.CreateChild();
                child.LogicalConnector = child.LogicalConnector;
                newChild.Add(child.ToWorkingConditionExpression());
            }

            return working;
        }

        private WorkingConditionExpression ToWorkingConditionExpression(ConditionExpression parent)
        {
            var working = new WorkingConditionExpression();

            working.LogicalConnector = this.LogicalConnector;

            foreach (var statement in this.Statements)
            {
                working.Statements.Add(statement.ToWorkingConditionStatement());
            }

            foreach (var child in this.Children)
            {
                var newChild = working.CreateChild();
                newChild.Add(child.ToWorkingConditionExpression());
            }

            return working;
        }
    }
}
