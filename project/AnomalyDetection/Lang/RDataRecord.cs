using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lang
{
    public class RDataRecord : DataRecord<double>
    {
        protected double mYValue = 0;
        protected double mPredictedYValue = 0;

        protected double[] mData = null;
        protected int mFeatureNum = 0;

        protected bool[] mIsCategorical = null;

        public override void Copy(DataRecord<double> rhs)
        {
            base.Copy(rhs);

            RDataRecord rhs2 = rhs as RDataRecord;
            mYValue = rhs2.mYValue;
            mPredictedYValue = rhs2.mPredictedYValue;
            mData = (double[])rhs2.mData.Clone();
            mIsCategorical = (bool[])rhs2.mIsCategorical.Clone();
            mFeatureNum = rhs2.mFeatureNum;
        }

        public override DataRecord<double> Clone()
        {
            RDataRecord clone = new RDataRecord(mFeatureNum);
            clone.Copy(this);
            return clone;
        }

        public double[] Features
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

        public RDataRecord(int num_features)
        {
            mFeatureNum = num_features;
            mData = new double[num_features + 1];
            mIsCategorical = new bool[num_features + 1];
            mData[0] = 1.0;
        }

        /// <summary>
        /// feature index starts at 1
        /// </summary>
        /// <param name="index">feature index starts at 1</param>
        /// <returns></returns>
        public bool IsFeatureCategorical(int index)
        {
            return mIsCategorical[index];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">feature index starts at 1</param>
        /// <param name="categorical"></param>
        public void SetFeatureCatogorical(int index, bool categorical)
        {
            mIsCategorical[index] = categorical;
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

        public double YValue
        {
            get { return mYValue; }
            set { mYValue = value; }
        }

        public double PredictedYValue
        {
            get { return mPredictedYValue; }
            set { mPredictedYValue = value; }
        }
    }
}
