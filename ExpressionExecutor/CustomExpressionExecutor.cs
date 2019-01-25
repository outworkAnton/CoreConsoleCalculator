using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionExecutor
{
    public class CustomExpressionExecutor : MathExpressionExecutor
    {
        private readonly Dictionary<char, Func<Expression, Expression, Expression>> _arithmeticOperations =
            new Dictionary<char, Func<Expression, Expression, Expression>>
            {
                {'+', Expression.Add},
                {'-', Expression.Subtract},
                {'*', Expression.Multiply},
                {'/', Expression.Divide}
            };

        protected override string ExecuteImplementation(string expression)
        {
            return CalculateExpression(expression).ToString("0.##");
        }

        private decimal CalculateExpression(string expression)
        {
            foreach (var op in _arithmeticOperations)
            {
                if (!expression.Contains(op.Key)) continue;
                var segments = expression.Split(op.Key);
                Expression operationResult = Expression.Constant(CalculateExpression(segments[0]));

                operationResult = segments
                    .Skip(1)
                    .Aggregate(operationResult,(current, next) => op.Value(current, Expression.Constant(CalculateExpression(next))));

                var compiledExp = Expression.Lambda<Func<decimal>>(operationResult).Compile();
                return compiledExp();
            }

            return decimal.TryParse(expression, out var value)
                ? value
                : throw new ArgumentException("Evaluation failed");
        }
    }
}