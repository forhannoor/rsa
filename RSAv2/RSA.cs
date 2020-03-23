// Implements RSA algorithm.

using System;
using System.Numerics;
using System.Text;

namespace RSAv2
{
    class RSA
    {
        private int[] _cipheredText;
        private int _p, _q, _n, _phi, _d, _e;
        private PrimeNumber _primeNumber;

        public RSA()
        {
            _primeNumber = new PrimeNumber();
        }

        // Selects two prime numbers and calculates associated paramters.
        private void SetParameters()
        {
            _p = _primeNumber.RandomPrime();
            _q = _primeNumber.RandomPrime();

            if (_p > _q)
            {
                var t = _q;
                _q = _p;
                _p = t;
            }

            _n = _p * _q;
            _phi = (_p - 1) * (_q - 1);
        }

        // Determines if two numbers are co-primes (GCD/HCF is 1).
        private bool IsCoPrime(int a, int b)
        {
            return (GCD(a, b) == 1);
        }

        // Calculates GCD/HCF between two integers.
        private int GCD(int a, int b)
        {
            if (a == 0)
            {
                return b;
            }

            return GCD(b % a, a);
        }

        // Determines the value of _e.
        private int E()
        {
            for (int i = 2; i < _phi; ++i)
            {
                if (IsCoPrime(i, _phi))
                {
                    return i;
                }
            }

            return -1;
        }

        // Determines the value of _d.
        private int D(int phi, int e)
        {
            int r = 1;

            while ((e * r - 1) % phi != 0)
            {
                ++r;
            }

            return r;
        }

        // Converts a string into a byte array.
        private byte[] StringToBytes(string input)
        {
            int length = input.Length;
            byte[] data = new byte[length];
            char currentChar;
            byte currentByte;

            for (int i = 0; i < length; ++i)
            {
                currentChar = input[i];
                currentByte = (byte)currentChar;

                // If char is a numeric digit (i.e. '1', '2', '0', etc.).
                if (Char.IsDigit(currentChar))
                {
                    currentByte = (byte)(currentByte - 48);
                }

                // If char is a blank space (' ').
                else if (currentChar == ' ')
                {
                    currentByte = (byte)199;
                }

                // If char is a new line ('\n').
                else if (currentChar == '\n')
                {
                    currentByte = (byte)200;
                }

                // If char is a carriage return ('\r').
                else if (currentByte == 13)
                {
                    currentByte = (byte)201;
                }

                else
                {
                    currentByte = (byte)(currentByte - 55);
                }

                data[i] = currentByte;
            }

            return data;
        }

        // Converts a byte array into a string.
        private string BytesToString(byte[] input)
        {
            int length = input.Length;
            var stringBuilder = new StringBuilder();
            byte currentByte;

            for (int i = 0; i < length; ++i)
            {
                currentByte = input[i];

                // If current byte represents a numeric digit.
                if (currentByte >= 0 && currentByte <= 9)
                {
                    currentByte = (byte)(currentByte + 48);
                }

                // If current byte represents a white space.
                else if (currentByte == 199)
                {
                    currentByte = 32;
                }

                // If current byte represents a new line.
                else if (currentByte == 200)
                {
                    currentByte = 10;
                }

                // If current byte represents a carriage return.
                else if (currentByte == 201)
                {
                    currentByte = 13;
                }

                else
                {
                    currentByte = (byte)(currentByte + 55);
                }

                stringBuilder.Append((char)currentByte);
            }

            return stringBuilder.ToString();
        }

        // Converts an int array into a string.
        private string IntsToString(int[] numbers)
        {
            int length = numbers.Length;
            var bytes = new byte[length];

            for (int i = 0; i < length; ++i)
            {
                bytes[i] = (byte)numbers[i];
            }

            return Encoding.UTF8.GetString(bytes);
        }

        // Translates plain text into ciphered text.
        private int[] CipheredText(byte[] data, int e, int n)
        {
            int length = data.Length, temp;
            var result = new int[length];

            for (int i = 0; i < length; ++i)
            {
                temp = data[i];

                if (temp >= 199 && temp <= 201)
                {
                    result[i] = temp;
                }

                else
                {
                    temp = (int)Math.Pow(temp, e);
                    temp %= n;
                    result[i] = temp;
                }
            }

            return result;
        }

        // Translates ciphered text into plain text.
        private byte[] DecipheredText(int[] data, int d, int n)
        {
            int length = data.Length, temp;
            var bytes = new byte[length];
            BigInteger bigInt;

            for (int i = 0; i < data.Length; ++i)
            {
                temp = data[i];

                // If temp represents whitespace, newline or carriage return.
                if (temp >= 199 && temp <= 201)
                {
                    bytes[i] = (byte)temp;
                }

                else
                {
                    bigInt = temp;
                    bigInt = BigInteger.ModPow(bigInt, d, n);
                    temp = Int32.Parse(bigInt.ToString());
                    bytes[i] = (byte)temp;
                }
            }

            return bytes;
        }

        // Returns string version of encrypted message.
        public string Encrypt(string message)
        {
            // Initialize parameters required for encryption.
            SetParameters();
            // Translate string into a byte array.
            byte[] bytes = StringToBytes(message);
            // Determine the value of _e and _d.
            _e = E();
            _d = D(_phi, _e);
            // Translate plain byte array into ciphered int array.
            _cipheredText = CipheredText(bytes, _e, _n);
            var stringBuilder = new StringBuilder();
            int length = _cipheredText.Length;

            for (int j = 0; j < length; ++j)
            {
                stringBuilder.Append(_cipheredText[j]);
            }

            return stringBuilder.ToString();
        }

        // Returns string version of decrypted message.
        public string Decrypt(string message)
        {
            // Translates ciphered int array into plain byte array.
            byte[] plainText = DecipheredText(_cipheredText, _d, _n);
            return BytesToString(plainText);
        }

        public int GetN
        { get { return _n; } set { _n = value; } }

        public int GetD
        { get { return _d; } set { _d = value; } }

        public int GetE
        { get { return _e; } set { _e = value; } }
    }
}