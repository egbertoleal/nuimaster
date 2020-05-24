using System;
using System.Collections.Generic;
using System.Linq;
using Thesis.Processing.Common;
using Thesis.Processing.Data;

namespace Thesis.Processing.TestCaseReduction
{
    public static class DefaultReduction
    {
        private static string DIR_PATH_INPUT = $"{ConstAux.ROOT_DIR_CLUSTERING_FOLDER}\\";
        private static string DIR_PATH_OUTPUT = $"{ConstAux.ROOT_DIR_TEST_REDUCTION_FOLDER}\\";
        private static string ACTION_OUTPUT_TYPE = ConstAux.ACTION_REDUCTION_TYPE;
        private const string REDUCTION_ALGORITHM = ConstAux.REDUCTION_ALGORITHM_PAPER;

        public static void RunReduction(string projectName, string clusteringAlgorithmName, int numClusters)
        {
            string inputFilePath = $"{DIR_PATH_INPUT}{CommonActions.GetFileNameByAlgorithmAndNumCluster(ConstAux.ACTION_CLUSTERING_TYPE, projectName, clusteringAlgorithmName, numClusters)}";
            var inputContent = new FileSheet(inputFilePath);

            var fileContent = inputContent.GetAllFileContents();

            // Group by Cluster
            var groupedByCluster = fileContent.GroupBy(x => x.ClusterNumber)
                 .Select(s => new { Cluster = s.Key, Count = s.Count(), Items = s })
                 .ToList();

            var testCasesAfterRemovedWithSameDistance = new List<FileContent>();
            var reducedTestCaseReduction = new List<FileContent>();

            foreach (var clusterGroup in groupedByCluster)
            {
                Console.WriteLine($"Cluster '{clusterGroup.Cluster}'");
                Console.WriteLine($"Original Count '{clusterGroup.Count}'");
                // Remove sample with same distance
                var groupedByDistance = clusterGroup.Items.GroupBy(x => x.DistanceToCentroid)
                 .Select(s => new { Distance = s.Key, Count = s.Count(), HighCover = s.OrderByDescending(x => x.QtdReqCover).ToList().First() })
                 .ToList();
                var clusterSubSet = groupedByDistance.Select(x => x.HighCover).ToList();
                testCasesAfterRemovedWithSameDistance.AddRange(clusterSubSet);
                Console.WriteLine($"After Distance Reduced '{clusterSubSet.Count}'");

                var greedyReduction = new GreedyReduction(clusterSubSet);
                var reducedClusterGreedy = greedyReduction.GetReducededCluster();
                reducedTestCaseReduction.AddRange(reducedClusterGreedy);
                Console.WriteLine($"Final {reducedClusterGreedy.Count}");
            }

            var testCasesIncludedList = Enumerable.Repeat(false, inputContent.TotalRowsWithContent).ToArray();

            foreach (var testCase in reducedTestCaseReduction)
            {
                testCasesIncludedList[testCase.IndexFile] = true;
            }

            inputContent.AddColumnValues(testCasesIncludedList, "TestIncluded");

            var outputFilePath = $"{DIR_PATH_OUTPUT}{CommonActions.GetFileNameByAlgorithmAndNumCluster(ACTION_OUTPUT_TYPE, projectName, REDUCTION_ALGORITHM + clusteringAlgorithmName, numClusters)}";
            inputContent.SaveToFile(outputFilePath);

            var finalTestSuite = inputContent.GetAllFileContents();

            var originalReqCover = finalTestSuite.SelectMany(x => x.ListReqCover.ToList()).ToList().Distinct().ToList();
            var reducedReqCover = finalTestSuite.Where(x => x.TestCasesIncluded).SelectMany(x => x.ListReqCover.ToList()).ToList().Distinct().ToList();
            var afterRemoveWithSameDistanceReqCover = finalTestSuite
                .Where(x => testCasesAfterRemovedWithSameDistance.Exists(y => y.TestClassName == x.TestClassName && y.TestMethodName == x.TestMethodName))
                .SelectMany(z => z.ListReqCover.ToList()).ToList().Distinct().ToList();

            var logFilePath = $"{DIR_PATH_OUTPUT}LogClustering.csv";
            CommonActions.WriteLogReduction(logFilePath, projectName, clusteringAlgorithmName, REDUCTION_ALGORITHM, numClusters,
                finalTestSuite.Count, originalReqCover.Count,
                testCasesAfterRemovedWithSameDistance.Count,
                afterRemoveWithSameDistanceReqCover.Count,
                finalTestSuite.Where(x => x.TestCasesIncluded).ToList().Count, reducedReqCover.Count);            
        }
    }
}
