using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lang;
using MathNet.Numerics.LinearAlgebra.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace AnomalyDetection
{
    public class MultiVariateGaussianDistributionAD<T>
        where T : MLDataPoint
    {
        private List<int> mFeatureIndexList = new List<int>();
        private HashSet<int> mFeatureIndexSet = new HashSet<int>();
        private Matrix<double> mGaussian_Mu = null;
        private Matrix<double> mGaussian_Sigma2 = null;
        private double mEpsilon = 0.02;

        public double Epsilon
        {
            get { return mEpsilon; }
            set { mEpsilon = value; }
        }

        public void AddSelectedFeature(int feature_index)
        {
            if (mFeatureIndexSet.Contains(feature_index))
            {
                return;
            }
            mFeatureIndexList.Add(feature_index);
            mFeatureIndexSet.Add(feature_index);
        }

        public void ClearSelectedFeatures()
        {
            mFeatureIndexList.Clear();
            mFeatureIndexSet.Clear();
        }

        public double CalcProbability(T rec)
        {
            int n = mFeatureIndexList.Count;
            Matrix<double> x=new DenseMatrix(n, 1, 0);
            for(int index=0; index < n; ++index)
            {
                int feature_index=mFeatureIndexList[index];
                x[index, 0]=rec[feature_index];
            }

            double det=mGaussian_Sigma2.Determinant();
            Matrix<double> sigma_inverse = mGaussian_Sigma2.Inverse();
            Matrix<double> x_minus_mu = x - mGaussian_Mu;
            Matrix<double> x_minus_mu_transpose = x_minus_mu.Transpose();

            Matrix<double> v=x_minus_mu_transpose.Multiply(sigma_inverse).Multiply(x_minus_mu);

            double num2=System.Math.Pow(2 * System.Math.PI, n / 2.0) * System.Math.Sqrt(System.Math.Abs(det));
            //Console.WriteLine("num2: {0}", num2);
            return System.Math.Exp(-0.5 * v[0, 0]) / num2;
        }

        public bool IsAnomaly(T rec)
        {
            return IsAnomaly(rec, mEpsilon);
        }

        public bool IsAnomaly(T rec, double epsilon)
        {
            double p = CalcProbability(rec);

            Console.WriteLine("{0}", p);

            return p < epsilon;
        }

        public void ComputeGaussianDistribution(List<T> records)
        {
            int sample_count = records.Count;

            if (mFeatureIndexList.Count == 0)
            {
                int dim = records[0].Dimension;
                for (int i = 0; i < dim; ++i)
                {
                    AddSelectedFeature(i);
                }
            }

            int feature_count = mFeatureIndexList.Count;
            mGaussian_Mu = new DenseMatrix(feature_count, 1, 0);
            for (int index = 0; index < feature_count; ++index )
            {
                int feature_index = mFeatureIndexList[index];
                double mu = 0;
                for (int i = 0; i < sample_count; ++i)
                {
                    double x = records[i][feature_index];
                    mu += x;
                }
                mu /= sample_count;

                mGaussian_Mu[index, 0] = mu; 
            }

            mGaussian_Sigma2 = new DenseMatrix(feature_count, feature_count, 0);

            Matrix<double> X = new DenseMatrix(feature_count, 1, 0);
            for (int i = 0; i < sample_count; ++i)
            {
                for (int index = 0; index < feature_count; ++index)
                {
                    int feature_index = mFeatureIndexList[index];
                    X[index, 0] = records[i][feature_index];
                }
                Matrix<double> X_minus_mu = X - mGaussian_Mu;
                Matrix<double> X_minus_mu_transpose = X_minus_mu.Transpose();
                mGaussian_Sigma2=mGaussian_Sigma2+X_minus_mu.Multiply(X_minus_mu_transpose).Multiply(1.0 / sample_count);
            }

        }

        /// <summary>
        /// Method that finds a suitable threshold value for anomaly detection
        /// </summary>
        /// <param name="X">data points for the training set</param>
        /// <param name="Xval">data points for the cross validation set</param>
        /// <param name="yval">class label associated with the data points for the cross validation set, 1 for Anomal, 0 for Normal</param>
        /// <param name="max_F1Score">best value for F1 score on the (Xval, yval)</param>
        /// <returns>The best value for threshold which yields the highest F1Score for the evaluation on the cross validation set</returns>
        public double SelectThreshold(List<T> X, List<T> Xval, bool[] yval, out double max_F1Score)
        {
            ComputeGaussianDistribution(X);
            return SelectThreshold(Xval, yval, out max_F1Score);
        }

        public List<int> FindOutliers(List<T> X, double epsilon)
        {
            List<int> outliers=new List<int>();
            int row_count = X.Count;
            for (int i = 0; i < row_count; ++i)
            {
                if (IsAnomaly(X[i], epsilon))
                {
                    outliers.Add(i);
                }
            }
            return outliers;
        }

        /// <summary>
        /// Method that finds a suitable threshold value for anomaly detection
        /// </summary>
        /// <param name="Xval">data points for the cross validation set</param>
        /// <param name="yval">class label associated with the data points for the cross validation set, 1 for Anomal, 0 for Normal</param>
        /// <param name="max_F1Score">best value for F1 score on the (Xval, yval)</param>
        /// <returns>The best value for threshold which yields the highest F1Score for the evaluation on the cross validation set</returns>
        public double SelectThreshold(List<T> Xval, bool[] yval, out double max_F1Score)
        {
            int row_count = Xval.Count;

            double[] pval = new double[row_count];
            for (int i = 0; i < row_count; ++i)
            {
                T rec = Xval[i];
                double pval_entry = CalcProbability(rec);
                pval[i] = pval_entry;
            }

            return SelectThreshold(pval, yval, out max_F1Score);
        }


        public double SelectThreshold(double[] pval, bool[] yval, out double max_F1Score)
        {
            int row_count = pval.Length;

            double min_pval=pval.Min();
            double max_pval = pval.Max();

            double pval_interval = (max_pval - min_pval) / 1000;

            double precision, recall, F1Score;
            max_F1Score = double.MinValue;
            double best_epsilon = min_pval;
            for (double epsilon = min_pval; epsilon < max_pval; epsilon += pval_interval)
            {
                int true_positive = 0;
                int false_positive = 0;
                int false_negative = 0;

                for (int i = 0; i < row_count; ++i)
                {
                    bool is_anomaly = pval[i] < epsilon;

                    if (is_anomaly)
                    {
                        if (yval[i]) true_positive++;
                        else false_positive++;
                    }
                    else
                    {
                        if (yval[i]) false_negative++;
                    }
                }

                precision = (double)true_positive / (true_positive + false_positive);
                recall = (double)true_positive / (true_positive + false_negative);
                F1Score = precision * recall * 2 / (precision + recall);
                if (F1Score > max_F1Score)
                {
                    max_F1Score = F1Score;
                    best_epsilon = epsilon;
                }
            }

            return best_epsilon;
        }

        public void GetEvaluationMetrics(List<T> Xval, bool[] yval, double epsilon, out double precision, out double recall, out double F1Score)
        {
            int true_positive = 0;
            int false_positive = 0;
            int false_negative = 0;

            int row_count = Xval.Count;
            for (int i = 0; i < row_count; ++i)
            {
                bool predicted = IsAnomaly(Xval[i], epsilon);

                if (predicted)
                {
                    if (yval[i]) true_positive++;
                    else false_positive++;
                }
                else
                {
                    if (yval[i]) false_negative++;
                }
            }

            precision = (double)true_positive / (true_positive + false_positive);
            recall = (double)true_positive / (true_positive + false_negative);
            F1Score = precision*recall*2/(precision + recall);
        }
    }
}
