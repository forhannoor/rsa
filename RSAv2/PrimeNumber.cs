// Handles prime number operations.

using System;
using System.Collections.Generic;

namespace RSAv2
{
    class PrimeNumber
    {
        private int[] _numbers;
        private Random _random;
        // File name to read prime numbers from.
        private const string SOURCE = "resource/prime_numbers.txt";
        private const int SIZE = 200;
        
        public PrimeNumber(string fileName = null)
        {
            _numbers = new int [SIZE];
            _random = new Random();
            FileOperation fo = new FileOperation();
            fo.FileName = fileName ?? SOURCE;
            IEnumerable<string> nums = fo.Read();
            var index = 0;

            // If unable to read prime number file.
            if(nums == null)
            {
                Console.WriteLine("Unable to read prime number file! Terminating program.");
                // Stop program execution.
                Environment.Exit(1);
            }

            else
            {

                // Initialize prime numbers in the list.
                foreach (string x in nums)
                {
                    _numbers[index++] = int.Parse(x);
                }
            }
        }

        // Checks for primality given an integer.
        private bool IsPrime(int a)
        {
            bool r = true;

            // If number is already in list.
            if(a >= _numbers[0] && a <= _numbers[SIZE - 1])
            {
                for(int i = 0; i < SIZE; i++)
                {
                    if(_numbers[i] == a)
                    {
                        break;
                    }
                }
            }

            // Number is not in the list, determine mathematically.
            else
            {
                var limit = (int) Math.Sqrt(a);

                for (int i = 2; r && i <= limit; i++)
                {
                    if (a % i == 0)
                    {
                        r = false;
                    }
                }
            }
            
            return r;
        }

        // Returns a random prime from list.
        public int RandomPrime()
        {
            var index = _random.Next(0, SIZE);
            return _numbers[index];
        }
    }
}