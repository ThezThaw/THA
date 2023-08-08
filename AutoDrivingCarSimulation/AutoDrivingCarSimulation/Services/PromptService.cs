namespace AutoDrivingCarSimulation.Services
{
    public class PromptService : IPromptService
    {
        public async Task<string> AskInput(string message, bool removeEalierText, bool newLine, params object[] param)
        {
            await ShowMessage(message, removeEalierText, newLine, param);
            return await Task.FromResult(Console.ReadLine());
        }
        public async Task ShowWarning(string message, bool removeEalierText, bool newLine, params object[] param)
        {
            if (removeEalierText) await Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            message += newLine ? Environment.NewLine : string.Empty;
            Console.WriteLine(message, param);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public async Task ShowMessage(string message, bool removeEalierText, bool newLine, params object[] param)
        {
            if (removeEalierText) await Clear();
            message += newLine ? Environment.NewLine : string.Empty;
            Console.WriteLine(message, param);
        }
        public async Task Clear()
        {
            await Task.Run(() => Console.Clear());
        }
    }

    public interface IPromptService
    {
        Task<string> AskInput(string message, bool removeEalierText, bool newLine, params object[] param);
        Task ShowWarning(string message, bool removeEalierText, bool newLine, params object[] param);
        Task ShowMessage(string message, bool removeEalierText, bool newLine, params object[] param);
        Task Clear();
    }
}
