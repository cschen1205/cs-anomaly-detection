using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lang;
using System.IO;

namespace Util
{
    public class MLDataPointUtil
    {
        public static void GetSize(List<MLDataPoint> X, out int row_count, out int col_count)
        {
            row_count = X.Count;
            col_count = 0;
            if (row_count > 0)
            {
                col_count = X[0].Dimension;
            }
        }

        public static List<MLDataPoint> LoadDataSet(string filepath)
        {
            List<MLDataPoint> data_set = new List<MLDataPoint>();

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
                    MLDataPoint point = new FeatureVector(feature_values.ToArray(), false, false);
                    data_set.Add(point);
                }
            }
            return data_set;
        }
    }
}
