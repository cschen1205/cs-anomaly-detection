using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SimuKit.ML.Lang;
using SimuKit.ML.Util;

namespace SimuKit.ML.AnomalyDetection.UT
{
    public class MultiVariateGaussianDistributionAD_UT
    {
        

        public static void Test_LoadDataSet()
        {
            List<MLDataPoint> X1 = MLDataPointUtil.LoadDataSet("X1.txt");
            List<MLDataPoint> Xval1 = MLDataPointUtil.LoadDataSet("Xval1.txt");
            List<MLDataPoint> yval1 = MLDataPointUtil.LoadDataSet("yval1.txt");

            List<MLDataPoint> X2 = MLDataPointUtil.LoadDataSet("X2.txt");
            List<MLDataPoint> Xval2 = MLDataPointUtil.LoadDataSet("Xval2.txt");
            List<MLDataPoint> yval2 = MLDataPointUtil.LoadDataSet("yval2.txt");

            int row_count, col_count;

            MLDataPointUtil.GetSize(X1, out row_count, out col_count);
            Console.WriteLine("size(X1)=[{0} {1}]", row_count, col_count);

            MLDataPointUtil.GetSize(Xval1, out row_count, out col_count);
            Console.WriteLine("size(Xval1)=[{0} {1}]", row_count, col_count);

            MLDataPointUtil.GetSize(yval1, out row_count, out col_count);
            Console.WriteLine("size(yval1)=[{0} {1}]", row_count, col_count);

            MLDataPointUtil.GetSize(X2, out row_count, out col_count);
            Console.WriteLine("size(X2)=[{0} {1}]", row_count, col_count);

            MLDataPointUtil.GetSize(Xval2, out row_count, out col_count);
            Console.WriteLine("size(Xval2)=[{0} {1}]", row_count, col_count);

            MLDataPointUtil.GetSize(yval2, out row_count, out col_count);
            Console.WriteLine("size(yval2)=[{0} {1}]", row_count, col_count);
        }

        

        /// <summary>
        /// test probability calculation
        /// </summary>
        public static void Test_CalcProbability_X(int data_set_index)
        {
            List<MLDataPoint> X = MLDataPointUtil.LoadDataSet(string.Format("X{0}.txt", data_set_index));

            MultiVariateGaussianDistributionAD<MLDataPoint> algorithm = new MultiVariateGaussianDistributionAD<MLDataPoint>();
            algorithm.ComputeGaussianDistribution(X);

            List<MLDataPoint> correct_p = MLDataPointUtil.LoadDataSet(string.Format("p{0}.txt", data_set_index));

            int row_count = X.Count;

            double total_error = 0;
            for (int i = 0; i < row_count; ++i)
            {
                double p = algorithm.CalcProbability(X[i]);
                double error=System.Math.Abs(correct_p[i][0] - p);
                total_error += error;
                Console.WriteLine("p={0} correct_p={1}", p, correct_p[i]);
            }

            Console.WriteLine("Total error: {0}", total_error);

        }

        /// <summary>
        /// Test cross validation 
        /// </summary>
        public static void Test_CalcProbability_Xval(int data_set_index)
        {
            List<MLDataPoint> X = MLDataPointUtil.LoadDataSet(string.Format("X{0}.txt", data_set_index));
            List<MLDataPoint> Xval = MLDataPointUtil.LoadDataSet(string.Format("Xval{0}.txt", data_set_index));

            MultiVariateGaussianDistributionAD<MLDataPoint> algorithm = new MultiVariateGaussianDistributionAD<MLDataPoint>();
            algorithm.ComputeGaussianDistribution(X);

            List<MLDataPoint> correct_pval = MLDataPointUtil.LoadDataSet(string.Format("pval{0}.txt", data_set_index));

            int row_count = Xval.Count;

            double total_error = 0;
            for (int i = 0; i < row_count; ++i)
            {
                double pval = algorithm.CalcProbability(Xval[i]);
                double error = System.Math.Abs(correct_pval[i][0] - pval);
                total_error += error;
                Console.WriteLine("pval={0} correct_pval={1}", pval, correct_pval[i]);
            }

            Console.WriteLine("Total error: {0}", total_error);
        }

        protected static void SelectThreshold(int data_set_index, out double F1Score, out double threshold)
        {
            List<MLDataPoint> X = MLDataPointUtil.LoadDataSet(string.Format("X{0}.txt", data_set_index));
            List<MLDataPoint> Xval = MLDataPointUtil.LoadDataSet(string.Format("Xval{0}.txt", data_set_index));
            List<MLDataPoint> yval_temp = MLDataPointUtil.LoadDataSet(string.Format("yval{0}.txt", data_set_index));

            bool[] yval = new bool[yval_temp.Count];

            int row_count = yval_temp.Count;
            for (int i = 0; i < row_count; ++i)
            {
                yval[i] = yval_temp[i][0] > 0.5;
            }

            MultiVariateGaussianDistributionAD<MLDataPoint> algorithm = new MultiVariateGaussianDistributionAD<MLDataPoint>();
            
            threshold = algorithm.SelectThreshold(X, Xval, yval, out F1Score);
        }

        public static void Test_SelectThreshold(int data_set_index)
        {
            double threshold, F1Score;
            SelectThreshold(data_set_index, out F1Score, out threshold);

            Console.WriteLine("Threshold: {0}", threshold);
            Console.WriteLine("F1Score: {0}", F1Score);

        }

        public static void Test_FindOutliers(int data_set_index)
        {
            double threshold, F1Score;
            SelectThreshold(data_set_index, out F1Score, out threshold);

            List<MLDataPoint> X = MLDataPointUtil.LoadDataSet(string.Format("X{0}.txt", data_set_index));
            List<MLDataPoint> correct_outliers = MLDataPointUtil.LoadDataSet(string.Format("outliers{0}.txt", data_set_index));

            Console.WriteLine("Correct Outliers:");
            for (int i = 0; i < correct_outliers.Count; ++i)
            {
                Console.WriteLine("{0}", correct_outliers[i][0]);
            }

            MultiVariateGaussianDistributionAD<MLDataPoint> algorithm = new MultiVariateGaussianDistributionAD<MLDataPoint>();
            algorithm.ComputeGaussianDistribution(X);
            List<int> outliers=algorithm.FindOutliers(X, threshold);

            Console.WriteLine("Predict Outliers:");
            for (int i = 0; i < outliers.Count; ++i)
            {
                Console.WriteLine("{0}", outliers[i]+1);
            }

            Console.WriteLine("Point Count: {0}", X.Count);

            Console.WriteLine("Threshold: {0}", threshold);

            Console.WriteLine("Predict Outliers Count; {0} Correct Outliers Count: {1}", outliers.Count, correct_outliers.Count);
        }

    }
}
