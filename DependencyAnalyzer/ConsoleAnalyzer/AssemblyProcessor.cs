using System;
using System.IO;

namespace ConsoleAnalyzer
{
    public class AssemblyProcessor
    {
        public AssemblyProcessor(string assemblyPath)
        {
            AssemblyPath = assemblyPath;
        }

        private string _assemblyPath;

        public string AssemblyPath
        {
            get { return _assemblyPath; }
            set
            {
                if (File.Exists(value))
                {
                    _assemblyPath = value;
                }
                else
                {
                    throw new FileNotFoundException("Cannot find assembly.", value);
                }
                
            }
        }

        public void Process()
        {
            Console.WriteLine("Parsing assembly '{0}'", AssemblyPath);
        }


    }
}