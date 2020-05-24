using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AuxProject.AuxPreProcessing
{
    [TestClass]
    public class FeatureJavaCoverParse
    {
        private string SRC_JAVA_DIR = @"D:\Github\nui\wseclipse\apache-commons-math\src\main\java";
        private string REQ_FILE = @"D:\FilesThesis\1- ReqCover\apache\01-ReqFile.csv";
        private int ReqCounter = 0;

        IList<string> ListReq = new List<string>();

        [TestMethod]
        public void ParseJavaWithReqCover()
        {
            ReqCounter = 0;
            ListReq.Add("ClassName,TestCase");

            string[] allfiles = Directory.GetFiles(SRC_JAVA_DIR, "*.java", SearchOption.AllDirectories);
            foreach (var file in allfiles)
            {
                Console.Out.WriteLine(file);
                ReplaceFile(file);
            }

            File.WriteAllLines(REQ_FILE, ListReq);
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

            using (var input = File.OpenText(inputPath))
            using (var output = new StreamWriter(tempOutputFile))
            {
                string line;
                while (null != (line = input.ReadLine()))
                {
                    if (line.Contains("LogCode.LogCoverageCode.PrintCode();"))
                    {
                        ReqCounter++;
                        ListReq.Add($"{className},c{ReqCounter}");

                        line = line.Replace("LogCode.LogCoverageCode.PrintCode();", $"LogCode.LogCoverageCode.PrintCode({ReqCounter});");

                    }
                    // optionally modify line.
                    output.WriteLine(line);
                }
            }

            Console.WriteLine("Replacing '" + inputPath + "' with '" + tempOutputFile + "'");
            File.Delete(inputPath);
            File.Move(tempOutputFile, inputPath);
        }
    }
}
