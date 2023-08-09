using AutoDrivingCarSimulation.Data;
using AutoDrivingCarSimulation.FormatChecker;
using AutoDrivingCarSimulation.Services;
using Moq;

namespace AutoDrivingCarSimulation.UnitTest
{
    public class CommandInputTest : UnitTestBase
    {
        private const string pattern = @"^[LRF]+$";
        private readonly Mock<CommandInputChecker> inputChecker = new Mock<CommandInputChecker>(pattern);
        private readonly IAskCommandService service;
        public CommandInputTest()
        {
            service = new AskCommandService(promptService.Object,
                inputChecker.Object);
        }

        //[Theory]
        //[InlineData("AABBCC")]
        //[InlineData("ABC")]
        //[InlineData("LLRFRFF")]
        //[InlineData("FFFFFF")]
        //[InlineData("RRRRRLFFRFLLR")]
        //public async Task CommandInput(string? input)
        //{
        //    var askForThisCar = new CarData()
        //    {
        //        name = "A"
        //    };

        //    promptService
        //        .Setup(s => s.AskInput(AppConst.PromptText.AskCommand, false, true, askForThisCar.name))
        //        .Returns(Task.FromResult(input));

        //    var regx = new Regex(pattern);
        //    var exceptedInput = regx.IsMatch(input ?? string.Empty);
        //    var taskCompleted = Task.Run(() => service.Ask(askForThisCar)).Wait(TimeSpan.FromSeconds(5));

        //    if (exceptedInput)
        //    {
        //        Assert.True(taskCompleted);
        //    }
        //    else
        //    {
        //        Assert.False(taskCompleted);
        //    }
        //}

        [Theory]        
        [InlineData("LLRFRFF")]
        [InlineData("FFFFFF")]
        [InlineData("RRRRRLFFRFLLR")]
        [InlineData("L")]
        [InlineData("R")]
        [InlineData("F")]
        public async Task ValidScenarios(string? input)
        {
            var taskCompleted = await Test(input);
            Assert.True(taskCompleted);
        }

        [Theory]
        [InlineData("AABBCC")]
        [InlineData("ABC")]
        [InlineData("abcdef")]
        [InlineData("llrrff")]
        public async Task InvalidScenarios(string? input)
        {
            var taskCompleted = await Test(input);
            Assert.False(taskCompleted);
        }
        private async Task<bool> Test(string input)
        {
            var askForThisCar = new CarData()
            {
                name = "A"
            };

            promptService
                .Setup(s => s.AskInput(AppConst.PromptText.AskCommand, false, true, askForThisCar.name))
                .Returns(Task.FromResult(input));

            simulationFieldDataContext
                .Setup(s => s.GetData())
                .Returns(new SimulationFieldData()
                {
                    width = 10,
                    height = 10
                });

            var taskCompleted = Task.Run(() => service.Ask(askForThisCar)).Wait(TimeSpan.FromSeconds(5));
            return await Task.FromResult(taskCompleted);
        }
    }
}
