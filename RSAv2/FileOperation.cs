// Handles file operation.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RSAv2
{
    class FileOperation
    {
        private StringBuilder _stringBuilder;

        public FileOperation() { _stringBuilder = new StringBuilder(); }

        public string FileName { get; set; }

        public string Content { get; set; }

        // Reads file content provided its name.
        public IEnumerable<string> Read()
        {
            IEnumerable<string> lines = null;

            try
            {
                lines = File.ReadLines(FileName);

                foreach (string s in lines)
                {
                    _stringBuilder.Append(s);
                    _stringBuilder.AppendLine();
                }

                Content = _stringBuilder.ToString();
                _stringBuilder.Clear();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }

            return lines;
        }
    }
}