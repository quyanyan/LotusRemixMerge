using Microsoft.Office.Interop.Word;
using Newtonsoft.Json.Linq;
using Processor;
using SuperHero;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FatBear
{
    class Program
    {
        static void Main(string[] args)
        {
            var outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            if (Directory.Exists(outputFolder))
            {
                // Directory.Delete(outputFolder, true);    
            }

            Directory.CreateDirectory(outputFolder);

            var processor = new Processor.Processor(new JsonOutputFormatter());

            var files = Directory.GetFiles("Resumes").Select(Path.GetFullPath);
            foreach (var file in files)
            {
                var output = processor.Process(file);
                var i = 0;
                foreach (var item in output)
                {
                    if (item == null)
                    {
                        continue;
                    }
                    Console.WriteLine(item);
                    var outputFileName = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                    using (var writer = new StreamWriter(Path.Combine(outputFolder, outputFileName + i + ".txt")))
                    {
                        writer.Write(item);
                    }
                    i++;
                }
            }
        }
    }
}