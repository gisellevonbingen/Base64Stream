using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Base64Stream.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var original = "Hello, World!\r\n안녕하세요!";
            byte[] encoded = null;

            using (var output = new MemoryStream())
            {
                using (var bs = new Base64Stream(output, true))
                {
                    bs.Write(Encoding.Default.GetBytes(original));
                }

                encoded = output.ToArray();
            }

            string decoded = null;

            using (var input = new MemoryStream(encoded))
            {
                using (var bs = new Base64Stream(input, true))
                {
                    using (var temp = new MemoryStream())
                    {
                        bs.CopyTo(temp);
                        decoded = Encoding.Default.GetString(temp.ToArray());
                    }

                }

            }

            Console.WriteLine();
            Console.WriteLine("===== Original =====");
            Console.WriteLine(original);
            Console.WriteLine();
            Console.WriteLine("===== Encoded =====");
            Console.WriteLine(Encoding.Default.GetString(encoded));
            Console.WriteLine();
            Console.WriteLine("===== Decoded =====");
            Console.WriteLine(decoded);
            Console.WriteLine();
            Console.WriteLine("===== Test Result =====");
            Console.WriteLine(original.Equals(decoded));
        }

    }

}
