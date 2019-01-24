using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ExpressionExecutor
{
    public class MathExpressionExecutor : IMathExpressionExecutor
    {
        private static readonly char[] mathOperators = {'+', '-', '*', '/', '(', ')'};

        public string Execute(string exp)
        {
            exp = ParseExpression(exp);
            return new DataTable().Compute(exp, "").ToString();
        }

        private static string ParseExpression(string exp)
        {
            var expressionElements = new List<string>();
            var element = string.Empty;
            foreach (var c in exp)
            {
                if (char.IsDigit(c))
                {
                    element += c.ToString();
                }
                else if (mathOperators.Contains(c))
                {
                    if (!string.IsNullOrWhiteSpace(element))
                    {
                        expressionElements.Add(element);
                        element = string.Empty;
                    }
                    var lastElement = expressionElements.LastOrDefault();
                    if (lastElement != null && char.IsDigit(lastElement[lastElement.Length - 1]))
                    {
                        expressionElements.Add(c.ToString());
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(element)) continue;
                    expressionElements.Add(element);
                    element = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(element))
            {
                expressionElements.Add(element);
            }

            if (!expressionElements.Any())
            {
                throw new ArgumentException("The input string does not contain the valid expression");
            }

            return string.Join(" ", expressionElements);
        }
    }
}