using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Model;
using Readers;
using Readers.Doc;
using Readers.Docx;
using Readers.Pdf;
using Readers.Plain;
using Readers.Html;
using Readers.Htm;

namespace Processor
{
    internal class InputReaderFactory : IInputReaderFactory
    {
        //private readonly IApplicationSettings _applicationSettings;

        public InputReaderFactory()
        {
        }

        public IInputReader LoadInputReaders()
        {
            //var parserLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, readerLoaction);

            var parsers = new List<IInputReader>();
            parsers.Add(new HtmPageReader());
            parsers.Add(new HtmlPageReader());
            parsers.Add(new InteropDocInputReader());
            parsers.Add(new OpenXmlInputReader());
            parsers.Add(new PdfInputReader());
            parsers.Add(new PlainTextInputReader());
            //foreach (var dll in Directory.GetFiles(parserLocation, "*.dll", SearchOption.AllDirectories))
            //{
            //    try
            //    {
            //        var loadedAssembly = Assembly.LoadFile(dll);
            //        var instances = from t in loadedAssembly.GetTypes()
            //                        where t.GetInterfaces().Contains(typeof(IInputReader))
            //                        select Activator.CreateInstance(t) as IInputReader;
            //        parsers.AddRange(instances);
            //    }
            //    catch (FileLoadException)
            //    {
            //        // The Assembly has already been loaded, ignore  
            //    }
            //    catch (BadImageFormatException)
            //    {
            //        // If a BadImageFormatException exception is thrown, the file is not an assembly, ignore    
            //    } 

            //}

            if (!parsers.Any())
            {
                throw new ApplicationException("No parsers registered");
            }

            for (var i = 0; i < parsers.Count - 1; i++)
            {
                parsers[i].NextReader = parsers[i + 1];
            }

            return parsers[0];
        }
    }
}