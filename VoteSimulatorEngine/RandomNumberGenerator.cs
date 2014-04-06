using System;

namespace ClassLibrary1
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly Random random;

        public RandomNumberGenerator()
        {
            random = new Random();
        }

        public int GenerateRandomNumber(int maxValue)
        {
            return random.Next(maxValue);
        }
    }
}
