using AutoDrivingCarSimulation.Data;
using AutoDrivingCarSimulation.FormatChecker;
using AutoDrivingCarSimulation.Services;
using Moq;
using System.Text.RegularExpressions;

namespace AutoDrivingCarSimulation.UnitTest
{
    public class PositionInputTest : UnitTestBase
    {
        private const string pattern = @"^\d+\s\d+\s[NSWE]$";
        private readonly Mock<PositionInputFormatChecker> inputChecker = new Mock<PositionInputFormatChecker>(pattern);
        private readonly IAskPositionService service;
        public PositionInputTest()
        {
            service = new AskPositionService(promptService.Object,
                simulationFieldDataContext.Object,
                inputChecker.Object);
        }

        //[Theory]
        //[InlineData("1 1 N")]
        //[InlineData("11N")]
        //[InlineData("1 1N")]
        //[InlineData("0 0 N")]
        //public async Task PositionInput(string? input)
        //{
        //    var testResult = await Test(input);
        //    if (testResult.exceptedInput)
        //    {
        //        Assert.True(testResult.taskCompleted);
        //    }
        //    else
        //    {
        //        Assert.False(testResult.taskCompleted);
        //    }
        //}

        //[Theory]
        //[InlineData("11 11 N")]
        //public async Task PositionOutOfField(string? input)
        //{
        //    var testResult = await Test(input);
        //    Assert.False(testResult.taskCompleted);
        //}

        //private async Task<(bool exceptedInput, bool taskCompleted)> Test(string? input)
        //{
        //    var askForThisCar = new CarData()
        //    {
        //        name = "A"
        //    };

        //    promptService
        //        .Setup(s => s.AskInput(AppConst.PromptText.AskInitialPosition, false, true, askForThisCar.name))
        //        .Returns(Task.FromResult(input));

        //    fieldDataContext
        //        .Setup(s => s.GetData())
        //        .Returns(new FieldData()
        //        {
        //            width = 10,
        //            height = 10
        //        });

        //    var regx = new Regex(pattern);
        //    var exceptedInput = regx.IsMatch(input ?? string.Empty);
        //    var taskCompleted = Task.Run(() => service.Ask(askForThisCar)).Wait(TimeSpan.FromSeconds(5));

        //    return (exceptedInput, taskCompleted);
        //}

        [Theory]
        [InlineData("1 1 N")]
        [InlineData("0 0 N")]
        public async Task ValidScenarios(string? input)
        {
            var taskCompleted = await Test(input);
            Assert.True(taskCompleted);
        }

        [Theory]
        [InlineData("11N")]
        [InlineData("1 1N")]
        public async Task InvalidScenarios(string? input)
        {
            var taskCompleted = await Test(input);
            Assert.False(taskCompleted);
        }

        [Theory]
        [InlineData("11 11 N")]
        public async Task InitialPositionOutOfFieldScenario(string? input)
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
                .Setup(s => s.AskInput(AppConst.PromptText.AskInitialPosition, false, true, askForThisCar.name))
                .Returns(Task.FromResult(input));

            simulationFieldDataContext
                .Setup(s => s.GetData())
                .Returns(Task.FromResult(new SimulationFieldData()
                {
                    width = 10,
                    height = 10
                }));

            var taskCompleted = Task.Run(() => service.Ask(askForThisCar)).Wait(TimeSpan.FromSeconds(5));
            return await Task.FromResult(taskCompleted);
        }
    }
}
