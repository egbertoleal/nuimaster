using System;
using System.Collections.Generic;
using System.Linq;

namespace Thesis.Processing.Clustering
{
    public class KMedoids
    {

        public double[][] OriginalData;        
        public int NumClusters;
        public int[] Clustering;
        public int[] Medoids;
        public double[] Distances;

        private int MaxIterations;

        public int NumRows
        {
            get
            {
                return OriginalData.Length;
            }
        }

        public int NumFeatures
        {
            get
            {
                return OriginalData[0].Length;
            }
        }

        public KMedoids(double[][] rawData, int numClusters, int maxIterations = 0)
        {
            NumClusters = numClusters;            
            OriginalData = rawData;
            MaxIterations = maxIterations == 0 ? NumRows * 10 : maxIterations;
        }

        public int Cluster()
        {
            int[] currentCentroids = InitMedoids();            
            int[] currentClusterLabel;
            double[] currentDistanceToCentroid;
            AssignToCluster(currentCentroids, out currentDistanceToCentroid, out currentClusterLabel);

            bool changed = true;
            int counter = 0;
            int totalChanges = 0;
            int newBestCentroid;

            while (changed && counter < MaxIterations)            
            {
                changed = false;
                counter++;

                for (int indexCluster = 0; indexCluster < NumClusters; indexCluster++)
                {
                    newBestCentroid = GetBestCentroid(currentCentroids, currentClusterLabel, indexCluster);
                    if(newBestCentroid != currentCentroids[indexCluster])
                    {
                        currentCentroids[indexCluster] = newBestCentroid;
                        changed = true;
                        totalChanges++;
                    }                    
                }

                AssignToCluster(currentCentroids, out currentDistanceToCentroid, out currentClusterLabel);
            }

            Clustering = currentClusterLabel;
            Medoids = currentCentroids;
            Distances = currentDistanceToCentroid;

            return counter;
        }      

        private int[] InitMedoids()
        {
            int[] newMedoids = new int[NumClusters];
            
            for (int i = 0; i < newMedoids.Length; ++i)
                newMedoids[i] = -1;

            for (int i = 0; i < newMedoids.Length; ++i)
            {
                newMedoids[i] = SelectRandomMedoid(newMedoids);
            }

            return newMedoids;
        }

        private int SelectRandomMedoid(int[] currentCentroidList)
        {
            Random random = new Random();

            int randomMedoidIndex = -1;

            // Avoid same centroid to different cluster
            while (randomMedoidIndex < 0 || currentCentroidList.Contains(randomMedoidIndex))
            {
                randomMedoidIndex = random.Next(0, NumRows);
            }

            return randomMedoidIndex;
        }

        private int GetBestCentroid(int[] currentCentroidsList, int[] currentClustersLabel, int indexCluster)
        {   
            var rowsInsideCluster = GetListIndexInsideCluster(currentClustersLabel, indexCluster);
            int bestCentroidIndex = currentCentroidsList[indexCluster];
            int newCentroidIndex;
            double bestCentroidCost;
            double newCentroidCost; 

            for (int indexNewCentroid = 0; indexNewCentroid < rowsInsideCluster.Length; indexNewCentroid++)
            {
                bestCentroidCost = 0;
                newCentroidCost = 0;
                newCentroidIndex = rowsInsideCluster[indexNewCentroid];

                if (bestCentroidIndex != newCentroidIndex)
                {
                    for (int indexClusterSample = 0; indexClusterSample < rowsInsideCluster.Length; indexClusterSample++)
                    {
                        bestCentroidCost += Distance(OriginalData[rowsInsideCluster[indexClusterSample]], OriginalData[bestCentroidIndex]);
                        newCentroidCost += Distance(OriginalData[rowsInsideCluster[indexClusterSample]], OriginalData[newCentroidIndex]);                        
                    }

                    if(newCentroidCost < bestCentroidCost)
                    {
                        bestCentroidIndex = newCentroidIndex;
                    }
                }
            }

            return bestCentroidIndex;
        }

        private int[] GetListIndexInsideCluster(int[] currentClustersLabel, int indexCluster)
        {
            var listIndexInsideCluster = new List<int>();
            for (int indexRow = 0; indexRow < currentClustersLabel.Length; indexRow++)
            {
                if (currentClustersLabel[indexRow] == indexCluster)
                {
                    listIndexInsideCluster.Add(indexRow);
                }
            }

            return listIndexInsideCluster.ToArray();
        }

        private void AssignToCluster(int[] listCentroids, out double[] outDistanceToCentroid, out int[] outClusterLabel)
        {   
            outClusterLabel = new int[NumRows];
            outDistanceToCentroid = new double[NumRows];

            double[] TempRowDistances = new double[NumClusters];

            for (int i = 0; i < OriginalData.Length; ++i)
            {
                for (int k = 0; k < NumClusters; ++k)
                    TempRowDistances[k] = Distance(OriginalData[i], OriginalData[listCentroids[k]]);

                int newClusterID = MinIndex(TempRowDistances);                
                outClusterLabel[i] = newClusterID;
                outDistanceToCentroid[i] = TempRowDistances[newClusterID];                
            }            
        }

        private double Distance(double[] tuple, double[] center)
        {
            // Euclidean distance between two vectors
            double sumSquaredDiffs = 0.0;
            for (int j = 0; j < tuple.Length; ++j)
                sumSquaredDiffs += Math.Pow(tuple[j] - center[j], 2);
            return Math.Sqrt(sumSquaredDiffs);
        }

        private int MinIndex(double[] distances)
        {
            // index of smallest value in array
            // helper for UpdateClustering()
            int indexOfMin = 0;
            double smallDist = distances[0];
            for (int k = 0; k < distances.Length; ++k)
            {
                if (distances[k] < smallDist)
                {
                    smallDist = distances[k];
                    indexOfMin = k;
                }
            }
            return indexOfMin;
        }
       
    }
}
