﻿using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Model;

namespace Readers.Docx
{
    public class OpenXmlInputReader : InputReaderBase
    {
        protected override bool CanHandle(string location)
        {
            return location.EndsWith("docx");
        }

        protected override IList<string> Handle(string location)
        {
            var lines = new List<string>();

            using (var wordDoc = WordprocessingDocument.Open(location, false))
            {
                var paragraphElements = wordDoc.MainDocumentPart.Document.Body.Descendants<Paragraph>();

                lines.AddRange(paragraphElements.Select(p => 
                    p.Descendants<Text>())
                        .Select(textElements => string.Join("", textElements.Select(t => t.Text))));
            }

            return lines;
        }
    }
}
