using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Thesis.Processing.Data
{
    public class FileSheet
    {

        #region Class fields
        public DataTable FileContent = new DataTable();
        public string InputFilePath;
        public bool HasHeaderRow;

        public int TotalRowsWithContent
        {
            get
            {
                return FileContent.Rows.Count;
            }
        }

        public int TotalNumColumn
        {
            get
            {
                return FileContent.Columns.Count;
            }
        }

        #endregion

        #region Constructors

        public FileSheet(string inputFilePath, bool hasHeaderRow = true, char splitChar = ',')
        {
            InputFilePath = inputFilePath;
            HasHeaderRow = hasHeaderRow;
            ReadFile(InputFilePath, splitChar);            
        }

        #endregion

        #region public methods

        public double[][] GetDoubleMatrix(int initialIndex, int finalIndex)
        {
            int totalNumberOfFeatures = finalIndex - initialIndex + 1;
            var contentDouble = new double[TotalRowsWithContent][];

            for (int rowIndex = 0; rowIndex < contentDouble.Length; rowIndex++)
            {
                contentDouble[rowIndex] = new double[totalNumberOfFeatures];

                for (int columnIndex = 0; columnIndex < totalNumberOfFeatures; columnIndex++)
                {
                    contentDouble[rowIndex][columnIndex] = GetColumnDoubleValue(rowIndex, columnIndex + initialIndex);
                }
            }

            return contentDouble;
        }

        public string[][] GetStringMatrix()
        {   
            var returnContent = new string[TotalRowsWithContent][];

            for (int rowIndex = 0; rowIndex < returnContent.Length; rowIndex++)
            {
                returnContent[rowIndex] = new string[TotalNumColumn];

                for (int columnIndex = 0; columnIndex < TotalNumColumn; columnIndex++)
                {
                    returnContent[rowIndex][columnIndex] = GetColumnStringValue(rowIndex, columnIndex);
                }
            }

            return returnContent;
        }

        public void AddColumnValues(bool[] columnValues, string columnName)
        {
            AddColumnValues(columnValues.Select(x => x.ToString()).ToArray(), columnName);
        }

        public void AddColumnValues(double[] columnValues, string columnName)
        {
            AddColumnValues(columnValues.Select(x => x.ToString()).ToArray(), columnName);
        }

        public void AddColumnValues(int[] columnValues, string columnName)
        {
            AddColumnValues(columnValues.Select(x => x.ToString()).ToArray(), columnName);
        }

        public void AddColumnValues(string[] columnValues, string columnName)
        {
            AddColumn(columnName);
            for (int rowIndex = 0; rowIndex < columnValues.Length; rowIndex++)
            {
                FileContent.Rows[rowIndex][columnName] = columnValues[rowIndex];
            }
        }

        public void SaveToFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (var output = new StreamWriter(filePath))
            {
                output.WriteLine(GetCSVHeaderString());

                for (int rowIndex = 0; rowIndex < TotalRowsWithContent; rowIndex++)
                {
                    var rowContent = FileContent.Rows[rowIndex].ItemArray.Select(c => c.ToString()).ToArray();
                    output.WriteLine(string.Join(",", rowContent));;
                }
            }
        }

        public double GetColumnDoubleValue(int rowIndex, int columnIndex)
        {
            return Convert.ToDouble(GetColumnStringValue(rowIndex,columnIndex));
        }

        public double GetColumnDoubleValue(int rowIndex, string columnName)
        {
            return Convert.ToDouble(GetColumnStringValue(rowIndex, columnName));
        }

        public bool GetColumnBooleanValue(int rowIndex, string columnName)
        {
            return Convert.ToBoolean(GetColumnStringValue(rowIndex, columnName));
        }

        public int GetColumnIntValue(int rowIndex, int columnIndex)
        {
            return Convert.ToInt32(GetColumnStringValue(rowIndex, columnIndex));
        }

        public int GetColumnIntValue(int rowIndex, string columnName)
        {
            return Convert.ToInt32(GetColumnStringValue(rowIndex, columnName));
        }

        public string GetColumnStringValue(int rowIndex, int columnIndex)
        {
            return Convert.ToString(FileContent.Rows[rowIndex][columnIndex]);
        }

        public string GetColumnStringValue(int rowIndex, string columnName)
        {
            return Convert.ToString(FileContent.Rows[rowIndex][columnName]);
        }

        public List<FileContent> GetAllFileContents()
        {
            var returnList = new List<FileContent>();
            for (int i = 0; i < TotalRowsWithContent; i++)
            {
                returnList.Add(GetRowContent(i));
            }

            return returnList;
        }

        public FileContent GetRowContent(int rowIndex)
        {
            return new FileContent
            {
                IndexFile = rowIndex,
                TestClassName = GetColumnStringValue(rowIndex, "TestClassName"),
                TestMethodName = GetColumnStringValue(rowIndex, "TestMethodName"),
                ListReqCover = GetColumnStringValue(rowIndex, "ListReqCover").Split('-').ToList(),
                ClusterNumber = GetColumnIntValue(rowIndex, "cluster"),
                DistanceToCentroid = GetColumnDoubleValue(rowIndex, "distance"),
                TestCasesIncluded = FileContent.Columns.Contains("TestIncluded") ? GetColumnBooleanValue(rowIndex, "TestIncluded") : false
            };
        }

        public void Dispose()
        {
            FileContent.Dispose();
        }

        #endregion

        #region private / internal

        private string GetCSVHeaderString()
        {
            string[] columnNames = (from dc in FileContent.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
            return string.Join(",", columnNames);
        }        

        private void SetColumnValue(int rowIndex, int columnIndex, string value)
        {
            FileContent.Rows[rowIndex][columnIndex] = value;
        }

        private void ReadFile(string inputFilePath, char splitChar)
        {
            string[] splitLineContent;

            using (var input = File.OpenText(inputFilePath))
            {
                string line;
                bool isFirstRow = true;
                while (null != (line = input.ReadLine()))
                {
                    splitLineContent = line.Split(splitChar);

                    if (isFirstRow)
                    {
                        if (HasHeaderRow)
                        {
                            AddColumns(splitLineContent);
                        }
                        isFirstRow = false;
                    }
                    else
                    {
                        AddRow(splitLineContent);
                    }
                }
            }
        }

        private void AddRow(string[] rowContent)
        {
            var newRow = FileContent.NewRow();
            for (int i = 0; i < rowContent.Length; i++)
            {
                newRow[i] = rowContent[i];
            }
            FileContent.Rows.Add(newRow);
        }

        private void AddColumns(string[] columnsName)
        {
            foreach (var columnName in columnsName)
            {
                AddColumn(columnName);
            }
        }

        private void AddColumn(string columnName)
        {
            DataColumn column = new DataColumn(columnName);
            column.DataType = System.Type.GetType("System.String");

            FileContent.Columns.Add(column);
        }

        #endregion
    }
}
