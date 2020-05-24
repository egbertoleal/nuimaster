using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thesis.Processing.Clustering;
using Thesis.Processing.Common;
using Thesis.Processing.Data;

namespace AuxProject.ClusteringTests
{
    [TestClass]
    public class KMeansTest
    {
        private const string ALGORITHM_NAME = ConstAux.ALGORITHM_NAME_KMEANS;
        private const string ACTION_CLUSTERING_TYPE = ConstAux.ACTION_CLUSTERING_TYPE;
        private string DIR_PATH_OUTPUT = $"{ConstAux.ROOT_DIR_CLUSTERING_FOLDER}\\";
        private const int MAX_NUMBER_ITERATION = 100;

        #region Test methods

        [TestMethod]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        [DataRow(9)]
        [DataRow(10)]
        [DataRow(11)]
        [DataRow(12)]
        [DataRow(13)]
        [DataRow(14)]
        [DataRow(15)]
        public void RunKMeansOnJavaTestProject(int numClusters)
        {
            RunKMeansOOMetricsTest(numClusters, ConstAux.JAVA_TEST_PROJECT_PRE_PROCESSING_FILE_FULL_PATH, ConstAux.PROJECT_NAME_JAVA_TEST);
        }


        [TestMethod]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        [DataRow(9)]
        [DataRow(10)]
        [DataRow(11)]
        [DataRow(12)]
        [DataRow(13)]
        [DataRow(14)]
        [DataRow(15)]
        public void RunKMeansOnFirebaseProject(int numClusters)
        {
            RunKMeansOOMetricsTest(numClusters, ConstAux.FIREBASE_PRE_PROCESSING_FILE_FULL_PATH, ConstAux.PROJECT_NAME_FIREBASE);
        }

        [TestMethod]
        public void RunMeansFindBestKForJavaTest()
        {
            RunMeansFindBestK(ConstAux.JAVA_TEST_PROJECT_PRE_PROCESSING_FILE_FULL_PATH, ConstAux.PROJECT_NAME_JAVA_TEST);
        }

        [TestMethod]
        public void RunMeansFindBestKForFirebaseProject()
        {
            RunMeansFindBestK(ConstAux.FIREBASE_PRE_PROCESSING_FILE_FULL_PATH, ConstAux.PROJECT_NAME_FIREBASE);
        }

        #endregion

        #region Internal / Private

        private void RunKMeansOOMetricsTest(int numClusters, string inputFilePath, string projectName)
        {
            var inputContent = new FileSheet(inputFilePath);

            double[][] rawData = inputContent.GetDoubleMatrix(ConstAux.METRICS_FILE_INITIAL_INDEX_FEATURES, ConstAux.METRICS_FILE_FINAL_INDEX_FEATURES);

            var clusterAlg = new KMeans(rawData, numClusters, MAX_NUMBER_ITERATION);
            int iterations = clusterAlg.Cluster();
            inputContent.AddColumnValues(clusterAlg.Clustering, "cluster");
            inputContent.AddColumnValues(clusterAlg.Distances, "distance");

            var outputFilePath = $"{DIR_PATH_OUTPUT}{CommonActions.GetFileNameByAlgorithmAndNumCluster(ACTION_CLUSTERING_TYPE, projectName, ALGORITHM_NAME, numClusters)}";
            inputContent.SaveToFile(outputFilePath);

            var logFilePath = $"{DIR_PATH_OUTPUT}LogClustering.csv";

            var result = clusterAlg.Clustering.GroupBy(s => s).Select(g => new { Cluster = g.Key, Count = g.Count() });
            foreach (var e in result)
            {
                CommonActions.WriteLogClustering(logFilePath, projectName, ALGORITHM_NAME, numClusters, e.Cluster, e.Count, iterations, clusterAlg.Distances.Sum());
            }
        }

        private void RunMeansFindBestK(string inputFilePath, string projectName)
        {
            var logFilePath = $"{DIR_PATH_OUTPUT}LogBestK.csv";

            var inputContent = new FileSheet(inputFilePath);

            double[][] rawData = inputContent.GetDoubleMatrix(ConstAux.METRICS_FILE_INITIAL_INDEX_FEATURES, ConstAux.METRICS_FILE_FINAL_INDEX_FEATURES);

            for (int numClusters = 1; numClusters < 30; numClusters++)
            {
                KMeans clusterAlg = new KMeans(rawData, numClusters, MAX_NUMBER_ITERATION);
                clusterAlg.Cluster();

                double sumDistances = clusterAlg.Distances.Sum();

                CommonActions.WriteLogBestK(logFilePath, projectName, ALGORITHM_NAME, numClusters, sumDistances);
            }
        }

        #endregion

    }
}
