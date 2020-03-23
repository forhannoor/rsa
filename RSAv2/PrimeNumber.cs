using System;
using System.Collections.Generic;

namespace RSAv2
{
    class PrimeNumber
    {
        private int[] _numbers;
        private Random _random;
        // File to read numbers from.
        private const string SOURCE = "resource/prime_numbers.txt";
        private const int SIZE = 200;
        
        // Constructor that reads prime numbers from file and initializes list.
        public PrimeNumber(string fileName = null)
        {
            _numbers = new int [SIZE];
            _random = new Random();
            var fileOperation = new FileOperation();
            fileOperation.FileName = fileName ?? SOURCE;
            IEnumerable<string> numbers = fileOperation.Read();
            int index = 0;

            // If file reading failed.
            if(numbers == null)
            {
                Console.WriteLine("Unable to read prime numbers from file. Terminating program.");
                // Exit program.
                Environment.Exit(1);
            }

            // If file reading succeeded.
            else
            {
                // Initialize list with prime numbers.
                foreach (var number in numbers)
                {
                    _numbers[index++] = int.Parse(number);
                }
            }
        }

        // Tests for primality.
        private bool IsPrime(int number)
        {
            bool isPrime = true;

            // If number is already in list.
            if(number >= _numbers[0] && number <= _numbers[SIZE - 1])
            {
                for(int i = 0; i < SIZE; ++i)
                {
                    if(_numbers[i] == number)
                    {
                        break;
                    }
                }
            }

            // If number is not in list, determine mathematically.
            else
            {
                var limit = (int) Math.Sqrt(number);

                for (int i = 2; isPrime && i <= limit; ++i)
                {
                    if (number % i == 0)
                    {
                        isPrime = false;
                    }
                }
            }
            
            return isPrime;
        }

        // Returns a prime number randomly from list.
        public int RandomPrime()
        {
            int index = _random.Next(0, SIZE);
            return _numbers[index];
        }
    }
}