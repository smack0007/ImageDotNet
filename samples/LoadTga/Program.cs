using System;
using System.IO;
using ImageDotNet.Tga;

namespace ImageDotNet.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);

            var fileNames = new string[]
            {
                "Image24bitsNoRLE.tga",
                "Image32bitsNoRLE.tga",
                "Image24bitsRLE.tga",
                "Image32bitsRLE.tga",
            };

            foreach (string fileName in fileNames)
            {
                Console.WriteLine("{0}:", fileName);

                Image image = Image.LoadTga(Path.Combine(basePath, fileName));

                Console.WriteLine(image.GetType().Name);

                //for (int i = 0; i < image.Length; i += image.BytesPerPixel)
                //{
                //    for (int j = 0; j < image.BytesPerPixel; j++)
                //    {
                //        Console.Write("{0:000} ", image[i + j]);
                //    }

                //    Console.WriteLine();
                //}

                Console.WriteLine();
            }

            Image image2 = Image.LoadTga(Path.Combine(basePath, fileNames[0]));
            image2.SaveTga(Path.Combine(basePath, "output.tga"));
        }
    }
}
