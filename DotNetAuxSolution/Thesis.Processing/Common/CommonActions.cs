using System;
using System.IO;
using System.Text;

namespace Thesis.Processing.Common
{
    public static class CommonActions
    {

        public static string GetFileNameByAlgorithmAndNumCluster(string subject, string projectName, string algorithmName, int numClusters)
        {
            return $"{projectName}\\{subject}_{algorithmName}_K_{numClusters}.csv";
        }

        public static string GetFileNameDatePrefix()
        {
            return DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        }


        public static string GetFileNameWithDatePrefix(string fileName)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + fileName;
        }

        public static void WriteLogClustering(string filePath, string ProjectName, string AlgorithmName, int NumCluster, int ClusterNumber, int TotalTestMethod, int TotalIterations, double totalDistance)
        {
            WriteLogLine(filePath, $"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")},{ProjectName},{AlgorithmName},{NumCluster},{ClusterNumber},{TotalTestMethod},{TotalIterations},{totalDistance}");
        }

        public static void WriteLogBestK(string filePath, string ProjectName, string AlgorithmName, int NumCluster, double totalDistance)
        {
            WriteLogLine(filePath, $"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")},{ProjectName},{AlgorithmName},{NumCluster},{totalDistance}");
        }

        public static void WriteLogReduction(string filePath, string ProjectName, string clusteringAlgorithmName, string reductionAlgorithmName, int NumCluster, int QtdTestCaseOriginal,
            int QtdReqCoverOriginal, int QtdTestCaseAfterDistanceFilter, int QtdReqCoverAfterDistanceFilter, int QtdTestCaseFinalReduction, int QtdReqCoverFinalReduction)
        {
            WriteLogLine(filePath, $"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")},{ProjectName},{clusteringAlgorithmName},{reductionAlgorithmName},{NumCluster},{QtdTestCaseOriginal},{QtdReqCoverOriginal},{QtdTestCaseAfterDistanceFilter},{QtdReqCoverAfterDistanceFilter},{QtdTestCaseFinalReduction},{QtdReqCoverFinalReduction}");
        }

        private static void WriteLogLine(string filePath, string lineContent)
        {
            using (FileStream arqLog = File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                byte[] bytes = Encoding.Default.GetBytes(lineContent + "\r\n");
                arqLog.Write(bytes, 0, bytes.Length);
                arqLog.Close();
            }
        }
    }
}
