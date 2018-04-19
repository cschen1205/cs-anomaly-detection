namespace Lang
{
    using System.Collections.Generic;
    public class SampleSet
    {
        protected List<MLDataPoint> m_inputs = null;
        protected List<MLDataPoint> m_targets = null;
        protected int m_input_dim;
        protected int m_output_dim;

        public SampleSet(int input_dim, int output_dim)
        {
            m_input_dim = input_dim;
            m_output_dim = output_dim;

            m_inputs = new List<MLDataPoint>();
            m_targets = new List<MLDataPoint>();
        }

        public SampleSet(int input_dim)
        {
            m_input_dim = input_dim;
            m_output_dim = -1;
            m_inputs = new List<MLDataPoint>();
        }

        public int size()
        {
            return m_inputs.Count;
        }

        public void addSample(MLDataPoint input, MLDataPoint target)
        {
            m_inputs.Add(input);
            m_targets.Add(target);
        }

        public MLDataPoint getSample(int index)
        {
            return m_inputs[index];
        }

        public MLDataPoint getSampleTarget(int index)
        {
            return m_targets[index];
        }
    }
}
