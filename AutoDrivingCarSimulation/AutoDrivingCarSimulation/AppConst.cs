namespace AutoDrivingCarSimulation
{
    public static class AppConst
    {
        public const string InputDelimiter = " ";
        public static class PromptText
        {
            public const string WelcomeText = "Welcome to Auto Driving Car Simulation!";
            public const string AskSimulationFieldInput = "Please enter the width and height of the simulation field in x y format:";
            public const string AskProcessOption = "Please choose from the following options:\r\n[1] Add a car to field\r\n[2] Run simulation";

            public const string InvalidOption = "Invalid option!";
            public const string InvalidSimulationFieldFormat = "Invalid input! E.g. x y format: 10 10";
            public const string InvalidSimulationFieldSize = "Simulation field too small!";

            public const string SimulationFieldInfo = "You have created a field of {0} x {1}.";
        }
    }

    public class AppEnum
    {
        public enum ProcessOption
        { 
            AddCar = 1,
            RunSimulation = 2
        }
    }

}
