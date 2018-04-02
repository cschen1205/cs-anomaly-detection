using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimuKit.ML.AnomalyDetection.UT
{
    class Program
    {
        static void Main(string[] args)
        {
            //GaussianDistributionAD_UT.Test_LoadDataSet();
            //GaussianDistributionAD_UT.Test_ComputeGaussianDistribution();

            MultiVariateGaussianDistributionAD_UT.Test_CalcProbability_X(1);
            //MultiVariateGaussianDistributionAD_UT.Test_CalcProbability_Xval(1);
            //MultiVariateGaussianDistributionAD_UT.Test_SelectThreshold(1);
            //MultiVariateGaussianDistributionAD_UT.Test_FindOutliers(1);

            //MultiVariateGaussianDistributionAD_UT.Test_CalcProbability_X(2);
            //MultiVariateGaussianDistributionAD_UT.Test_CalcProbability_Xval(2);
            //MultiVariateGaussianDistributionAD_UT.Test_SelectThreshold(2);
            //MultiVariateGaussianDistributionAD_UT.Test_FindOutliers(2);
        }
    }
}
