using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AuxProject.AuxPreProcessing
{
    [TestClass]
    public class AddCoverCodeJava
    {
        private string SRC_JAVA_DIR = @"D:\Github\nui\wseclipse\apache-commons-math\src\main\java";
        private string CODIGO_LOG = "LogCode.LogCoverageCode.PrintCode();";

        [TestMethod]
        public void AddLogCodeJava()
        {
            string[] allfiles = Directory.GetFiles(SRC_JAVA_DIR, "*.java", SearchOption.AllDirectories);
            foreach (var file in allfiles)
            {
                Console.Out.WriteLine(file);
                if (!file.Contains("LogCoverageCode.java"))
                {
                    ReplaceFile(file);
                }

            }
        }

        public void ReplaceFile(string inputPath)
        {
            string className = inputPath.Replace($"{SRC_JAVA_DIR}\\", "");
            className = className.Replace(@"\", ".");
            className = className.Replace(@".java", "");

            string tempOutputFile = inputPath + ".temp";
            Console.WriteLine("Temp File: " + tempOutputFile);
            Console.WriteLine("Processing File: " + inputPath);

            if (File.Exists(tempOutputFile))
            {
                File.Delete(tempOutputFile);
            }

            int countSymbol = 0;
            bool insideClass = false;

            using (var input = File.OpenText(inputPath))
            using (var output = new StreamWriter(tempOutputFile))
            {
                string line;
                while (null != (line = input.ReadLine()))
                {
                    if (line.Trim().Contains("class ") && !line.Trim().StartsWith("*"))
                    {
                        insideClass = true;
                    }

                    if (insideClass && line.Contains("{"))
                    {
                        countSymbol++;
                    }

                    if (countSymbol >= 2 && line.Contains(";")
                    && line.Contains(";")
                    && !line.Trim().Equals("};")
                    && !line.Equals(" for (")
                    && !line.Equals(" for(")
                    //&& !line.Contains(" final ")
                    && !line.Contains(" static ")
                    && !line.Contains(" public ")
                    && !line.Contains(" private ")
                    && !line.Contains(" protected ")
                    && !line.Contains(" default ")
                    && !line.Contains(" abstract ")
                    && !line.Contains(" interface ")
                    && !line.Trim().StartsWith("for (")
                    && !line.Trim().StartsWith("for(")
                    && !line.Trim().Contains("private ")
                    && !line.Trim().Contains("public ")
                    && !line.Trim().Contains("protected ")
                    && !line.Trim().Contains("default ")
                    && !line.Trim().Contains("abstract ")
                    && !line.Trim().Contains("interface "))
                    {

                        if (line.Contains("  throw ") ||
                            line.Contains(" return ") ||
                            line.Contains("return;") ||
                            line.Contains(" break ") ||
                            line.Contains(" break;") ||
                            line.Contains(" continue;") ||
                            line.Contains(" continue "))
                        {
                            int indexFirstCharacter = GetIndexFirstCharacterNonWhitespace(line);
                            line = line.Insert(indexFirstCharacter, CODIGO_LOG);
                        }
                        else
                        {
                            line = line.Replace(";", ";" + CODIGO_LOG);
                        }

                        //if (line.Contains(" super(") ||
                        //    line.Contains(" super (") ||
                        //    line.Contains(" this(") ||
                        //    line.Contains(" this ("))
                        //{
                        //    line = line.Replace(";", ";" + CODIGO_LOG);
                        //}
                        //else
                        //{

                        //    int indexFirstCharacter = GetIndexFirstCharacterNonWhitespace(line);
                        //    line = line.Insert(indexFirstCharacter, CODIGO_LOG);
                        //}
                    }

                    output.WriteLine(line);
                }
            }

            Console.WriteLine("Replacing '" + inputPath + "' with '" + tempOutputFile + "'");
            File.Delete(inputPath);
            File.Move(tempOutputFile, inputPath);
        }

        private int GetIndexFirstCharacterNonWhitespace(string text)
        {
            return text.TakeWhile(Char.IsWhiteSpace).Count();
        }
    }
}