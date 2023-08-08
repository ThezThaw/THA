using AutoDrivingCarSimulation.Data;
using AutoDrivingCarSimulation.FormatChecker;

namespace AutoDrivingCarSimulation.Services
{
    public class SimulationFieldService : ISimulationFieldService
    {
        private readonly IPromptService promptService;
        private readonly SimulationFieldInputChecker simulationFieldInputChecker;
        private readonly IDataContext<SimulationFieldData> dataContext;
        public SimulationFieldService(
            IDataContext<SimulationFieldData> dataContext,
            IPromptService promptService,
            SimulationFieldInputChecker simulationFieldInputChecker)
        {
            this.dataContext = dataContext;
            this.promptService = promptService;
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
                    Console.WriteLine("Invalid input!");
                }
                else if (!validSimulationField)
                {
                    Console.WriteLine("Simulation field too small!");
                }

                input = await promptService.AskInput("Please enter the width and height of the simulation field in x y format:");
                validFormat = await simulationFieldInputChecker.IsMatch(input);

                if (validFormat)
                {
                    var xy = input.Split(" ");
                    field = new SimulationFieldData()
                    {
                        width = Convert.ToInt32(xy[0]),
                        height = Convert.ToInt32(xy[1]),
                    };

                    validSimulationField = field.width > 0 && field.height > 0;
                }
            } while (!validFormat || !validSimulationField);

            
            dataContext.SetData(field);
            Console.WriteLine("You have created a field of {0} x {1}.", field.width, field.height);
        }
    }

    public interface ISimulationFieldService
    {
        Task DefineField();
    }
}
