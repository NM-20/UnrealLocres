using LocresLib;
using System.Collections.Generic;
using System.IO;

namespace UnrealLocres.Converter
{
    public sealed class TxtConverter : BaseConverter
    {
        public override string ExportExtension => "txt";
  
        public override string ImportExtension => "txt";

        protected override List<TranslationEntry> Read(TextReader stream, LocresFile locres)
        {
            var result = new List<TranslationEntry>(locres.TotalCount);

            foreach (LocresNamespace currentNamespace in locres)
            {
                foreach (LocresString currentString in currentNamespace)
                {
                    /* Assuming that the order and amount of strings in the text file is the
                       same, we can use the stream's current line as the translation's value.
                    */
                    string key = string.Format("{0}/{1}",
                        currentNamespace.Name, currentString.Key);

                    result.Add(new TranslationEntry(
                        key, currentString.Value, stream.ReadLine().Replace("\\n", "\n")));
                }
            }

            return result;
        }
  
        protected override void Write(List<TranslationEntry> data, TextWriter writer)
        {
            /* Since we're writing plaintext, exporting is pretty straightforward: write all
               `TranslationEntry` values. 
            */
            foreach (TranslationEntry current in data)
              writer.WriteLine(current.Source.Replace("\n", "\\n"));
        }
    }
}
