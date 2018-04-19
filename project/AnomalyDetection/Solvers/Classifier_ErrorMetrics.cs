using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solvers;

namespace Lang
{
    public class Classifier_ErrorMetrics<T>
        where T : CDataRecord
    {
        private double mPrecision;
        private double mRecall;
        private double mF1score;

        public double Precision
        {
            get { return mPrecision; }
        }

        public double Recall
        {
            get { return mRecall; }
        }

        public double F1Score
        {
            get { return mF1score; }
        }

        public Classifier_ErrorMetrics(Classifier<T, double> classifier, List<T> data_set, List<string> class_field_labels)
        {
            int sample_count = data_set.Count;
            int true_positive_count = 0;
            int false_positive_count = 0;
            int false_negative_count = 0;
            int true_negative_count = 0;
            for (int i = 0; i < sample_count; ++i)
            {
                T rec = data_set[i] as T;
                string actual_label = rec.Label;
                string predicted_label = classifier.Predict(rec);

                foreach (string class_label in class_field_labels)
                {
                    int true_positive = (actual_label == class_label) && (predicted_label == class_label) ? 1 : 0;
                    int true_negative = (actual_label != class_label) && (predicted_label != class_label) ? 1 : 0;
                    int false_positive = (actual_label != class_label) && (predicted_label == class_label) ? 1 : 0;
                    int false_negative = (actual_label == class_label) && (predicted_label != class_label) ? 1 : 0;
                    true_positive_count += true_positive;
                    true_negative_count += true_negative;
                    false_positive_count += false_positive;
                    false_negative_count += false_negative;
                }
            }

            //precision: Of all the samples where we predicted y=1,what fraction actually has y=1?
            mPrecision = (double)true_positive_count / (true_positive_count + false_positive_count);

            //recall: Of all samples that actually have y=1, what fraction did we correctly detect as having y=1?
            mRecall = (double)true_positive_count / (true_positive_count + false_negative_count);

            mF1score = 2 * (mPrecision * mRecall) / (mPrecision + mRecall);
        }

    }
}
