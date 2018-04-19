using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lang;

namespace AnomalyDetection.UT
{
    public class GaussianDistributionAD_UT
    {
        public static List<MLDataPoint> LoadDataSet(string filepath)
        {
            List<MLDataPoint> data_set = new List<MLDataPoint>();

            string line;
            using (StreamReader reader = new StreamReader(filepath))
            {
                while ((line = reader.ReadLine())!=null)
                {
                    string[] values=line.Split(new char[] { ' ', '\t', ',' });
                    List<double> feature_values = new List<double>();
                    for(int i=0; i < values.Length; ++i)
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

        public static void Test_LoadDataSet()
        {
            List<MLDataPoint> X1=LoadDataSet("X1.txt");
            List<MLDataPoint> Xval1 = LoadDataSet("Xval1.txt");
            List<MLDataPoint> yval1 = LoadDataSet("yval1.txt");

            List<MLDataPoint> X2 = LoadDataSet("X2.txt");
            List<MLDataPoint> Xval2 = LoadDataSet("Xval2.txt");
            List<MLDataPoint> yval2 = LoadDataSet("yval2.txt");

            int row_count, col_count;

            GetSize(X1, out row_count, out col_count);
            Console.WriteLine("size(X1)=[{0} {1}]", row_count, col_count);

            GetSize(Xval1, out row_count, out col_count);
            Console.WriteLine("size(Xval1)=[{0} {1}]", row_count, col_count);

            GetSize(yval1, out row_count, out col_count);
            Console.WriteLine("size(yval1)=[{0} {1}]", row_count, col_count);

            GetSize(X2, out row_count, out col_count);
            Console.WriteLine("size(X2)=[{0} {1}]", row_count, col_count);

            GetSize(Xval2, out row_count, out col_count);
            Console.WriteLine("size(Xval2)=[{0} {1}]", row_count, col_count);

            GetSize(yval2, out row_count, out col_count);
            Console.WriteLine("size(yval2)=[{0} {1}]", row_count, col_count);
        }

        public static void GetSize(List<MLDataPoint> X, out int row_count, out int col_count)
        {
            row_count = X.Count;
            col_count = 0;
            if (row_count > 0)
            {
                col_count = X[0].Dimension;
            }
        }

        public static void Test_ComputeGaussianDistribution()
        {
            List<MLDataPoint> X=LoadDataSet("X1.txt");

            GaussianDistributionAD<MLDataPoint> algorithm = new GaussianDistributionAD<MLDataPoint>();
            algorithm.ComputeGaussianDistribution(X);

            Console.WriteLine("{0}", algorithm);
        }
    }
}
