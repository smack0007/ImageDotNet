using System;

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

                Console.WriteLine(image.Format.ToString());

                for (int i = 0; i < image.Length; i += image.BytesPerPixel)
                {
                    for (int j = 0; j < image.BytesPerPixel; j++)
                    {
                        Console.Write("{0:000} ", image[i + j]);
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
