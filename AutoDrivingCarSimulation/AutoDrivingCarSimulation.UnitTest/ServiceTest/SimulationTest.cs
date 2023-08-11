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

        [Fact]
        public async Task Collide__2Cars__SimulationTest()
        {
            var cars = new List<CarData>();
            cars.Add(new CarData()
            {
                name = "A",
                initialPosition = new PositionData()
                {
                    x = 10,
                    y = 0,
                    direction = AppEnum.Direction.N
                },
                currentPosition = new PositionData()
                {
                    x = 10,
                    y = 0,
                    direction = AppEnum.Direction.N
                },
                command = new CommandData()
                {
                    command = "FFFFFLFFFFFRFF"
                }
            });
            cars.Add(new CarData()
            {
                name = "B",
                initialPosition = new PositionData()
                {
                    x = 1,
                    y = 0,
                    direction = AppEnum.Direction.N
                },
                currentPosition = new PositionData()
                {
                    x = 1,
                    y = 0,
                    direction = AppEnum.Direction.N
                },
                command = new CommandData()
                {
                    command = "FFFFFFFFFRFFFFFFFRFFRFFFFF\r\n"
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
            Assert.Equal(7, carA.currentPosition.y);
            Assert.Equal(Direction.N, carA.currentPosition.direction);
            Assert.True(carA.hitAfterStopped.Any());
            Assert.Contains("B", carA.hitAfterStopped.Select(c => c.name));

            Assert.Equal(5, carB.currentPosition.x);
            Assert.Equal(7, carB.currentPosition.y);
            Assert.Equal(Direction.W, carB.currentPosition.direction);
            Assert.True(carB.collide);
            Assert.Contains("A", carB.collideWith.Select(c => c.name));
            Assert.False(carB.hitAfterStopped.Any());
        }

        [Fact]
        public async Task Collide__3Cars__SimulationTest()
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
                    command = "FRF"
                }
            });
            cars.Add(new CarData()
            {
                name = "B",
                initialPosition = new PositionData()
                {
                    x = 0,
                    y = 0,
                    direction = AppEnum.Direction.E
                },
                currentPosition = new PositionData()
                {
                    x = 0,
                    y = 0,
                    direction = AppEnum.Direction.E
                },
                command = new CommandData()
                {
                    command = "FLF"
                }
            });
            cars.Add(new CarData()
            {
                name = "C",
                initialPosition = new PositionData()
                {
                    x = 1,
                    y = 1,
                    direction = AppEnum.Direction.N
                },
                currentPosition = new PositionData()
                {
                    x = 1,
                    y = 1,
                    direction = AppEnum.Direction.N
                },
                command = new CommandData()
                {
                    command = "FRFRFRF"
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
            var carC = cars.First(c => c.name == "C");

            Assert.Equal(1, carA.currentPosition.x);
            Assert.Equal(1, carA.currentPosition.y);
            Assert.Equal(Direction.E, carA.currentPosition.direction);
            Assert.True(carA.collide);
            Assert.Contains("B", carA.collideWith.Select(c => c.name));
            Assert.Equal(3, carA.collideStep);
            Assert.True(carA.hitAfterStopped.Any());
            Assert.Contains("C", carA.hitAfterStopped.Select(c => c.name));

            Assert.Equal(1, carB.currentPosition.x);
            Assert.Equal(1, carB.currentPosition.y);
            Assert.Equal(Direction.N, carB.currentPosition.direction);
            Assert.True(carB.collide);
            Assert.Contains("A", carB.collideWith.Select(c => c.name));
            Assert.Equal(3, carB.collideStep);
            Assert.True(carB.hitAfterStopped.Any());
            Assert.Contains("C", carB.hitAfterStopped.Select(c => c.name));

            Assert.Equal(1, carC.currentPosition.x);
            Assert.Equal(1, carC.currentPosition.y);
            Assert.Equal(Direction.W, carC.currentPosition.direction);
            Assert.True(carC.collide);
            Assert.Contains("A", carC.collideWith.Select(c => c.name));
            Assert.Contains("B", carC.collideWith.Select(c => c.name));
            Assert.Equal(7, carC.collideStep);
            Assert.False(carC.hitAfterStopped.Any());
        }

        [Fact]
        public async Task Collide__4Cars__SimulationTest()
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
                    command = "FRF"
                }
            });
            cars.Add(new CarData()
            {
                name = "B",
                initialPosition = new PositionData()
                {
                    x = 0,
                    y = 0,
                    direction = AppEnum.Direction.E
                },
                currentPosition = new PositionData()
                {
                    x = 0,
                    y = 0,
                    direction = AppEnum.Direction.E
                },
                command = new CommandData()
                {
                    command = "FLF"
                }
            });
            cars.Add(new CarData()
            {
                name = "C",
                initialPosition = new PositionData()
                {
                    x = 1,
                    y = 1,
                    direction = AppEnum.Direction.N
                },
                currentPosition = new PositionData()
                {
                    x = 1,
                    y = 1,
                    direction = AppEnum.Direction.N
                },
                command = new CommandData()
                {
                    command = "FRFRFRF"
                }
            });
            cars.Add(new CarData()
            {
                name = "D",
                initialPosition = new PositionData()
                {
                    x = 1,
                    y = 1,
                    direction = AppEnum.Direction.E
                },
                currentPosition = new PositionData()
                {
                    x = 1,
                    y = 1,
                    direction = AppEnum.Direction.E
                },
                command = new CommandData()
                {
                    command = "FFFRRFFF"
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
            var carC = cars.First(c => c.name == "C");
            var carD = cars.First(c => c.name == "D");

            Assert.Equal(1, carA.currentPosition.x);
            Assert.Equal(1, carA.currentPosition.y);
            Assert.Equal(Direction.E, carA.currentPosition.direction);
            Assert.True(carA.collide);
            Assert.Contains("B", carA.collideWith.Select(c => c.name));
            Assert.Equal(3, carA.collideStep);
            Assert.True(carA.hitAfterStopped.Any());
            Assert.Contains("C", carA.hitAfterStopped.Select(c => c.name));
            Assert.Contains("D", carA.hitAfterStopped.Select(c => c.name));

            Assert.Equal(1, carB.currentPosition.x);
            Assert.Equal(1, carB.currentPosition.y);
            Assert.Equal(Direction.N, carB.currentPosition.direction);
            Assert.True(carB.collide);
            Assert.Contains("A", carB.collideWith.Select(c => c.name));
            Assert.Equal(3, carB.collideStep);
            Assert.True(carB.hitAfterStopped.Any());
            Assert.Contains("C", carB.hitAfterStopped.Select(c => c.name));
            Assert.Contains("D", carB.hitAfterStopped.Select(c => c.name));

            Assert.Equal(1, carC.currentPosition.x);
            Assert.Equal(1, carC.currentPosition.y);
            Assert.Equal(Direction.W, carC.currentPosition.direction);
            Assert.True(carC.collide);
            Assert.Contains("A", carC.collideWith.Select(c => c.name));
            Assert.Contains("B", carC.collideWith.Select(c => c.name));
            Assert.Equal(7, carC.collideStep);
            Assert.True(carC.hitAfterStopped.Any());
            Assert.Contains("D", carC.hitAfterStopped.Select(c => c.name));

            Assert.Equal(1, carD.currentPosition.x);
            Assert.Equal(1, carD.currentPosition.y);
            Assert.Equal(Direction.W, carD.currentPosition.direction);
            Assert.True(carD.collide);
            Assert.Contains("A", carD.collideWith.Select(c => c.name));
            Assert.Contains("B", carD.collideWith.Select(c => c.name));
            Assert.Contains("C", carD.collideWith.Select(c => c.name));
            Assert.Equal(8, carD.collideStep);
            Assert.False(carD.hitAfterStopped.Any());
        }
    }
}
