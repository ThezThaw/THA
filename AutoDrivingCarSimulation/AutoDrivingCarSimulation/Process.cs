﻿using AutoDrivingCarSimulation.Data;
using AutoDrivingCarSimulation.Services;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace AutoDrivingCarSimulation
{
    public class Process : IProcess
    {
        private bool exceptionHappened = false;
        private readonly IHostApplicationLifetime hostApplicationLifetime;

        private readonly IPromptService promptService;
        private readonly ISimulationFieldService simulationFieldService;
        private readonly IAskOutput<ProcessOptionData> askProcessOptionService;
        private readonly ICarService carService;
        private readonly IAskPositionService askPositionService;
        private readonly IAskCommandService askCommandService;
        private readonly ISimulationService simulationService;
        private readonly IAskOutput<ExitOptionData> askExitOptionService;

        private readonly IDataContext<List<CarData>> carDataContext;
        public Process(ISimulationFieldService simulationFieldService,
            IAskOutput<ProcessOptionData> askProcessOptionService,
            IPromptService promptService,
            ICarService carService,
            IAskPositionService askPositionService,
            IAskCommandService askCommandService,
            IDataContext<List<CarData>> carDataContext,
            ISimulationService simulationService,
            IAskOutput<ExitOptionData> askExitOptionService,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            this.simulationFieldService = simulationFieldService;
            this.askProcessOptionService = askProcessOptionService;
            this.promptService = promptService;
            this.carService = carService;
            this.askPositionService = askPositionService;
            this.askCommandService = askCommandService;
            this.carDataContext = carDataContext;
            this.simulationService = simulationService;
            this.askExitOptionService = askExitOptionService;
            this.hostApplicationLifetime = hostApplicationLifetime;
        }
        public async Task Start()
        {
            try
            {
                var removeEalierPromptedText = !exceptionHappened;
                await promptService.ShowMessage(AppConst.PromptText.WelcomeText, removeEalierPromptedText, true);
                exceptionHappened = false;//reset flag
                //Ask field input
                //Check input format
                await simulationFieldService.DefineField();
                await AskProcessOption();

                //Ask exit option
                //Start over OR exit
                var exitOption = await askExitOptionService.Ask();
                if (exitOption.option == AppEnum.ExitOption.StartOver)
                {
                    await carDataContext.Reset();
                    await Start();
                }
                else if (exitOption.option == AppEnum.ExitOption.Exit)
                {
                    await promptService.ShowMessage(AppConst.PromptText.GoodbyeText, true, true);
                    await Stop();
                }
            }
            catch (Exception)
            {
                await promptService.ShowWarning(AppConst.PromptText.Exception, true, true);
                exceptionHappened = true;
                await carDataContext.Reset();
                await Start();
            }
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
                var cars = await carDataContext.GetData();
                if (cars is null || !cars.Any())
                {
                    await promptService.ShowWarning(AppConst.PromptText.NoCarInTheList, true, true);
                    await AskProcessOption();
                }
                await RunSimulation();
            }
        }
        private async Task RunSimulation()
        {
            await simulationService.Run();
            await ShowListOfCar();
            await ShowResult();
        }
        private async Task ShowListOfCar()
        {
            var cars = await carDataContext.GetData();
            await promptService.ShowMessage(AppConst.PromptText.ListOfCarText, true, false);
            var sb = new StringBuilder();
            cars.ForEach(async car =>
            {
                sb.AppendLine($"- {car.name}, ({car.initialPosition.x},{car.initialPosition.y}) {car.initialPosition.direction}, {car.command.command}");
            });
            await promptService.ShowMessage(sb.ToString(), false, true);
        }
        private async Task ShowResult()
        {
            await promptService.ShowMessage(AppConst.PromptText.AfterSimulationResultText, false, false);
            var sb = new StringBuilder();
            var cars = await carDataContext.GetData();
            var collideCars = cars.Where(c => c.collide).ToList();
            cars.OrderBy(c => c.name).ToList().ForEach(car =>
            {
                //After stopped, another moving car might be come and hit.
                var hitBy = car.hitAfterStopped.Any() ? string.Join(",", car.hitAfterStopped.Select(c => c.name)) : String.Empty;

                sb.Append($"- {car.name}, ");
                if (car.collide)
                {
                    var collideWith = string.Join(",", car.collideWith.Select(c => c.name));                    
                    sb.AppendLine($"collides with {collideWith} at ({car.currentPosition.x},{car.currentPosition.y}) at step {car.collideStep}");
                }
                else
                {
                    sb.AppendLine($"({car.currentPosition.x},{car.currentPosition.y}) {car.currentPosition.direction}");
                }
            });
            await promptService.ShowMessage(sb.ToString(), false, true);
        }
        public async Task Stop()
        {
            await Task.Run(() =>
            {
                Console.ReadKey();
                this.hostApplicationLifetime.StopApplication();
            });
        }
    }

    public interface IProcess
    {
        Task Start();
        Task Stop();
    }
}
