using AutoDrivingCarSimulation.FormatChecker;
using AutoDrivingCarSimulation.Services;
using Moq;

namespace AutoDrivingCarSimulation.UnitTest
{
    public class ProcessOptionInputTest : UnitTestBase
    {
        private const string pattern = @"^[12]$";
        private readonly Mock<ProcessOptionChecker> inputChecker = new Mock<ProcessOptionChecker>(pattern);        
        private readonly AskProcessOptionService service;
        public ProcessOptionInputTest()
        {
            service = new AskProcessOptionService(
                promptService.Object,
                inputChecker.Object);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async Task ValidScenarios(string? input)
        {
            var taskCompleted = await Test(input);
            Assert.True(taskCompleted);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("3")]
        [InlineData("4")]
        [InlineData("a")]
        [InlineData("aa")]
        [InlineData("bbb")]
        [InlineData("abc")]
        public async Task InvalidScenarios(string? input)
        {
            var taskCompleted = await Test(input);
            Assert.False(taskCompleted);
        }
        private async Task<bool> Test(string input)
        {
            promptService
                .Setup(s => s.AskInput(AppConst.PromptText.AskProcessOption, false, true))
                .Returns(Task.FromResult(input));

            var taskCompleted = Task.Run(() => service.Ask()).Wait(TimeSpan.FromSeconds(5));
            return await Task.FromResult(taskCompleted);
        }
    }
}
