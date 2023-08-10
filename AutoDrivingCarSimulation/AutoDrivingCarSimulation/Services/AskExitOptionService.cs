using AutoDrivingCarSimulation.Data;
using AutoDrivingCarSimulation.FormatChecker;
using static AutoDrivingCarSimulation.AppEnum;

namespace AutoDrivingCarSimulation.Services
{
    public class AskExitOptionService : ServiceBase, IAskOutput<ExitOptionData>
    {
        private readonly ExitOptionFormatChecker exitOptionFormatChecker;
        public AskExitOptionService(IPromptService promptService,
            ExitOptionFormatChecker exitOptionFormatChecker) : base(promptService) 
        { 
            this.exitOptionFormatChecker = exitOptionFormatChecker;
        }
        public async Task<ExitOptionData> Ask()
        {
            var validFormat = true;
            var inputOption = string.Empty;
            do
            {
                if (!validFormat)
                {
                    await promptService.ShowWarning(AppConst.PromptText.InvalidOption, true, true);
                }
                inputOption = await promptService.AskInput(AppConst.PromptText.AskExitOption, false, true);
                validFormat = await exitOptionFormatChecker.IsMatch(inputOption.Trim());

            } while (!validFormat);

            await promptService.Clear();
            Enum.TryParse(inputOption.Trim(), out ExitOption option);
            return new ExitOptionData()
            {
                option = option
            };
        }
    }
}
