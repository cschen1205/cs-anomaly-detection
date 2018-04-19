using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lang;

namespace Solvers
{
    public class Classifier_LearningCurve<T, U>
        where T : DataRecord<U>
    {
        protected int[] mTrainingSampleCounts;
        protected double[] mX_cost;
        protected double[] mXval_cost;

        public int[] TrainingSampleCount
        {
            get { return mTrainingSampleCounts; }
        }

        public double[] X_cost
        {
            get { return mX_cost; }
        }

        public double[] Xval_cost
        {
            get { return mXval_cost; }
        }

        public Classifier_LearningCurve(Classifier<T, U> classifier, List<T> X, List<T> Xval)
        {
            int X_length = X.Count;
            mX_cost = new double[X_length - 1];
            mXval_cost = new double[X_length - 1];
            mTrainingSampleCounts = new int[X_length - 1];

            for (int i = 1; i < X_length; ++i)
            {
                mTrainingSampleCounts[i - 1] = i;

                classifier.Train(X);
                mX_cost[i - 1] = classifier.ComputeCost(X);
                mXval_cost[i - 1] = classifier.ComputeCost(Xval);
            }
        }
    }
}
