using static AutoDrivingCarSimulation.AppEnum;

namespace AutoDrivingCarSimulation.Services
{
    public class SimulationFactory : ISimulationFactory
    {
        private readonly IDriveHolder<Rotate> rotate;
        private readonly IDriveHolder<Move> move;
        public SimulationFactory(IDriveHolder<Rotate> rotate,
            IDriveHolder<Move> move)
        {
            this.rotate = rotate;
            this.move = move;
        }
        public IDriveService GetDriveService(DriveCommand driveCommand)
        {
            return driveCommand switch
            {
                DriveCommand.L or DriveCommand.R => (IDriveService)rotate,
                DriveCommand.F => (IDriveService)move,
                _ => (IDriveService)rotate,
            };
        }
    }

    public interface ISimulationFactory
    {
        IDriveService GetDriveService(DriveCommand driveCommand);
    }
}
