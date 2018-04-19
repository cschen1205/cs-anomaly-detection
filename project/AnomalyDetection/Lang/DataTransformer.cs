using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lang
{
    public class DataTransformer<T>
        where T : DataRecord<double>
    {
        public void FindFeaturesBounds(List<T> data_set, out double[] lower_bounds, out double[] upper_bounds)
        {
            int sample_count = data_set.Count;
            int dimension = data_set[0].Dimension;
            lower_bounds = new double[dimension];
            upper_bounds = new double[dimension];
            for (int d = 0; d < dimension; ++d)
            {
                double lower_bound = double.MaxValue;
                double upper_bound = double.MinValue;
                for (int i = 0; i < sample_count; ++i)
                {
                    double value = data_set[i][d];
                    if (lower_bound > value) lower_bound = value;
                    if (upper_bound < value) upper_bound = value;
                }
                lower_bounds[d] = lower_bound;
                upper_bounds[d] = upper_bound;
            }
        }

        public void FindMeanAndStd(List<T> data_set, out double[] means, out double[] std_deviations)
        {
            int sample_count = data_set.Count;
            int dimension = data_set[0].Dimension;

            means = new double[dimension];
            std_deviations = new double[dimension];
            for (int d = 0; d < dimension; ++d)
            {
                for (int i = 0; i < sample_count; ++i)
                {
                    double value = data_set[i][d];
                    means[d] += value;
                }
                means[d] /= sample_count;
            }

            for (int d = 0; d < dimension; ++d)
            {
                for (int i = 0; i < sample_count; ++i)
                {
                    double value = data_set[i][d];
                    std_deviations[d] += Math.Pow(value - means[d], 2);
                }
                std_deviations[d] /= sample_count;
                std_deviations[d] = Math.Sqrt(std_deviations[d]);
            }
        }

        public void DoFeaturesScaling(List<T> data_set)
        {
            double[] lower_bounds, upper_bounds;
            FindFeaturesBounds(data_set, out lower_bounds, out upper_bounds);
            DoFeaturesScaling(data_set, upper_bounds, lower_bounds);
        }

        public void DoFeaturesScaling(T rec, double[] upper_bounds, double[] lower_bounds)
        {
            int dimension = rec.Dimension;

            for (int d = 0; d < dimension; ++d)
            {
                double lower_bound = lower_bounds[d];
                double upper_bound = upper_bounds[d];
                double range = upper_bound - lower_bound;
                if (range != 0)
                {
                    rec[d] = (rec[d] - lower_bound) / range;
                }
            }
        }

        public void DoFeaturesScaling(List<T> data_set, double[] upper_bounds, double[] lower_bounds)
        {
            int sample_count = data_set.Count;
            int dimension = data_set[0].Dimension;

            for (int d = 0; d < dimension; ++d)
            {
                double lower_bound = lower_bounds[d];
                double upper_bound = upper_bounds[d];
                double range = upper_bound - lower_bound;
                for (int i = 0; i < sample_count; ++i)
                {
                    T rec = data_set[i];
                    if (range != 0)
                    {
                        rec[d] = (rec[d] - lower_bound) / range;
                    }
                }
            }
        }

        public void DoMeanNormalization(List<T> data_set)
        {
            double[] means, std_deviations;
            FindMeanAndStd(data_set, out means, out std_deviations);
            DoMeanNormalization(data_set, means, std_deviations);
        }

        public void DoMeanNormalization(List<T> data_set, double[] means, double[] std_deviations)
        {
            int sample_count = data_set.Count;
            int dimension = data_set[0].Dimension;

            for (int d = 0; d < dimension; ++d)
            {
                double mean = means[d];
                double std = std_deviations[d];
                for (int i = 0; i < sample_count; ++i)
                {
                    T rec = data_set[i];
                    if (std != 0)
                    {
                        rec[d] = (rec[d] - mean) / std;
                    }
                }
            }
        }

        public void DoMeanNormalizationWithFeatureScaling(List<T> data_set)
        {
            double[] means, std_deviations;
            double[] upper_bounds, lower_bounds;
            FindMeanAndStd(data_set, out means, out std_deviations);
            FindFeaturesBounds(data_set, out lower_bounds, out upper_bounds);
            DoMeanNormalizationWithFeaturesScaling(data_set, means, lower_bounds, upper_bounds);
        }

        public void DoMeanNormalizationWithFeaturesScaling(List<T> data_set, double[] means, double[] lower_bounds, double[] upper_bounds)
        {
            int sample_count = data_set.Count;
            int dimension = data_set[0].Dimension;

            for (int d = 0; d < dimension; ++d)
            {
                double mean = means[d];
                double upper_bound = upper_bounds[d];
                double lower_bound = lower_bounds[d];
                double range = upper_bound - lower_bound;
                for (int i = 0; i < sample_count; ++i)
                {
                    T rec = data_set[i];
                    if (range != 0)
                    {
                        rec[d] = (rec[d] - mean) / range;
                    }
                }
            }
        }


    }
}
