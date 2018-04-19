using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lang;

namespace Solvers
{
    public abstract class Classifier<T, U> : IClassifier<T>
        where T : DataRecord<U>
    {
        public double GetClassificationError(List<T> data_set)
        {
            int m = 0;
            double total_error = 0;
            foreach (T rec in data_set)
            {
                total_error += (rec.Label == Predict(rec) ? 0 : 1);
                m++;
            }
            return total_error / m;
        }

        public virtual void Train(List<T> data_set)
        {
            Train(data_set as IEnumerable<T>);
        }

        public virtual void Train(IEnumerable<T> data_store)
        {
            throw new NotImplementedException();
        }

        public virtual void Predict(List<T> data_set)
        {
            foreach (T rec in data_set)
            {
                rec.PredictedLabel = Predict(rec);
            }
        }

        public virtual double ComputeCost(List<T> data_set)
        {
            double cost = 0;
            int record_count = 0;
            int error_count = 0;
            foreach (T rec in data_set)
            {
                error_count += (rec.Label != Predict(rec) ? 1 : 0);
                record_count++;
            }
            cost = (double)error_count / record_count;
            return cost;
        }

        public virtual string Predict(T rec)
        {
            throw new NotImplementedException();
        }
    }
}
