using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lang;

namespace AnomalyDetection
{
    /// <summary>
    /// Unsupervised anomaly detection algorithm based on Gaussian distribution
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GaussianDistributionAD<T>
        where T : MLDataPoint
    {
        private List<int> mFeatureIndexList = new List<int>();
        private HashSet<int> mFeatureIndexSet = new HashSet<int>();
        private Dictionary<int, double> mGaussian_Mu = new Dictionary<int, double>();
        private Dictionary<int, double> mGaussian_Sigma2 = new Dictionary<int, double>();
        private double mEpsilon = 0.02;

        public Dictionary<int, double> Gaussian_Mu
        {
            get { return mGaussian_Mu; }
        }

        public Dictionary<int, double> Gaussian_Sigma2
        {
            get { return mGaussian_Sigma2; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(int feature_index in mFeatureIndexList)
            {
                sb.AppendFormat("mu[{0}]={1:0.00}\n", feature_index, mGaussian_Mu[feature_index]);
            }

            foreach (int feature_index in mFeatureIndexList)
            {
                sb.AppendFormat("sigma^2[{0}]={1:0.000}\n", feature_index, mGaussian_Sigma2[feature_index]);
            }

            return sb.ToString();
        }

        public double Epsilon
        {
            get { return mEpsilon; }
            set { mEpsilon = value; }
        }

        public void SelectFeature(int feature_index)
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

        public double FindProbability(int feature_index, double x)
        {
            double mu=mGaussian_Mu[feature_index];
            double sigma2=mGaussian_Sigma2[feature_index];
            return System.Math.Exp(-System.Math.Pow(x-mu, 2)/(2*sigma2)) / (System.Math.Sqrt(2*System.Math.PI*sigma2));
        }

        public double CalcProbability(T rec)
        {
            double product = 1;
            foreach (int feature_index in mFeatureIndexList)
            {
                product*=FindProbability(feature_index, rec[feature_index]);
            }
            return product;
        }

        public bool IsAnomaly(T rec)
        {
            double p = CalcProbability(rec);
            return p < mEpsilon;
        }

        public void ComputeGaussianDistribution(List<T> records)
        {
            int sample_count = records.Count;

            if (mFeatureIndexList.Count == 0)
            {
                int dim = records[0].Dimension;
                for (int i = 0; i < dim; ++i)
                {
                    SelectFeature(i);
                }
            }
            
            foreach (int feature_index in mFeatureIndexList)
            {
                double mu = 0;
                for (int i = 0; i < sample_count; ++i)
                {
                    double x = records[i][feature_index];
                    mu += x;
                }
                mu /= sample_count;

                double sqr_sum = 0;
                for (int i = 0; i < sample_count; ++i)
                {
                    double x = records[i][feature_index];
                    sqr_sum += System.Math.Pow(x - mu, 2);
                }

                double sigma2 = sqr_sum / sample_count;

                mGaussian_Mu[feature_index] = mu;
                mGaussian_Sigma2[feature_index] = sigma2;
            }
            
        }
    }
}
