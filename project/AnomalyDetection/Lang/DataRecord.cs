using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lang
{
    public abstract class DataRecord<T>
    {
        protected DataSetTypes mDataSetType = DataSetTypes.Training;
        protected string mLabel = "";
        protected string mPredictedLabel = "";
        protected object mTag;

        public virtual void Copy(DataRecord<T> rhs)
        {
            mDataSetType = rhs.mDataSetType;
            mLabel = rhs.mLabel;
            mPredictedLabel = rhs.mPredictedLabel;
            mTag = rhs.mTag;
        }

        public virtual DataRecord<T> Clone()
        {
            throw new NotImplementedException();
        }

        public object Tag
        {
            get { return mTag; }
            set { mTag = value; }
        }

        public DataSetTypes DataSetType
        {
            get { return mDataSetType; }
            set { mDataSetType = value; }
        }

        public abstract T this[int index]
        {
            get;
            set;
        }

        public abstract int Dimension
        {
            get;
        }


        public string Label
        {
            get { return mLabel; }
            set { mLabel = value; }
        }

        public string PredictedLabel
        {
            get { return mPredictedLabel; }
            set { mPredictedLabel = value; }
        }

    }
}
