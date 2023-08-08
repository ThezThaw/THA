namespace AutoDrivingCarSimulation.Data
{
    public class DataContext<T> : IDataContext<T> where T : class
    {
        public T data { get; set; }
        public DataContext() { }
        public void SetData(T data)
        {
            this.data = data;
        }
        public T GetData() => this.data;
    }
    public interface IDataContext<T> where T : class
    {
        void SetData(T data);
        T GetData();
    }
}
