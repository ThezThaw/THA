using System.Text.RegularExpressions;

namespace AutoDrivingCarSimulation
{
    public abstract class FormatCheckerBase
    {
        private readonly Regex regex;
        public FormatCheckerBase(string pattern)
        {
            regex = new Regex(pattern);
        }
        public async Task<bool> IsMatch(string input)
        {
            return await Task.FromResult(regex.IsMatch(input));
        }
    }

    public interface IFormatChecker
    {
        Task<bool> IsMatch(string input);
    }
}
