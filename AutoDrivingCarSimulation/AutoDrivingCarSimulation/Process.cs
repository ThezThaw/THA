using AutoDrivingCarSimulation.Services;

namespace AutoDrivingCarSimulation
{
    public class Process : IProcess
    {
        private readonly ISimulationFieldService simulationFieldService;
        public Process(ISimulationFieldService simulationFieldService)
        {
            this.simulationFieldService = simulationFieldService;
        }
        public async Task Start()
        {
            Console.WriteLine("Welcome to Auto Driving Car Simulation!");
            //Ask field input
            //Check input format
            await simulationFieldService.DefineField();

            //Ask process option
            //check input

            //Add car OR Run simulation

            //Show result

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
