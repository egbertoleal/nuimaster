using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thesis.Processing.Common;
using Thesis.Processing.TestCaseReduction;

namespace AuxProject.TestReduction
{
    [TestClass]
    public class AdaptedReductionProcessTest
    {
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
        public void RunOnKMedoidsClusterJavaTest(int numClusters)
        {   
            RunReductionFromInput(ConstAux.PROJECT_NAME_JAVA_TEST, ConstAux.ALGORITHM_NAME_KMEDOIDS, numClusters);
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
        public void RunOnKMedoidsClusterFirebase(int numClusters)
        {
            RunReductionFromInput(ConstAux.PROJECT_NAME_FIREBASE, ConstAux.ALGORITHM_NAME_KMEDOIDS, numClusters);
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
        public void RunOnKMedoidsClusterApache(int numClusters)
        {
            RunReductionFromInput(ConstAux.PROJECT_NAME_APACHE, ConstAux.ALGORITHM_NAME_KMEDOIDS, numClusters);
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
        public void RunOnKMeansClusterJavaTest(int numClusters)
        {   
            RunReductionFromInput(ConstAux.PROJECT_NAME_JAVA_TEST, ConstAux.ALGORITHM_NAME_KMEANS, numClusters);
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
        public void RunOnKMeansClusterFirebase(int numClusters)
        {
            RunReductionFromInput(ConstAux.PROJECT_NAME_FIREBASE, ConstAux.ALGORITHM_NAME_KMEANS, numClusters);
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
        public void RunOnKMeansClusterApache(int numClusters)
        {
            RunReductionFromInput(ConstAux.PROJECT_NAME_APACHE, ConstAux.ALGORITHM_NAME_KMEANS, numClusters);
        }

        public void RunReductionFromInput(string projectName, string clusteringAlgorithmName, int numClusters)
        {
            AdaptedReduction.RunReduction(projectName, clusteringAlgorithmName, numClusters);
        }

    }
}
