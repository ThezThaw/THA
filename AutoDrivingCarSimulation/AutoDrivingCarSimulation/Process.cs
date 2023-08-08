using AutoDrivingCarSimulation.Data;
using AutoDrivingCarSimulation.Services;

namespace AutoDrivingCarSimulation
{
    public class Process : IProcess
    {
        private readonly IPromptService promptService;
        private readonly ISimulationFieldService simulationFieldService;
        private readonly IAskOutput<ProcessOptionData> askProcessOptionService;
        public Process(ISimulationFieldService simulationFieldService,
            IAskOutput<ProcessOptionData> askProcessOptionService,
            IPromptService promptService)
        {
            this.simulationFieldService = simulationFieldService;
            this.askProcessOptionService = askProcessOptionService;
            this.promptService = promptService;
        }
        public async Task Start()
        {
            Console.WriteLine("Welcome to Auto Driving Car Simulation!");
            await promptService.ShowMessage(AppConst.PromptText.WelcomeText, true, true);
            //Ask field input
            //Check input format
            await simulationFieldService.DefineField();

            //Ask process option
            //check input
            var processOption = await askProcessOptionService.Ask();
            //Add car OR Run simulation            

            //Show result
            if (processOption.option == AppEnum.ProcessOption.AddCar)
            {
                Console.WriteLine($"You choose Add Car option");
            }
            else if (processOption.option == AppEnum.ProcessOption.RunSimulation)
            {
                Console.WriteLine($"You choose Run Simulation option");
            }

            //Ask exit option
            //Start over OR exit
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
