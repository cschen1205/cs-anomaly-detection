namespace Lang
{
    using System.Collections.Generic;
    using System;
    public class FeatureVector : MLDataPoint
    {
        protected double[] m_data;
        protected double[] m_lower_bound;
        protected double[] m_upper_bound;
        protected object mTag;
        protected Dictionary<string, int> mIntAttributes = new Dictionary<string, int>();

        public object Tag
        {
            get { return mTag; }
            set { mTag = value; }
        }

        public int GetIntAttrValue(string name, int default_value = -1)
        {
            if (mIntAttributes.ContainsKey(name))
            {
                return mIntAttributes[name];
            }
            return default_value;
        }

        public void SetIntAttrValue(string name, int val)
        {
            mIntAttributes[name] = val;
        }

        public FeatureVector(int data)
        {
            m_data = new double[1];
            m_lower_bound = new double[1];
            m_upper_bound = new double[1];
            m_data[0] = data;
            m_lower_bound[0] = double.NegativeInfinity;
            m_upper_bound[0] = double.PositiveInfinity;
        }

        public FeatureVector(double data)
        {
            m_data = new double[1];
            m_lower_bound = new double[1];
            m_upper_bound = new double[1];
            m_data[0] = data;
            m_lower_bound[0] = double.NegativeInfinity;
            m_upper_bound[0] = double.PositiveInfinity;
        }

        public FeatureVector(int[] data, bool create_lower_bound = true, bool create_upper_bound = true)
        {
            m_data = new double[data.Length];
            if (create_lower_bound)
            {
                m_lower_bound = new double[data.Length];
            }
            if (create_upper_bound)
            {
                m_upper_bound = new double[data.Length];
            }

            for (int i = 0; i < data.Length; ++i)
            {
                m_data[i] = data[i];
                if (create_lower_bound)
                {
                    m_lower_bound[i] = double.NegativeInfinity;
                }
                if (create_upper_bound)
                {
                    m_upper_bound[i] = double.PositiveInfinity;
                }
            }
        }

        public FeatureVector(double[] data, bool create_lower_bound = true, bool create_upper_bound = true)
        {
            m_data = new double[data.Length];

            if (create_lower_bound)
            {
                m_lower_bound = new double[data.Length];
            }
            if (create_upper_bound)
            {
                m_upper_bound = new double[data.Length];
            }

            for (int i = 0; i < data.Length; ++i)
            {
                m_data[i] = data[i];
                if (create_lower_bound)
                {
                    m_lower_bound[i] = double.NegativeInfinity;
                }
                if (create_upper_bound)
                {
                    m_upper_bound[i] = double.PositiveInfinity;
                }
            }
        }

        public FeatureVector(int[] data, int[] lower_bound, int[] upper_bound)
        {
            m_data = new double[data.Length];
            if (lower_bound != null)
            {
                m_lower_bound = new double[data.Length];
            }
            if (upper_bound != null)
            {
                m_upper_bound = new double[data.Length];
            }

            for (int i = 0; i < data.Length; ++i)
            {
                m_data[i] = data[i];
                if (lower_bound != null)
                {
                    m_lower_bound[i] = lower_bound[i];
                }
                if (upper_bound != null)
                {
                    m_upper_bound[i] = upper_bound[i];
                }
            }

        }

        public FeatureVector(int data, int lower_bound, int upper_bound)
        {
            m_data = new double[1];
            m_lower_bound = new double[1];
            m_upper_bound = new double[1];

            m_data[0] = data;
            m_lower_bound[0] = lower_bound;
            m_upper_bound[0] = upper_bound;
        }

        public FeatureVector(double data, double lower_bound, double upper_bound)
        {
            m_data = new double[1];
            m_lower_bound = new double[1];
            m_upper_bound = new double[1];

            m_data[0] = data;
            m_lower_bound[0] = lower_bound;
            m_upper_bound[0] = upper_bound;
        }

        public FeatureVector(double[] data, double[] lower_bound, double[] upper_bound)
        {
            m_data = new double[data.Length];
            if (lower_bound != null)
            {
                m_lower_bound = new double[data.Length];
            }
            if (upper_bound != null)
            {
                m_upper_bound = new double[data.Length];
            }

            for (int i = 0; i < data.Length; ++i)
            {
                m_data[i] = data[i];
                if (lower_bound != null)
                {
                    m_lower_bound[i] = lower_bound[i];
                }
                if (upper_bound != null)
                {
                    m_upper_bound[i] = upper_bound[i];
                }
            }
        }



        public override double this[int index]
        {
            get
            {
                return m_data[index];
            }
            set
            {
                m_data[index] = value;
            }
        }

        public override int Dimension
        {
            get
            {
                return m_data.Length;
            }
        }

        public override List<double> getLowerBounds()
        {
            List<double> lower_bounds = new List<double>();
            if (m_lower_bound != null)
            {
                for (int i = 0; i < m_lower_bound.Length; i++)
                {
                    lower_bounds.Add(m_lower_bound[i]);
                }
            }

            return lower_bounds;
        }

        public override List<double> getUpperBounds()
        {
            List<double> upper_bounds = new List<double>();
            if (m_upper_bound != null)
            {
                for (int i = 0; i < m_upper_bound.Length; i++)
                {
                    upper_bounds.Add(m_upper_bound[i]);
                }
            }

            return upper_bounds;
        }

        public override MLDataPoint Clone()
        {
            FeatureVector pt = new FeatureVector(m_data, m_lower_bound, m_upper_bound);
            pt.Tag = mTag;
            return pt;
        }
    }
}