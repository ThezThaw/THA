using AutoDrivingCarSimulation.Data;
using AutoDrivingCarSimulation.FormatChecker;
using static AutoDrivingCarSimulation.AppEnum;

namespace AutoDrivingCarSimulation.Services
{
    public class AskProcessOptionService : ServiceBase, IAskOutput<ProcessOptionData>
    {
        private readonly ProcessOptionChecker processOptionFormatChecker;
        public AskProcessOptionService(IPromptService promptService,
            ProcessOptionChecker processOptionFormatChecker) : base(promptService) 
        { 
            this.processOptionFormatChecker = processOptionFormatChecker;
        }
        public async Task<ProcessOptionData> Ask()
        {
            var validFormat = true;
            var inputOption = string.Empty;
            do
            {
                if (!validFormat)
                {
                    await promptService.ShowWarning(AppConst.PromptText.InvalidOption, true, true);
                }
                inputOption = await promptService.AskInput(AppConst.PromptText.AskProcessOption, false, true);
                validFormat = await processOptionFormatChecker.IsMatch(inputOption);

            } while (!validFormat);

            await promptService.Clear();
            Enum.TryParse(inputOption, out ProcessOption option);
            return new ProcessOptionData()
            {
                option = option
            };
        }
    }
}
