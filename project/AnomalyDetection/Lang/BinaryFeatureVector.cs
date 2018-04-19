namespace Lang
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    public class BinaryFeatureVector : BinaryMLDataPoint
    {
        bool[] m_pattern = null;

        public BinaryFeatureVector(int[] pattern)
        {
            m_pattern = new bool[pattern.Length];
            for (int i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] == 0)
                {
                    m_pattern[i] = false;
                }
                else
                {
                    m_pattern[i] = true;
                }
            }
        }

        public bool get(int index)
        {
            return m_pattern[index];
        }

        public void set(int index, bool value)
        {
            m_pattern[index] = value;
        }

        public int size()
        {
            return m_pattern.Length;
        }

        public String toString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m_pattern.Length; i++)
            {
                if (m_pattern[i])
                {
                    sb.Append("1 ");
                }
                else
                {
                    sb.Append("0 ");
                }
            }

            return sb.ToString();
        }
    }
}