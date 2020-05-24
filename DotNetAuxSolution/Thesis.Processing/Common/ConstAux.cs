namespace Thesis.Processing.Common
{
    public static class ConstAux
    {
        #region OO Metrics File
        public const string DOT_NET_PRE_PROCESSING_FILE_FULL_PATH = @"D:\FilesThesis\2 - PreProcessed\02-DotNetPreProcessedInput.csv";
        public const string JAVA_TEST_PROJECT_PRE_PROCESSING_FILE_FULL_PATH = @"D:\FilesThesis\2 - PreProcessed\01-PreProcessedInput.csv";
        public const string FIREBASE_PRE_PROCESSING_FILE_FULL_PATH = @"D:\FilesThesis\2 - PreProcessed\firebase\01-PreProcessedInput.csv";
        public const string APACHE_PRE_PROCESSING_FILE_FULL_PATH = @"D:\FilesThesis\2 - PreProcessed\apache\01-PreProcessedInput.csv";
        public const int METRICS_FILE_INITIAL_INDEX_FEATURES = 2;
        public const int METRICS_FILE_FINAL_INDEX_FEATURES = 6;
        public const int METRICS_FILE_TEST_CLASS_NAME_INDEX = 0;
        public const int METRICS_FILE_TEST_METHOD_NAME_INDEX = 1;

        #endregion

        #region Req Cover File

        public const string REQ_COVER_FILE_FULL_PATH = @"D:\FilesThesis\1- ReqCover\03-UniqueCoverFile.csv";
        public const string FIREBASE_REQ_COVER_FILE_FULL_PATH = @"D:\FilesThesis\1- ReqCover\firebase\03-UniqueCoverFile.csv";
        public const string APACHE_REQ_COVER_FILE_FULL_PATH = @"D:\FilesThesis\1- ReqCover\apache\03-UniqueCoverFile.csv";
        public const int REQ_COVER_CLASS_NAME_INDEX = 4;
        public const int REQ_COVER_METHOD_NAME_INDEX = 5;
        public const int REQ_COVER_TEST_CASE_INDEX = 3;

        #endregion

        #region Clustering

        public const string ROOT_DIR_CLUSTERING_FOLDER = @"D:\FilesThesis\3 - Clustering\";
        public const string ACTION_CLUSTERING_TYPE = "Clustering";
        public const string ALGORITHM_NAME_KMEDOIDS = "KMedoids";
        public const string ALGORITHM_NAME_KMEDOIDS_ML = "KMedoidsML";        
        public const string ALGORITHM_NAME_KMEANS = "KMeans";

        #endregion

        #region TestReduction

        public const string ROOT_DIR_TEST_REDUCTION_FOLDER = @"D:\FilesThesis\4 - Reduction\";
        public const string ACTION_REDUCTION_TYPE = "Reduction";
        public const string REDUCTION_ALGORITHM_PAPER = "Paper";
        public const string REDUCTION_ALGORITHM_ADAPTED = "Adapted";        

        #endregion

        #region Clustering

        public const string PROJECT_NAME_JAVA_TEST = "JavaTest";
        public const string PROJECT_NAME_FIREBASE = "Firebase";
        public const string PROJECT_NAME_APACHE = "Apache";

        #endregion

    }
}
