namespace AutoDrivingCarSimulation.Data
{
    public class CarData : CarReferenceData
    {
        public PositionData initialPosition { get; set; }
        public PositionData currentPosition { get; set; }
        public CommandData command { get; set; }
        public bool outOfSimulationField { get; set; }
        public bool collide { get; set; }
        public int collideStep { get; set; }
        public bool stopped { get; set; }
        public List<CarReferenceData> hitAfterStopped { get; set; } = new List<CarReferenceData>();
        public List<CarReferenceData> collideWith { get; set; } = new List<CarReferenceData>();
    }

    public class CarReferenceData
    {
        public string name { get; set; }
    }
}
