using System.Data;

namespace ExpressionExecutor
{
    public class DataTableExpressionExecutor : MathExpressionExecutor
    {
        protected override string ExecuteImplementation(string expression)
        {
            return new DataTable().Compute(expression, "").ToString();
        }
    }
}