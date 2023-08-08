using System.Text.RegularExpressions;

namespace AutoDrivingCarSimulation.FormatChecker
{
    public class SimulationFieldInputChecker
    {        
        private readonly Regex regex;
        public SimulationFieldInputChecker(string pattern)
        {
            regex = new Regex(pattern);
        }
        public async Task<bool> IsMatch(string input)
        {
            return await Task.FromResult(regex.IsMatch(input));
        }
    }
}
