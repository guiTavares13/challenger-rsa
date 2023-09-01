using System;
using System.Numerics;
using System.Security.Cryptography;

namespace challenger_rsa
{
    class Program
    {
        
        static void Main(string[] args)
        {
            bool flag = true;
            int bitLength = 4096;

            do
            {
                BigInteger random = GenerateRandomNumber(bitLength);

                if (IsProbablyPrime(random, 10))
                {
                    Console.WriteLine("É provavelmente primo.");
                    flag = false;
                }
                else
                {
                    Console.WriteLine("Não é primo.");
                }

                Console.WriteLine(random.ToString());

            } while (flag);

            
        }

        public static BigInteger GenerateRandomNumber(int bitLength)
        {
            byte[] randomBytes = new byte[bitLength / 8];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            randomBytes[randomBytes.Length - 1] &= 0x7F;

            return new BigInteger(randomBytes);
        }

        public static bool IsProbablyPrime(BigInteger n, int k)
        {
            if (n <= 1)
                return false;
            if (n <= 3)
                return true;

            BigInteger d = n - 1;
            int r = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                r++;
            }

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                for (int i = 0; i < k; i++)
                {
                    if (!MillerRabinTest(n, d, r, rng))
                        return false;
                }
            }

            return true;
        }

        public static BigInteger MillerRabinRandom(BigInteger n, RandomNumberGenerator rng)
        {
            byte[] bytes = n.ToByteArray();
            BigInteger result;

            do
            {
                rng.GetBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F;
                result = new BigInteger(bytes);
            } while (result < 2 || result >= n - 1);

            return result;
        }

        public static bool MillerRabinTest(BigInteger n, BigInteger d, int r, RandomNumberGenerator rng)
        {
            BigInteger a = MillerRabinRandom(n, rng);
            BigInteger x = BigInteger.ModPow(a, d, n);

            if (x == 1 || x == n - 1)
                return true;

            for (int i = 1; i < r; i++)
            {
                x = BigInteger.ModPow(x, 2, n);
                if (x == 1)
                    return false;
                if (x == n - 1)
                    return true;
            }

            return false;
        }
    }
}
