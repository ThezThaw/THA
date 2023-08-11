namespace AutoDrivingCarSimulation
{
    public static class AppConst
    {
        public const string InputDelimiter = " ";
        public static class PromptText
        {
            public const string WelcomeText = "Welcome to Auto Driving Car Simulation!";
            public const string GoodbyeText = "Thank you for running the simulation. Goodbye!";

            public const string AskSimulationFieldInput = "Please enter the width and height of the simulation field in x y format:";
            public const string AskProcessOption = "Please choose from the following options:\r\n[1] Add a car to field\r\n[2] Run simulation";
            public const string AskCarName = "Please enter the name of the car:";
            public const string AskInitialPosition = "Please enter initial position of car `{0}` in x y Direction format:";
            public const string AskCommand = "Please enter the command for car `{0}`:";
            public const string AskExitOption = "Please choose from the following options:\r\n[1] Start Over\r\n[2] Exit";

            public const string InvalidOption = "Invalid option!";
            public const string InvalidSimulationFieldFormat = "Invalid input! E.g. x y format: 10 10";
            public const string InvalidSimulationFieldSize = "Simulation field too small!";
            public const string InvalidPosition = "Invalid input! Please note that only N, S, W, E (representing North, South, West, East) are allowed for direction and x y position format.\r\nE.g. x y Direction format: 1 2 N";
            public const string InvalidPositionOutOfField = "Initial position is out of field. Current field width:{0}, height:{1}.";
            public const string InvalidCommand = "Invalid input! Only allow L,R and F\r\n - L : rotates the car by 90 degrees to the left\r\n - R : rotates the car by 90 degrees to the right\r\n - F : moves forward by 1 grid point\r\nE.g. FFRFFFFRRL";

            public const string SimulationFieldInfo = "You have created a field of {0} x {1}.";
            public const string DuplicateCarName = "`{0}` already exists in car list.";            
            public const string ListOfCarText = "Your current list of cars are:";
            public const string NoCarInTheList = "There is no car to start simulation.";
            public const string AfterSimulationResultText = "After simulation, the result is:";

            public const string Exception = "Encounter some exception. Service is restarted.";
        }
    }

    public class AppEnum
    {
        public enum ProcessOption
        { 
            AddCar = 1,
            RunSimulation = 2
        }
        public enum Direction
        {
            N = 0,
            E = 90,
            S = 180,
            W = 270
        }
        public enum DriveCommand
        {
            L = -90, //counter-clockwise
            R = 90, //clockwise
            F = 1 //move one grid point
        }
        public enum ExitOption
        {
            StartOver = 1,
            Exit = 2
        }
    }

}
