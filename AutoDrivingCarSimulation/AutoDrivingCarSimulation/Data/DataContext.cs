namespace AutoDrivingCarSimulation.Data
{
    public class DataContext<T> : IDataContext<T> where T : class
    {
        public T data { get; set; }
        public DataContext() { }
        public async Task SetData(T data)
        {
            await Task.Run(() => this.data = data);
        }
        public async Task<T> GetData()
        {
            return await Task.FromResult(this.data);
        }

        public async Task Reset()
        {
            await this.SetData(null);
        }
    }
    public interface IDataContext<T> where T : class
    {
        Task SetData(T data);
        Task Reset();
        Task<T> GetData();
    }
}
