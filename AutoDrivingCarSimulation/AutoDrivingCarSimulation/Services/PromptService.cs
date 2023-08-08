namespace AutoDrivingCarSimulation.Services
{
    public class PromptService : IPromptService
    {
        public async Task<string> AskInput(string message)
        {
            Console.WriteLine(message);
            return await Task.FromResult(Console.ReadLine());
        }
    }

    public interface IPromptService
    {
        Task<string> AskInput(string message);
    }
}
