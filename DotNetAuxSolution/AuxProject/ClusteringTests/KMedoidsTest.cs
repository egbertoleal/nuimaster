﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thesis.Processing.Clustering;
using Thesis.Processing.Common;
using Thesis.Processing.Data;

namespace AuxProject.ClusteringTests
{
    [TestClass]
    public class KMedoidsTest
    {
        private const string ALGORITHM_NAME = ConstAux.ALGORITHM_NAME_KMEDOIDS;
        private const string ACTION_CLUSTERING_TYPE = ConstAux.ACTION_CLUSTERING_TYPE;
        private string DIR_PATH_OUTPUT = $"{ConstAux.ROOT_DIR_CLUSTERING_FOLDER}\\";
        private const int MAX_NUMBER_ITERATION = 100;

        #region Test Methods

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
        public void RunKMedoidsOnJavaProject(int numClusters)
        {
            RunKMedoidsOOMetricsTest(numClusters, ConstAux.JAVA_TEST_PROJECT_PRE_PROCESSING_FILE_FULL_PATH, ConstAux.PROJECT_NAME_JAVA_TEST);
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
        public void RunKMedoidsOnFirebaseProject(int numClusters)
        {
            RunKMedoidsOOMetricsTest(numClusters, ConstAux.FIREBASE_PRE_PROCESSING_FILE_FULL_PATH, ConstAux.PROJECT_NAME_FIREBASE);
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
        public void RunKMedoidsOnApacheProject(int numClusters)
        {
            RunKMedoidsOOMetricsTest(numClusters, ConstAux.APACHE_PRE_PROCESSING_FILE_FULL_PATH, ConstAux.PROJECT_NAME_APACHE);
        }


        [TestMethod]
        public void RunMedoidsFindBestKForJavaTest()
        {
            RunMedoidsFindBestK(ConstAux.JAVA_TEST_PROJECT_PRE_PROCESSING_FILE_FULL_PATH, ConstAux.PROJECT_NAME_JAVA_TEST);
        }

        [TestMethod]
        public void RunMedoidsFindBestKForFirebaseProject()
        {
            RunMedoidsFindBestK(ConstAux.FIREBASE_PRE_PROCESSING_FILE_FULL_PATH, ConstAux.PROJECT_NAME_FIREBASE);
        }

        [TestMethod]
        public void RunMedoidsFindBestKForApacheProject()
        {
            RunMedoidsFindBestK(ConstAux.APACHE_PRE_PROCESSING_FILE_FULL_PATH, ConstAux.PROJECT_NAME_APACHE);
        }

        #endregion

        #region Internal / Private

        public void RunKMedoidsOOMetricsTest(int numClusters, string inputFilePath, string projectName)
        {
            var inputContent = new FileSheet(inputFilePath);

            double[][] rawData = inputContent.GetDoubleMatrix(ConstAux.METRICS_FILE_INITIAL_INDEX_FEATURES, ConstAux.METRICS_FILE_FINAL_INDEX_FEATURES);

            var clusterAlg = new KMedoids(rawData, numClusters, MAX_NUMBER_ITERATION);
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

        public void RunMedoidsFindBestK(string inputFilePath, string projectName)
        {
            var logFilePath = $"{DIR_PATH_OUTPUT}LogBestK.csv";

            var inputContent = new FileSheet(inputFilePath);

            double[][] rawData = inputContent.GetDoubleMatrix(ConstAux.METRICS_FILE_INITIAL_INDEX_FEATURES, ConstAux.METRICS_FILE_FINAL_INDEX_FEATURES);

            for (int numClusters = 2; numClusters < 10; numClusters++)
            {
                KMedoids clusterAlg = new KMedoids(rawData, numClusters, MAX_NUMBER_ITERATION);
                clusterAlg.Cluster(); 
                
                double sumDistances = clusterAlg.Distances.Sum();

                CommonActions.WriteLogBestK(logFilePath, projectName, ALGORITHM_NAME, numClusters, sumDistances);
            }
        }

        #endregion
    }
}
