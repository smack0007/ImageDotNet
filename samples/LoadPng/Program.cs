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

                Image image = Image.LoadPng(Path.Combine(basePath, fileName));

                Console.WriteLine(image.PixelType.Name);

                foreach (var pixel in image)
                    Console.WriteLine(pixel);
                
                Console.WriteLine();
            }

            Image image2 = Image.LoadPng(Path.Combine(basePath, fileNames[0]));
            image2.SavePng(Path.Combine(basePath, "output.png"));
        }
    }
}
