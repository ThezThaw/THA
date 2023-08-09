using AutoDrivingCarSimulation.Data;
using AutoDrivingCarSimulation.FormatChecker;

namespace AutoDrivingCarSimulation.Services
{
    public class AskPositionService : ServiceBase, IAskPositionService
    {
        private readonly PositionInputFormatChecker positionInputFormatChecker;
        private readonly IDataContext<SimulationFieldData> fieldDataContext;
        public AskPositionService(IPromptService promptService, IDataContext<SimulationFieldData> fieldDataContext, PositionInputFormatChecker positionInputFormatChecker) : base(promptService)
        {
            this.positionInputFormatChecker = positionInputFormatChecker;
            this.fieldDataContext = fieldDataContext;
        }
        public async Task Ask(CarData askForThisCar)
        {
            var validFormat = true;
            var validPosition = true;
            var input = string.Empty;
            var p = new PositionData();

            do
            {
                if (!validFormat)
                {
                    await promptService.ShowWarning(AppConst.PromptText.InvalidPosition, true, true);
                }
                else if (!validPosition)
                {
                    await promptService.ShowMessage(AppConst.PromptText.InvalidPositionOutOfField, true, true, fieldDataContext.GetData().width, fieldDataContext.GetData().height);
                }

                input = await promptService.AskInput(AppConst.PromptText.AskInitialPosition, false, true, askForThisCar.name);
                validFormat = await positionInputFormatChecker.IsMatch(input.Trim());

                if (validFormat)
                {
                    var position = input.Trim().Split(AppConst.InputDelimiter);
                    p.x = Convert.ToInt32(position[0]);
                    p.y = Convert.ToInt32(position[1]);
                    Enum.TryParse(position[2], out AppEnum.Direction direction);
                    p.direction = direction;

                    validPosition = p.x <= fieldDataContext.GetData().width && p.y <= fieldDataContext.GetData().height;
                }

            } while (!validFormat || !validPosition);

            askForThisCar.initialPosition = p;
            askForThisCar.currentPosition = p;
            await promptService.Clear();
        }
    }

    public interface IAskPositionService : IAskInput<CarData> { }
}
