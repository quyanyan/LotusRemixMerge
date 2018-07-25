﻿using System.Collections.Generic;
using System.IO;
using Model;

namespace Readers.Plain
{
    public class PlainTextInputReader : InputReaderBase
    {
        protected override bool CanHandle(string location)
        {
            return location.EndsWith("txt");
        }

        protected override IList<string> Handle(string location)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(location))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }
            return lines;
        }
    }
}
