namespace AutoDrivingCarSimulation.Services
{
    public interface IAsk<TInput, TOutput> where TInput : class where TOutput : class
    {
        Task<TOutput> Ask(TInput input);
    }

    public interface IAskInput<TInput> where TInput : class
    {
        Task Ask(TInput input);
    }

    public interface IAskOutput<TOutput> where TOutput : class
    {
        Task<TOutput> Ask();
    }
}
