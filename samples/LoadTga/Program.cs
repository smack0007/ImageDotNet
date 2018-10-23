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

                var image = Image.LoadTga(Path.Combine(basePath, fileName));

                Console.WriteLine(image.PixelType.Name);

                image = image.To<Rgba32>();

                Console.WriteLine(image.PixelType.Name);

                image.ForEachPixel(x => Console.WriteLine(x));

                Console.WriteLine();
            }

            var image2 = Image.LoadTga(Path.Combine(basePath, fileNames[0]));
            image2.SaveTga(Path.Combine(basePath, "output.tga"));
        }
    }
}
