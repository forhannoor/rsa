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

        public FileOperation() 
        { 
            _stringBuilder = new StringBuilder(); 
        }

        public string FileName { get; set; }

        public string Content { get; set; }

        // Reads file and returns its content.
        public IEnumerable<string> Read()
        {
            IEnumerable<string> lines = null;

            try
            {
                lines = File.ReadLines(FileName);

                foreach (var line in lines)
                {
                    _stringBuilder.Append(line);
                    _stringBuilder.AppendLine();
                }

                Content = _stringBuilder.ToString();
                _stringBuilder.Clear();
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                Console.WriteLine(fileNotFoundException.Message);
            }
            catch (ArgumentNullException argumentNullException)
            {
                Console.WriteLine(argumentNullException.Message);
            }

            return lines;
        }
    }
}