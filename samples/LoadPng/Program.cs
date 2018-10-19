using System;
using System.IO;
using ImageDotNet;

namespace LoadPng
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);

            var fileNames = new string[]
            {
                "Image32bits.png",
            };

            foreach (string fileName in fileNames)
            {
                Console.WriteLine("{0}:", fileName);

                var image = Image.LoadPng(Path.Combine(basePath, fileName)).To<Rgba32>();

                Console.WriteLine(image.PixelType.Name);

                image.ForEachPixel(x => Console.WriteLine(x));
                
                Console.WriteLine();
            }

            var image2 = Image.LoadPng(Path.Combine(basePath, fileNames[0]));
            image2.SavePng(Path.Combine(basePath, "output.png"));
        }
    }
}
