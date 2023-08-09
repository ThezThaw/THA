using AutoDrivingCarSimulation.Data;
using AutoDrivingCarSimulation.FormatChecker;

namespace AutoDrivingCarSimulation.Services
{
    public class AskCommandService : ServiceBase, IAskCommandService
    {
        private readonly CommandInputChecker commandInputFormatChecker;
        public AskCommandService(IPromptService promptService, CommandInputChecker commandInputFormatChecker) : base(promptService)
        {
            this.commandInputFormatChecker = commandInputFormatChecker;
        }
        public async Task Ask(CarData askForThisCar)
        {
            var validFormat = true;
            var input = string.Empty;
            do
            {
                if (!validFormat)
                {
                    await promptService.ShowWarning(AppConst.PromptText.InvalidCommand, true, true, true);
                }
                input = await promptService.AskInput(AppConst.PromptText.AskCommand, false, true, param: askForThisCar.name);
                validFormat = await commandInputFormatChecker.IsMatch(input.Trim());

            } while (!validFormat);
            askForThisCar.command = new CommandData() { command = input };
        }
    }

    public interface IAskCommandService : IAskInput<CarData> { }
}
