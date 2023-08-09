using AutoDrivingCarSimulation.Data;
using AutoDrivingCarSimulation.Services;
using System.Text;

namespace AutoDrivingCarSimulation
{
    public class Process : IProcess
    {
        private readonly IPromptService promptService;
        private readonly ISimulationFieldService simulationFieldService;
        private readonly IAskOutput<ProcessOptionData> askProcessOptionService;
        private readonly ICarService carService;
        private readonly IAskPositionService askPositionService;
        private readonly IAskCommandService askCommandService;

        private readonly IDataContext<List<CarData>> carDataContext;
        public Process(ISimulationFieldService simulationFieldService,
            IAskOutput<ProcessOptionData> askProcessOptionService,
            IPromptService promptService,
            ICarService carService,
            IAskPositionService askPositionService,
            IAskCommandService askCommandService,
            IDataContext<List<CarData>> carDataContext)
        {
            this.simulationFieldService = simulationFieldService;
            this.askProcessOptionService = askProcessOptionService;
            this.promptService = promptService;
            this.carService = carService;
            this.askPositionService = askPositionService;
            this.askCommandService = askCommandService;
            this.carDataContext = carDataContext;
        }
        public async Task Start()
        {
            await promptService.ShowMessage(AppConst.PromptText.WelcomeText, true, true);
            //Ask field input
            //Check input format
            await simulationFieldService.DefineField();
            await AskProcessOption();

            //Ask exit option
            //Start over OR exit
        }
        private async Task AskProcessOption()
        {
            //Ask process option
            //check input
            var processOption = await askProcessOptionService.Ask();
            //Add car OR Run simulation            

            if (processOption.option == AppEnum.ProcessOption.AddCar)
            {
                var currentAddedCar = await carService.Add();
                await askPositionService.Ask(currentAddedCar);
                await askCommandService.Ask(currentAddedCar);
                await ShowListOfCar();
                await AskProcessOption();
            }
            else if (processOption.option == AppEnum.ProcessOption.RunSimulation)
            {
                Console.WriteLine($"You choose Run Simulation option");
            }
        }
        private async Task ShowListOfCar()
        {
            await promptService.ShowMessage(AppConst.PromptText.ListOfCarText, true, false);
            var sb = new StringBuilder();
            carDataContext.GetData().ForEach(async car =>
            {
                sb.AppendLine($"- {car.name}, ({car.initialPosition.x},{car.initialPosition.y}) {car.initialPosition.direction}, {car.command.command}");
            });
            await promptService.ShowMessage(sb.ToString(), false, true);
        }
        public Task Stop()
        {
            throw new NotImplementedException();
        }
    }

    public interface IProcess
    {
        Task Start();
        Task Stop();
    }
}
