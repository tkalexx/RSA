using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace RSA
{
    class RSA
    {
        char[] alfavit = new char[] { '#', 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И',
                                                'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С',
                                                'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ',
                                                'Э', 'Ю', 'Я', ' ', '1', '2', '3', '4', '5', '6', '7',
                                                '8', '9', '0' };
        private bool IsTheNumberSimple(long n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (long i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }
        private long count_d(long m)
        {
            long d = m - 1;

            for (long i = 2; i <= m; i++)
                if ((m % i == 0) && (d % i == 0))
                {
                    d--;
                    i = 1;
                }

            return d;
        }
        private long count_e(long d, long m)
        {
            long e = 10;
            while (true)
            {
                if ((e * d) % m == 1)
                    break;
                else
                    e++;
            }

            return e;
        }
        public List<string> Encrypt1(string text1, long e, long n)
        {
            List<string> result = new List<string>();
            BigInteger bi;
            for (int i = 0; i < text1.Length; i++)
            {
                int index = Array.IndexOf(alfavit, text1[i]);
                bi = new BigInteger(index);
                bi = BigInteger.Pow(bi, (int)e);
                BigInteger n_ = new BigInteger((int)n);
                bi = bi % n_;
                result.Add(bi.ToString());
            }
            return result;
        }
        private string Dedoce(List<string> input, long d, long n)
        {
            string result = "";
            BigInteger bi;
            foreach (string item in input)
            {
                bi = new BigInteger(Convert.ToDouble(item));
                bi = BigInteger.Pow(bi, (int)d);
                BigInteger n_ = new BigInteger((int)n);
                bi = bi % n_;
                int index = Convert.ToInt32(bi.ToString());
                result += alfavit[index].ToString();
            }

            return result;
        }
        static void Main()
        {
            var cipher = new RSA();
            Console.Write("Введите число: ");
            int p1 = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите число: ");
            long d = 0;
            long n = 0;
            int q1 = Convert.ToInt32(Console.ReadLine());
            if ((p1 > 0) && (q1 > 0))
            {
                long p = Convert.ToInt64(p1);
                long q = Convert.ToInt64(q1);
                if (cipher.IsTheNumberSimple(p) && cipher.IsTheNumberSimple(q))
                {
                    string text1 = "";
                    StreamReader sr = new StreamReader("in.txt");
                    while (!sr.EndOfStream)
                    {
                        text1 += sr.ReadLine();
                    }
                    sr.Close();
                    text1 = text1.ToUpper();
                    n = p * q;
                    long m = (p - 1) * (q - 1);
                    d = 27;
                    long e_ = 3;
                    List<string> result = cipher.Encrypt1(text1, e_, n);
                    StreamWriter f = new StreamWriter("out1.txt");
                    foreach (string i in result)
                        f.WriteLine(i);
                    f.Close();
                }
                else
                    Console.WriteLine("p или q - не простые числа!");
            }
            else
            {
                Console.WriteLine("Введите p и q!");
            }
            if ((d > 0) && (n > 0))
            {
                long d1 = Convert.ToInt64(d);
                long n1 = Convert.ToInt64(n);
                List<string> input = new List<string>();
                StreamReader sr = new StreamReader("out1.txt");
                while (!sr.EndOfStream)
                {
                    input.Add(sr.ReadLine());
                }
                sr.Close();
                string result = cipher.Dedoce(input, d1, n1);
                StreamWriter sw = new StreamWriter("out2.txt");
                sw.WriteLine(result);
                sw.Close();
            }
            else
            {
                Console.WriteLine("Введите секретный ключ!");
            }
            Console.ReadLine();
        }
    }
}