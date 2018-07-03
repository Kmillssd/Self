using System;

namespace FeatureToggle
{
    class Program
    {
        public delegate void MultiplierDelegate(ref int value);

        static void Main(string[] args)
        {
            if (ConfigurationManager.EnableMultiplier)
            {
                Calculate((ref int x) => x *= 2);
            }
            else
            {
                Calculate();
            }

            Console.ReadKey();
        }
   
        public static void Calculate(MultiplierDelegate multiplierDelegate = null)
        {
            var value = 4;

            multiplierDelegate?.Invoke(ref value);

            Console.WriteLine($"Calculated value is {value}");
         }
    }
}
