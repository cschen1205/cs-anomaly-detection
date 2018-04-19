using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lang;

namespace Lang
{
    public class CDataRecord : DataRecord<double>
    {
        protected double[] mData = null;

        protected int mFeatureNum = 0;

        public double[] Data
        {
            get { return mData; }
        }

        public override int Dimension
        {
            get { return mFeatureNum + 1; }
        }

        public int FeatureCount
        {
            get { return mFeatureNum; }
        }


        public CDataRecord(int num_features)
        {
            mFeatureNum = num_features;
            mData = new double[num_features + 1];
            mData[0] = 1.0;
        }

        public override double this[int index]
        {
            get
            {
                return mData[index];
            }
            set
            {
                if (index == 0)
                {
                    throw new ArgumentException("this[index==0] is reserved and not allowed to be modified");
                }
                mData[index] = value;
            }
        }
    }
}
