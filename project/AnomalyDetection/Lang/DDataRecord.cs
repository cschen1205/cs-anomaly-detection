using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lang
{
    public class DDataRecord : DataRecord<string>
    {
        protected Dictionary<string, string> mData = new Dictionary<string, string>();

        public string[] FindFeatures()
        {
            return mData.Keys.ToArray();
        }

        public override int Dimension
        {
            get { return mData.Count; }
        }

        public int FeatureCount
        {
            get { return mData.Count; }
        }

        public DDataRecord()
        {

        }

        public override string this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool ContainsFeature(string feature_name)
        {
            return mData.ContainsKey(feature_name);
        }

        public string this[string key]
        {
            get
            {
                if (mData.ContainsKey(key))
                {
                    return mData[key];
                }
                else
                {
                    throw new ArgumentNullException("failed to find key");
                }
            }
            set
            {
                mData[key] = value;
            }
        }

        public virtual DDataRecord Clone()
        {
            DDataRecord clone = new DDataRecord();
            foreach (string feature_name in this.mData.Keys)
            {
                clone.mData[feature_name] = this.mData[feature_name];
            }

            clone.mLabel = mLabel;
            clone.mPredictedLabel = mPredictedLabel;

            return clone;
        }
    }
}
