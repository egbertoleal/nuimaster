using System;
using System.Linq;
using Accord.MachineLearning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thesis.Processing.Common;
using Thesis.Processing.Data;

namespace AuxProject.ClusteringTests
{
    [TestClass]
    public class KMedoidsMLReady
    {
        //private const string ALGORITHM_NAME = ConstAux.ALGORITHM_NAME_KMEDOIDS_ML;
        //private const string ACTION_CLUSTERING_TYPE = ConstAux.ACTION_CLUSTERING_TYPE;
        //private string DIR_PATH_OUTPUT = $"{ConstAux.ROOT_DIR_CLUSTERING_FOLDER}\\";        

        //[TestMethod]
        //[DataRow(2)]
        //[DataRow(3)]
        //[DataRow(4)]
        //[DataRow(5)]
        //[DataRow(6)]
        //[DataRow(7)]
        //[DataRow(8)]
        //[DataRow(9)]
        //[DataRow(10)]
        //[DataRow(11)]
        //[DataRow(12)]
        //[DataRow(13)]
        //[DataRow(14)]
        //[DataRow(15)]
        //public void RunKMedoidsMicrosoftTest(int numClusters)
        //{
        //    var inputContent = new FileSheet(ConstAux.PRE_PROCESSING_FILE_FULL_PATH);

        //    double[][] rawData = inputContent.GetDoubleMatrix(ConstAux.METRICS_FILE_INITIAL_INDEX_FEATURES, ConstAux.METRICS_FILE_FINAL_INDEX_FEATURES);

        //    KMedoids clusterAlg = new KMedoids(numClusters);
        //    clusterAlg.ComputeError = true;
        //    var resultLearn = clusterAlg.Learn(rawData);
        //    var finalLabel= resultLearn.Decide(rawData);
        //    var centroids = clusterAlg.Clusters.Centroids;

        //    var distancesList = new double[rawData.Length];
        //    for (int i = 0; i < distancesList.Length; i++)
        //    {
        //        distancesList[i] = Distance(rawData[i], centroids[finalLabel[i]]);
        //    }

        //    inputContent.AddColumnValues(finalLabel, "cluster");
        //    inputContent.AddColumnValues(distancesList, "distance");

        //    var outputFilePath = $"{DIR_PATH_OUTPUT}{CommonActions.GetFileNameByAlgorithmAndNumCluster(ACTION_CLUSTERING_TYPE, ALGORITHM_NAME, numClusters)}";
        //    inputContent.SaveToFile(outputFilePath);

        //    var logFilePath = $"{DIR_PATH_OUTPUT}LogClustering.csv";

        //    var result = finalLabel.GroupBy(s => s).Select(g => new { Cluster = g.Key, Count = g.Count() });
        //    foreach (var e in result)
        //    {
        //        CommonActions.WriteLogClustering(logFilePath, "", ALGORITHM_NAME, numClusters, e.Cluster, e.Count, clusterAlg.Iterations, distancesList.Sum());
        //    }
        //}

        //[TestMethod]
        //public void MedoidsFindMLBestK()
        //{
        //    var inputContent = new FileSheet(ConstAux.PRE_PROCESSING_FILE_FULL_PATH);

        //    double[][] rawData = inputContent.GetDoubleMatrix(ConstAux.METRICS_FILE_INITIAL_INDEX_FEATURES, ConstAux.METRICS_FILE_FINAL_INDEX_FEATURES);

        //    double[] distancesList;

        //    for (int numClusters = 2; numClusters < 15; numClusters++)
        //    {
        //        KMedoids clusterAlg = new KMedoids(numClusters);
        //        clusterAlg.ComputeError = true;
        //        var resultLearn = clusterAlg.Learn(rawData);
        //        var finalResult = resultLearn.Decide(rawData);
        //        var centroids = clusterAlg.Clusters.Centroids;

        //        distancesList = new double[rawData.Length];
        //        for (int i = 0; i < distancesList.Length; i++)
        //        {
        //            distancesList[i] = Distance(rawData[i], centroids[finalResult[i]]);
        //        }
        //        //double sumDistances = clusterAlg.Distances.Sum();

        //        //Console.WriteLine($"{numClusters};{sumDistances}");                
        //        Console.WriteLine($"{numClusters};{distancesList.Sum()}");

        //    }
        //}

        //private double Distance(double[] tuple, double[] center)
        //{
        //    // Euclidean distance between two vectors
        //    double sumSquaredDiffs = 0.0;
        //    for (int j = 0; j < tuple.Length; ++j)
        //        sumSquaredDiffs += Math.Pow(tuple[j] - center[j], 2);
        //    return Math.Sqrt(sumSquaredDiffs);
        //}

    }
}
