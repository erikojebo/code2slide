using System;
using System.IO;
using code2slide.core;

namespace code2slide
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("usage: code2slide <filename>");
            }

            var transformer = new MarkdownSlideTransformer();
            var slideShow = transformer.TransformFile(args[0]);

            slideShow.WriteToDirectory(Directory.GetCurrentDirectory());
        }
    }
}