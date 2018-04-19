namespace Lang
{
    public interface BinaryMLDataPoint
    {
        int size();
        bool get(int index);
        void set(int index, bool value);
        //public double avgValue();
    }
}
