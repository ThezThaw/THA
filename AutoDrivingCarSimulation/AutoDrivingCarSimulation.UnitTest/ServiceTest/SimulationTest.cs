using AutoDrivingCarSimulation.Data;
using AutoDrivingCarSimulation.Services;
using Moq;
using static AutoDrivingCarSimulation.AppEnum;

namespace AutoDrivingCarSimulation.UnitTest
{
    public class SimulationTest : UnitTestBase
    {
        private readonly Mock<ISimulationFactory> simulationFactory = new Mock<ISimulationFactory>();
        private readonly ISimulationService simulationService;
        public SimulationTest()
        {
            simulationService = new SimulationService(
                promptService.Object,
                simulationFactory.Object,
                simulationFieldDataContext.Object,
                carDataContext.Object);
        }
        private void SimulationFactorySetup()
        {
            simulationFactory
                    .Setup(s => s.GetDriveService(It.Is<DriveCommand>(command => command == DriveCommand.L || command == DriveCommand.R)))
                    .Returns(new Rotate());

            simulationFactory
                .Setup(s => s.GetDriveService(It.Is<DriveCommand>(command => command == DriveCommand.F)))
                .Returns(new Move());
        }

        [Fact]
        public async Task ValidScenario()
        {
            var cars = new List<CarData>();
            cars.Add(new CarData()
            {
                name = "A",
                initialPosition = new PositionData()
                {
                    x = 1,
                    y = 2,
                    direction = AppEnum.Direction.N
                },
                currentPosition = new PositionData()
                {
                    x = 1,
                    y = 2,
                    direction = AppEnum.Direction.N
                },
                command = new CommandData()
                {
                    command = "FFRFFFFRRL"
                }
            });

            simulationFieldDataContext
                .Setup(s => s.GetData())
                .Returns(Task.FromResult(new SimulationFieldData() { width = 10, height = 10 }));

            carDataContext
                .Setup(s => s.GetData())
                .Returns(Task.FromResult(cars));

            SimulationFactorySetup();
            await simulationService.Run();

            var carA = cars.First(c => c.name == "A");

            Assert.Equal(5, carA.currentPosition.x);
            Assert.Equal(4, carA.currentPosition.y);
            Assert.False(carA.collide);
            Assert.False(carA.outOfSimulationField);
            Assert.Equal(Direction.S, (Direction)carA.currentPosition.direction);
        }

        [Fact]
        public async Task OutOfFieldSimulationTest()
        {
            var cars = new List<CarData>();
            cars.Add(new CarData()
            {
                name = "A",
                initialPosition = new PositionData()
                {
                    x = 0,
                    y = 0,
                    direction = AppEnum.Direction.N
                },
                currentPosition = new PositionData()
                {
                    x = 0,
                    y = 0,
                    direction = AppEnum.Direction.N
                },
                command = new CommandData()
                {
                    command = "RRFF"
                }
            });

            simulationFieldDataContext
                .Setup(s => s.GetData())
                .Returns(Task.FromResult(new SimulationFieldData() { width = 10, height = 10 }));

            carDataContext
                .Setup(s => s.GetData())
                .Returns(Task.FromResult(cars));

            SimulationFactorySetup();
            await simulationService.Run();

            var carA = cars.First(c => c.name == "A");

            Assert.Equal(0, carA.currentPosition.x);
            Assert.Equal(0, carA.currentPosition.y);
            Assert.True(carA.outOfSimulationField);
            Assert.Equal(Direction.S, (Direction)carA.currentPosition.direction);
        }
            
        [Fact]
        public async Task CollideSimulationTest()
        {
            var cars = new List<CarData>();
            cars.Add(new CarData()
            {
                name = "A",
                initialPosition = new PositionData()
                { 
                    x = 1,
                    y = 2,
                    direction = AppEnum.Direction.N
                },
                currentPosition = new PositionData()
                {
                    x = 1,
                    y = 2,
                    direction = AppEnum.Direction.N
                },
                command = new CommandData()
                { 
                    command = "FFRFFFFRRL"
                }
            });
            cars.Add(new CarData()
            {
                name = "B",
                initialPosition = new PositionData()
                {
                    x = 7,
                    y = 8,
                    direction = AppEnum.Direction.W
                },
                currentPosition = new PositionData()
                {
                    x = 7,
                    y = 8,
                    direction = AppEnum.Direction.W
                },
                command = new CommandData()
                {
                    command = "FFLFFFFFFF"
                }
            });

            simulationFieldDataContext
                .Setup(s => s.GetData())
                .Returns(Task.FromResult(new SimulationFieldData() { width = 10, height = 10 }));

            carDataContext
                .Setup(s => s.GetData())
                .Returns(Task.FromResult(cars));

            SimulationFactorySetup();
            await simulationService.Run();

            var carA = cars.First(c => c.name == "A");
            var carB = cars.First(c => c.name == "B");

            Assert.Equal(5, carA.currentPosition.x);
            Assert.Equal(4, carA.currentPosition.y);
            Assert.True(carA.collide);
            Assert.Equal(7, carA.collideStep);

            Assert.Equal(5, carB.currentPosition.x);
            Assert.Equal(4, carB.currentPosition.y);
            Assert.True(carB.collide);
            Assert.Equal(7, carB.collideStep);
        }
    }
}
