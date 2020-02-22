using Mammut.Server.Core.Models;
using Mammut.Server.Core.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Mammut.Server.Core.Constants;

namespace Mammut.Server.Core.Engine
{
    public class QueryEngine
    {
        private ServerCore _core;

        public QueryEngine(ServerCore core)
        {
            _core = core;
        }

        public void ExecuteDummy(Session session)
        {
            string sql = Properties.Resources.Dummy_Select_Query;

            ConditionExpression conditions = new ConditionExpression();

            ConditionExpression exp1 = conditions.CreateChild();

            ConditionExpression exp2 = conditions.CreateChild();
            exp2.Statements.Add(new ConditionStatement(Constants.ConditionType.None,
                new ConditionValue(ConditionValueType.AttributeName, "SafetyStockLevel"),
                ConditionQualifier.LessThan,
                new ConditionValue(ConditionValueType.Constant, 10)));

            exp2.Statements.Add(new ConditionStatement(Constants.ConditionType.Or,
                new ConditionValue(ConditionValueType.AttributeName, "SafetyStockLevel"), ConditionQualifier.GreaterThan, new ConditionValue(ConditionValueType.Constant, 500)));
            exp1.Children.Add(exp2);

            exp1.Statements.Add(new ConditionStatement(Constants.ConditionType.And,
                new ConditionValue(ConditionValueType.AttributeName, "SafetyStockLevel"), ConditionQualifier.NotEquals, new ConditionValue(ConditionValueType.Constant, 1000)));

            ConditionExpression exp3 = conditions.CreateChild(ConditionType.And);
            exp3.Statements.Add(new ConditionStatement(Constants.ConditionType.Or,
                new ConditionValue(ConditionValueType.AttributeName, "Color"), ConditionQualifier.Equals, new ConditionValue(ConditionValueType.Constant, "Silver")));
            exp3.Statements.Add(new ConditionStatement(Constants.ConditionType.Or,
                new ConditionValue(ConditionValueType.AttributeName, "Color"), ConditionQualifier.Equals, new ConditionValue(ConditionValueType.Constant, "Black")));

            conditions.Statements.Add(new ConditionStatement(Constants.ConditionType.And,
                new ConditionValue(ConditionValueType.AttributeName, "FinishedGoodsFlag"), ConditionQualifier.Equals, new ConditionValue(ConditionValueType.Constant, "1")));

            var documentIds = _core.Document.GetIdsByCondition(session, "AdventureWorks2008R2:Production:Product", conditions);

           /*
                ( <exp1>
                    ( <exp2>
                        SafetyStockLevel < 10
                        OR SafetyStockLevel > 500
                    )
                    AND SafetyStockLevel <> 1000
                )
                AND ( <exp3>
                    Color = 'Silver'
                    OR Color = 'Black'
                )
                AND FinishedGoodsFlag = 1
             */
        }
    }
}
