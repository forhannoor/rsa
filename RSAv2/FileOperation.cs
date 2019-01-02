/*
 * Handles reading content from designated file if exists
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RSAv2
{
    class FileOperation
    {
        private StringBuilder sb;

        public FileOperation() { sb = new StringBuilder(); }

        public FileOperation(string fName) { FileName = fName; sb = new StringBuilder(); }

        public String FileName { get; set; }

        public string Content{ get; set; }

        public IEnumerable<string> Read()
        {
            IEnumerable<string> lines = null;

            try
            {
                lines = File.ReadLines(FileName);
                
                foreach(string s in lines)
                {
                    sb.Append(s);
                    sb.AppendLine();
                }

                Content = sb.ToString();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return lines;
        }
    }
}