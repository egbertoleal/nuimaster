using System.Collections.Generic;

namespace Thesis.Processing.Data
{
    public class FileContent
    {
        public int IndexFile { get; set; }
        public string TestClassName { get; set; }
        public string TestMethodName { get; set; }
        public List<string> ListReqCover { get; set; }

        public int QtdReqCover 
        { 
            get
            {
                return ListReqCover.Count;
            }
        }

        public int ClusterNumber { get; set; }
        public double DistanceToCentroid { get; set; }
        public bool TestCasesIncluded { get; set; }
    }
}
