using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImage.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string[] fileNames = new string[]
            {
                "Image24bitsNoRLE.tga",
                "Image32bitsNoRLE.tga",
            };

            foreach (string fileName in fileNames)
            {
                Console.WriteLine("{0}:", fileName);

                Image image = Image.FromFile(fileName);

                int bpp = image.PixelFormat == PixelFormat.RGBA ? 4 : 3;

                for (int i = 0; i < image.Pixels.Length; i += bpp)
                {
                    for (int j = 0; j < bpp; j++)
                    {
                        Console.Write(image.Pixels[i + j] + " ");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
