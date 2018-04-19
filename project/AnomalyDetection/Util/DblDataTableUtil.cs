using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Util
{
    public class DblDataTableUtil
    {
        public static List<List<double>> LoadDataSet(string filepath)
        {
            List<List<double>> data_set = new List<List<double>>();

            string line;
            using (StreamReader reader = new StreamReader(filepath))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(new char[] { ' ', '\t', ',' });
                    List<double> feature_values = new List<double>();
                    for (int i = 0; i < values.Length; ++i)
                    {
                        double value;
                        if (double.TryParse(values[i], out value))
                        {
                            feature_values.Add(value);
                        }
                    }

                    data_set.Add(feature_values);
                }
            }
            return data_set;
        }

        public static void GetSize(List<List<double>> X, out int row_count, out int col_count)
        {
            row_count = X.Count;
            col_count = 0;
            if (row_count > 0)
            {
                col_count = X[0].Count;
            }
        }

        public static List<List<double>> SubMatrix(List<List<double>> X, int new_row_count, int new_col_count)
        {
            List<List<double>> sub_matrix = new List<List<double>>();

            for (int i = 0; i < new_row_count; ++i)
            {
                List<double> row = X[i];
                List<double> sub_row = new List<double>();
                for (int j = 0; j < new_col_count; ++j)
                {
                    sub_row.Add(row[j]);
                }
                sub_matrix.Add(sub_row);
            }

            return sub_matrix;
        }

        public static double[,] Convert2DArray(List<List<double>> X)
        {
            int row_count, col_count;
            DblDataTableUtil.GetSize(X, out row_count, out col_count);
            double[,] X_matrix = new double[row_count, col_count];

            for (int i = 0; i < row_count; ++i)
            {
                List<double> row = X[i];
                for (int j = 0; j < col_count; ++j)
                {
                    X_matrix[i, j] = row[j];
                }
            }

            return X_matrix;
        }
    }
}
