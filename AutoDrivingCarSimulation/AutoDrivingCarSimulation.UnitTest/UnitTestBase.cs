using AutoDrivingCarSimulation.Data;
using AutoDrivingCarSimulation.Services;
using Moq;

namespace AutoDrivingCarSimulation.UnitTest
{
    public class UnitTestBase
    {
        protected readonly Mock<IPromptService> promptService = new Mock<IPromptService>();
        protected readonly Mock<IDataContext<SimulationFieldData>> simulationFieldDataContext = new Mock<IDataContext<SimulationFieldData>>();
        protected readonly Mock<IDataContext<List<CarData>>> carDataContext = new Mock<IDataContext<List<CarData>>>();
    }
}