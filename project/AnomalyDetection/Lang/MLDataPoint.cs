using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lang
{
    public abstract class MLDataPoint
    {
        public abstract int Dimension
        {
            get;
        }

        public abstract List<double> getLowerBounds();
        public abstract List<double> getUpperBounds();
        public abstract MLDataPoint Clone();

        protected string mLabel = "";
        public string Label
        {
            get { return mLabel; }
            set { mLabel = value; }
        }

        public abstract double this[int i]
        {
            get;
            set;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            int dimension_count = Dimension;
            for (int i = 0; i < dimension_count; ++i)
            {
                if (i == 0)
                {
                    sb.AppendFormat("{0}", this[i]);
                }
                else
                {
                    sb.AppendFormat(" {0}", this[i]);
                }
            }
            sb.Append("]");
            return sb.ToString();
        }

        public double FindDistance2(MLDataPoint pt)
        {
            double distance = 0;
            int dimension = this.Dimension;
            for (int i = 0; i < dimension; ++i)
            {
                double d = (this[i] - pt[i]);
                distance += (d * d);
            }
            return System.Math.Sqrt(distance);
        }

    }
}
