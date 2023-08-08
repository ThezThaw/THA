using AutoDrivingCarSimulation.Data;
using AutoDrivingCarSimulation.FormatChecker;
using AutoDrivingCarSimulation.Services;
using Moq;
using System.Text.RegularExpressions;

namespace AutoDrivingCarSimulation.UnitTest
{
    public class SimulationFieldInputTest
    {
        private const string pattern = @"^\d+\s\d+$";
        private readonly Mock<IPromptService> promptService = new Mock<IPromptService>();
        private readonly Mock<SimulationFieldInputChecker> inputChecker = new Mock<SimulationFieldInputChecker>(pattern);
        protected readonly Mock<IDataContext<SimulationFieldData>> fieldDataContext = new Mock<IDataContext<SimulationFieldData>>();
        private readonly ISimulationFieldService service;
        public SimulationFieldInputTest()
        {
            service = new SimulationFieldService(                
                fieldDataContext.Object,
                promptService.Object,
                inputChecker.Object);
        }

        [Theory]
        [InlineData("10 10")]
        [InlineData("1 1")]
        public async Task ValidScenarios(string? input)
        {
            var taskCompleted = await Test(input);
            Assert.True(taskCompleted);
        }

        [Theory]
        [InlineData("0 0")]
        [InlineData("abc")]
        [InlineData("1010")]
        [InlineData("10 a")]
        [InlineData("10")]        
        public async Task InvalidScenarios(string? input)
        {
            var taskCompleted = await Test(input);
            Assert.False(taskCompleted);
        }
        private async Task<bool> Test(string input)
        {
            promptService
                .Setup(s => s.AskInput(AppConst.PromptText.AskSimulationFieldInput, false, true))
                .Returns(Task.FromResult(input));

            fieldDataContext
                    .Setup(s => s.GetData())
                    .Returns(new SimulationFieldData()
                    {
                        width = 10,
                        height = 10
                    });

            var taskCompleted = Task.Run(() => service.DefineField()).Wait(TimeSpan.FromSeconds(5));
            return await Task.FromResult(taskCompleted);
        }
    }
}
