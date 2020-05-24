using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thesis.Processing.Common;
using Thesis.Processing.Data;

namespace AuxProject.AuxPreProcessing
{
    [TestClass]
    public class MergeReqOOMetrics
    {

        [TestMethod]
        public void GeneratePreProcessingFileJavaAlgorithms()
        {
            GeneratePreProcessingFile(ConstAux.JAVA_TEST_PROJECT_PRE_PROCESSING_FILE_FULL_PATH, ConstAux.REQ_COVER_FILE_FULL_PATH);
        }

        [TestMethod]
        public void GeneratePreProcessingFileFirebase()
        {
            GeneratePreProcessingFile(ConstAux.FIREBASE_PRE_PROCESSING_FILE_FULL_PATH, ConstAux.FIREBASE_REQ_COVER_FILE_FULL_PATH);
        }

        public void GeneratePreProcessingFile(string metricsFileFullPath, string coverFileFullPath)
        {
            var metricFile = new FileSheet(metricsFileFullPath);
            var coverFile = new FileSheet(coverFileFullPath);
            var strCoverFileMatrix = coverFile.GetStringMatrix();
            coverFile.Dispose();
            coverFile = null;

            var countReqCoverPerMethod = new string[metricFile.TotalRowsWithContent];
            var listReqCoverPerMethod = new string[metricFile.TotalRowsWithContent];
            string[] filteredCoverMethod;

            for (int indexRow = 0; indexRow < metricFile.TotalRowsWithContent; indexRow++)
            {   
                filteredCoverMethod = GetReqCoverByMethod(strCoverFileMatrix, 
                    metricFile.GetColumnStringValue(indexRow, ConstAux.METRICS_FILE_TEST_CLASS_NAME_INDEX),
                    metricFile.GetColumnStringValue(indexRow, ConstAux.METRICS_FILE_TEST_METHOD_NAME_INDEX));

                if(filteredCoverMethod.Length == 0)
                {
                    Console.Out.WriteLine($"{metricFile.GetColumnStringValue(indexRow, ConstAux.METRICS_FILE_TEST_CLASS_NAME_INDEX)}.{metricFile.GetColumnStringValue(indexRow, ConstAux.METRICS_FILE_TEST_METHOD_NAME_INDEX)}");
                }
                countReqCoverPerMethod[indexRow] = filteredCoverMethod.Length.ToString();
                listReqCoverPerMethod[indexRow] = String.Join("-", filteredCoverMethod);
            }

            metricFile.AddColumnValues(countReqCoverPerMethod, "QtdReqCover");
            metricFile.AddColumnValues(listReqCoverPerMethod, "ListReqCover");

            metricFile.SaveToFile(metricsFileFullPath);
        }

        private string[] GetReqCoverByMethod(string[][] sourceMatrix,  string className, string methodName)
        {   
            var filteredMatrix = sourceMatrix.Where(x => 
            x[ConstAux.REQ_COVER_CLASS_NAME_INDEX].ToUpper() == className.ToUpper() &&
            x[ConstAux.REQ_COVER_METHOD_NAME_INDEX].ToUpper() == methodName.ToUpper()).ToArray();
            var coverList = new string[filteredMatrix.Length];
            for (int i = 0; i < filteredMatrix.Length; i++)
            {
                coverList[i] = filteredMatrix[i][ConstAux.REQ_COVER_TEST_CASE_INDEX];
            }

            return coverList.Distinct().ToArray();
        }

    }
}
