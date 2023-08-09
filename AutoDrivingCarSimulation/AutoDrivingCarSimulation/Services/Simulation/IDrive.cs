using AutoDrivingCarSimulation.Data;
using static AutoDrivingCarSimulation.AppEnum;

namespace AutoDrivingCarSimulation.Services
{
    public interface IDriveHolder<T> where T : class { }

    public interface IDriveService
    {
        Task<PositionData> Drive(PositionData currentPosition);
        Task SetDriveParameter(DriveCommand driveCommand);
    }
}
