using AutoDrivingCarSimulation.Data;
using static AutoDrivingCarSimulation.AppEnum;

namespace AutoDrivingCarSimulation.Services
{
    public class SimulationService : ServiceBase, ISimulationService
    {
        private readonly IDataContext<SimulationFieldData> simulationFieldDataContext;
        private readonly IDataContext<List<CarData>> carDataContext;
        private readonly ISimulationFactory simulationFactory;
        public SimulationService(IPromptService promptService,
            ISimulationFactory simulationFactory,
            IDataContext<SimulationFieldData> simulationFieldDataContext,
            IDataContext<List<CarData>> carDataContext) : base(promptService)
        {   
            this.simulationFieldDataContext = simulationFieldDataContext;
            this.carDataContext = carDataContext;
            this.simulationFactory = simulationFactory;
        }

        public async Task Run()
        {            
            var maxCommandLen = carDataContext.GetData().Max(x => x.command.command.Length);
            for (int idx = 0; idx < maxCommandLen; idx++)
            {
                foreach (var car in carDataContext.GetData())
                {
                    //skip drive if 'out of field' or collide with other car or no more command
                    var command = car.command.command.ToCharArray();
                    if (car.outOfSimulationField || car.collide || command.Length <= idx)
                    {
                        car.stopped = true;
                        continue;
                    }

                    //Get RotateService or MoveService base on command
                    Enum.TryParse(command[idx].ToString(), out DriveCommand driveCommand);
                    var driveService = simulationFactory.GetDriveService(driveCommand);

                    ///Set drive paramenter(Rotate or Move) and drive
                    await driveService.SetDriveParameter(driveCommand);
                    var newPosition = await driveService.Drive(car.currentPosition);

                    //Check out of field, if yes ignore command and stop at that position
                    car.outOfSimulationField = await IsOutOfField(newPosition);
                    if (!car.outOfSimulationField)
                    {
                        car.currentPosition = newPosition;
                    }
                }
                await FindCollideCars(idx);
            }
        }

        private async Task<bool> IsOutOfField(PositionData position)
        {
            var fieldXY = simulationFieldDataContext.GetData();
            var outOfField = position.x < 0 
                             ||
                             position.y < 0 
                             ||
                             position.x > fieldXY.width 
                             ||
                             position.y > fieldXY.height;
            return await Task.FromResult(outOfField);
        }
        private async Task FindCollideCars(int currentStepZeroBase)
        {
            await Task.Run(() =>
            {
                var samePositionCarGroup = carDataContext.GetData()
                                            .GroupBy(car => new
                                            {
                                                car.currentPosition.x,
                                                car.currentPosition.y
                                            })
                                            .ToList();

                foreach (var samePositionCars in samePositionCarGroup)
                {
                    //more than one car at same position means collide
                    if (samePositionCars.Count() > 1)
                    {
                        var stoppedCars = samePositionCars.Where(c => c.stopped);
                        var drivingCars = samePositionCars.Where(c => !c.stopped);

                        stoppedCars.ToList().ForEach(car => 
                        {
                            var cars = drivingCars.Select(c => new CarReferenceData() { name = c.name });                            
                            car.hitAfterStopped.AddRange(cars);
                        });

                        drivingCars.ToList().ForEach(car =>
                        {
                            var otherDrivingOrStoppedCars = samePositionCars.Where(c => c.name != car.name).Select(c => new CarReferenceData() { name = c.name });
                            car.collideWith.AddRange(otherDrivingOrStoppedCars);
                            car.collide = true;
                            car.stopped = true;
                            car.collideStep = (currentStepZeroBase + 1);
                        });
                    }
                }
            });
        }
    }

    public interface ISimulationService
    {
        Task Run();
    }
}
