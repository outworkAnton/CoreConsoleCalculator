using System;
using Autofac;
using ExpressionExecutor;

namespace CoreConsoleCalculator
{
    class Program
    {
        private static void Main(string[] args)
        {
            var executor = ConfigureDI().Resolve<IMathExpressionExecutor>();

#if DEBUG
            while (true)
            {
#else
            do
            {
                while (!Console.KeyAvailable)
                {
#endif
                Console.WriteLine("Input math expression (Press ESC to exit):");

                string result;

                try
                {
                    result = executor.Execute(Console.ReadLine());
                }
                catch (ArgumentException e)
                {
                    result = e.Message;
                }

                Console.WriteLine($"Result is: {result}");
#if DEBUG
            }
#else
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
#endif
        }

        private static IContainer ConfigureDI()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterType<CustomExpressionExecutor>()
                .As<IMathExpressionExecutor>();

            var container = builder.Build();
            return container;
        }
    }
}