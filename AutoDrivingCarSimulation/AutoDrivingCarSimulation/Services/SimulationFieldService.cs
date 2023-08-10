using AutoDrivingCarSimulation.Data;
using AutoDrivingCarSimulation.FormatChecker;

namespace AutoDrivingCarSimulation.Services
{
    public class SimulationFieldService : ServiceBase, ISimulationFieldService
    {
        private readonly SimulationFieldInputChecker simulationFieldInputChecker;
        private readonly IDataContext<SimulationFieldData> dataContext;
        public SimulationFieldService(
            IDataContext<SimulationFieldData> dataContext,
            IPromptService promptService,
            SimulationFieldInputChecker simulationFieldInputChecker): base(promptService)
        {
            this.dataContext = dataContext;
            this.simulationFieldInputChecker = simulationFieldInputChecker;
        }

        public async Task DefineField()
        {
            var validFormat = true;
            var validSimulationField = true;
            var input = string.Empty;
            var field = new SimulationFieldData();
            do
            {
                if (!validFormat)
                {
                    await promptService.ShowWarning(AppConst.PromptText.InvalidSimulationFieldFormat, true, true);
                }
                else if (!validSimulationField)
                {
                    await promptService.ShowWarning(AppConst.PromptText.InvalidSimulationFieldSize, true, true);
                }

                input = await promptService.AskInput(AppConst.PromptText.AskSimulationFieldInput, false, true);
                validFormat = await simulationFieldInputChecker.IsMatch(input);

                if (validFormat)
                {
                    var xy = input.Split(AppConst.InputDelimiter);
                    field = new SimulationFieldData()
                    {
                        width = Convert.ToInt32(xy[0]),
                        height = Convert.ToInt32(xy[1]),
                    };

                    validSimulationField = field.width > 0 && field.height > 0;
                }
            } while (!validFormat || !validSimulationField);
            
            await dataContext.SetData(field);
            await promptService.ShowMessage(AppConst.PromptText.SimulationFieldInfo, true, true, field.width, field.height);
        }
    }

    public interface ISimulationFieldService
    {
        Task DefineField();
    }
}
