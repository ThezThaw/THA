namespace AutoDrivingCarSimulation.Data
{
    public class CarData
    {
        public string name { get; set; }
        public PositionData initialPosition { get; set; }
        public PositionData currentPosition { get; set; }
        public CommandData command { get; set; }
    }
}
