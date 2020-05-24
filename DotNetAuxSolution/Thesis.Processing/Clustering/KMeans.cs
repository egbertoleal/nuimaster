using System;
using System.Linq;

// Adapted Code from: https://visualstudiomagazine.com/Articles/2013/12/01/K-Means-Data-Clustering-Using-C.aspx?Page=2

namespace Thesis.Processing.Clustering
{
    public class KMeans
    {

        public double[][] OriginalData;
        public int[] Clustering;
        public double[][] Means;
        public int NumClusters;
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

        public KMeans(double[][] rawData, int numClusters, int maxIterations = 0)
        {
            NumClusters = numClusters;            
            OriginalData = rawData; // so large values don't dominate
            //OriginalData = Normalized(rawData); // so large values don't dominate
            MaxIterations = maxIterations == 0 ? NumRows * 10 : maxIterations;
        }

        public int Cluster()
        {
            InitClustering();
            Allocate();

            bool changed = true; // was there a change in at least one cluster assignment?
            bool success = true; // were all means able to be computed? (no zero-count clusters)
            int counter = 0;
            while (changed == true && success == true && counter < MaxIterations)
            {
                counter++;
                success = UpdateMeans(); // compute new cluster means if possible. no effect if fail
                changed = UpdateClustering(); // (re)assign tuples to clusters. no effect if fail
            }

            return counter;
        }

        private double[][] Normalized(double[][] rawData)
        {
            // normalize raw data by computing (x - mean) / stddev
            // primary alternative is min-max:
            // v' = (v - min) / (max - min)

            // make a copy of input data
            double[][] result = new double[rawData.Length][];
            for (int i = 0; i < rawData.Length; ++i)
            {
                result[i] = new double[rawData[i].Length];
                Array.Copy(rawData[i], result[i], rawData[i].Length);
            }

            for (int j = 0; j < result[0].Length; ++j) // each col
            {
                double colSum = 0.0;
                for (int i = 0; i < result.Length; ++i)
                    colSum += result[i][j];
                double mean = colSum / result.Length;
                double sum = 0.0;
                for (int i = 0; i < result.Length; ++i)
                    sum += (result[i][j] - mean) * (result[i][j] - mean);
                double sd = sum / result.Length;
                for (int i = 0; i < result.Length; ++i)
                    result[i][j] = (result[i][j] - mean) / sd;
            }
            return result;
        }

        private void InitClustering()
        {
            Random random = new Random();
            int[] countPerCluster = new int[NumClusters];
            Array.Clear(countPerCluster, 0, countPerCluster.Length);

            int[] clustering = new int[NumRows];
            for (int i = 0; i < clustering.Length; ++i)
            {
                var randomCluster = random.Next(0, NumClusters); // other assignments random
                countPerCluster[randomCluster]++;
                clustering[i] = randomCluster;
            }

            // Assign at least one sample for each cluster
            for (int i = 0; i < countPerCluster.Length; ++i)
            {
                if(countPerCluster[i] == 0)
                {
                    for (int j = 0; j < clustering.Length; j++)
                    {
                        // Verify if cluster has more than 1
                        if(i != clustering[j] && countPerCluster[clustering[j]] > 1)
                        {
                            countPerCluster[clustering[j]]--;
                            countPerCluster[i]++;
                            clustering[j] = i;
                        }
                    }
                }

            } 
            
            Clustering = clustering;
        }

        private void Allocate()
        {
            // convenience matrix allocator for Cluster()
            double[][] result = new double[NumClusters][];
            for (int k = 0; k < NumClusters; ++k)
                result[k] = new double[NumFeatures];
            Means = result;
        }

        private bool UpdateMeans()
        {
            int numClusters = Means.Length;
            int[] clusterCounts = new int[numClusters];
            for (int i = 0; i < OriginalData.Length; ++i)
            {
                int cluster = Clustering[i];
                ++clusterCounts[cluster];
            }

            for (int k = 0; k < numClusters; ++k)
                if (clusterCounts[k] == 0)
                    return false; // bad clustering. no change to means[][]

            // update, zero-out means so it can be used as scratch matrix 
            for (int k = 0; k < Means.Length; ++k)
                for (int j = 0; j < Means[k].Length; ++j)
                    Means[k][j] = 0.0;

            for (int i = 0; i < OriginalData.Length; ++i)
            {
                int cluster = Clustering[i];
                for (int j = 0; j < OriginalData[i].Length; ++j)
                    Means[cluster][j] += OriginalData[i][j]; // accumulate sum
            }

            for (int k = 0; k < Means.Length; ++k)
                for (int j = 0; j < Means[k].Length; ++j)
                    Means[k][j] /= clusterCounts[k]; // danger of div by 0
            return true;
        }

        private bool UpdateClustering()
        {
            // (re)assign each tuple to a cluster (closest mean)
            // returns false if no tuple assignments change OR
            // if the reassignment would result in a clustering where
            // one or more clusters have no tuples.

            bool changed = false;

            int[] newClustering = new int[Clustering.Length]; // proposed result
            Array.Copy(Clustering, newClustering, Clustering.Length);

            double[] TempRowDistances = new double[NumClusters]; // distances from curr tuple to each mean
            Distances = new double[NumRows]; // distances from each row to the mean

            for (int i = 0; i < OriginalData.Length; ++i) // walk thru each tuple
            {
                for (int k = 0; k < NumClusters; ++k)
                    TempRowDistances[k] = Distance(OriginalData[i], Means[k]); // compute distances from curr tuple to all k means

                int newClusterID = MinIndex(TempRowDistances); // find closest mean ID
                Distances[i] = TempRowDistances[newClusterID]; //assign min distance for the row
                if (newClusterID != newClustering[i])
                {
                    changed = true;
                    newClustering[i] = newClusterID; // update
                }
            }

            if (changed == false)
                return false; // no change so bail and don't update clustering[][]

            // check proposed clustering[] cluster counts
            int[] clusterCounts = new int[NumClusters];
            for (int i = 0; i < OriginalData.Length; ++i)
            {
                int cluster = newClustering[i];
                ++clusterCounts[cluster];
            }

            for (int k = 0; k < NumClusters; ++k)
                if (clusterCounts[k] == 0)
                    return false; // bad clustering. no change to clustering[][]

            Array.Copy(newClustering, Clustering, newClustering.Length); // update
            return true; // good clustering and at least one change
        }

        private double Distance(double[] tuple, double[] mean)
        {
            // Euclidean distance between two vectors for UpdateClustering()
            // consider alternatives such as Manhattan distance
            double sumSquaredDiffs = 0.0;
            for (int j = 0; j < tuple.Length; ++j)
                sumSquaredDiffs += Math.Pow(tuple[j] - mean[j], 2);
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
