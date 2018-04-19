using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Util
{
    public class IntDataTableUtil
    {
        public static List<List<int>> LoadDataSet(string filepath)
        {
            List<List<int>> data_set = new List<List<int>>();

            string line;
            using (StreamReader reader = new StreamReader(filepath))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(new char[] { ' ', '\t', ',' });
                    List<int> feature_values = new List<int>();
                    for (int i = 0; i < values.Length; ++i)
                    {
                        int value;
                        if (int.TryParse(values[i], out value))
                        {
                            feature_values.Add(value);
                        }
                        else
                        {
                            double dvalue;
                            if (double.TryParse(values[i], out dvalue))
                            {
                                value = (int)dvalue;
                                feature_values.Add(value);
                            }
                        }
                    }

                    data_set.Add(feature_values);
                }
            }
            return data_set;
        }

        public static void GetSize(List<List<int>> X, out int row_count, out int col_count)
        {
            row_count = X.Count;
            col_count = 0;
            if (row_count > 0)
            {
                col_count = X[0].Count;
            }
        }

        public static List<List<int>> SubMatrix(List<List<int>> X, int new_row_count, int new_col_count)
        {
            List<List<int>> sub_matrix = new List<List<int>>();

            for (int i = 0; i < new_row_count; ++i)
            {
                List<int> row = X[i];
                List<int> sub_row = new List<int>();
                for (int j = 0; j < new_col_count; ++j)
                {
                    sub_row.Add(row[j]);
                }
                sub_matrix.Add(sub_row);
            }

            return sub_matrix;
        }

        public static int[,] Convert2DArray(List<List<int>> X)
        {
            int row_count, col_count;
            GetSize(X, out row_count, out col_count);
            int[,] X_matrix = new int[row_count, col_count];

            for (int i = 0; i < row_count; ++i)
            {
                List<int> row = X[i];
                for (int j = 0; j < col_count; ++j)
                {
                    X_matrix[i, j] = row[j];
                }
            }

            return X_matrix;
        }
    }
}
