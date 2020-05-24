using System.Collections.Generic;
using System.Linq;
using Thesis.Processing.Data;

namespace Thesis.Processing.TestCaseReduction
{
    public class GreedyReduction
    {   
        public List<string> RequirementsToCover;
        public List<FileContent> OriginalListTestCase;
        public List<FileContent> ReducedListTestCase;

        public GreedyReduction(List<FileContent> contentCluster)
        {
            OriginalListTestCase = contentCluster;
            RequirementsToCover = contentCluster.SelectMany(x => x.ListReqCover.ToList()).ToList().Distinct().ToList();
            ReducedListTestCase = new List<FileContent>();
        }


        public List<FileContent> GetReducededCluster()
        {
            while(RequirementsToCover.Count > 0)
            {
                var testCaseWithGreatestCover = OriginalListTestCase.OrderByDescending(x => x.QtdReqCover).First();
                AddToTestSuite(testCaseWithGreatestCover);
            }

            return ReducedListTestCase;
        }

        private void AddToTestSuite(FileContent testMethod)
        {
            OriginalListTestCase.Remove(testMethod);            
            foreach (var reqTest in testMethod.ListReqCover)
            {
                var testMethodsWithReqItem = OriginalListTestCase.Where(x => x.ListReqCover.Contains(reqTest));

                foreach (var testWithSameCover in testMethodsWithReqItem)
                {
                    testWithSameCover.ListReqCover.Remove(reqTest);
                }

                RequirementsToCover.Remove(reqTest);
            }

            ReducedListTestCase.Add(testMethod);
        }


    }
}
