using AutoDrivingCarSimulation.Data;
using Newtonsoft.Json;
using static AutoDrivingCarSimulation.AppEnum;

namespace AutoDrivingCarSimulation.Services
{
    public class Move : IDriveHolder<Move>, IDriveService
    {
        protected int moveGrid { get; set; }
        public Move() { }
        public async Task<PositionData> Drive(PositionData currentPosition)
        {
            var newPosition = JsonConvert.DeserializeObject<PositionData>(JsonConvert.SerializeObject(currentPosition));
            if (currentPosition.direction == Direction.N)
            {
                newPosition.y += moveGrid;
            }
            else if (currentPosition.direction == Direction.S)
            {
                newPosition.y += (moveGrid * -1);
            }
            else if (currentPosition.direction == Direction.E)
            {
                newPosition.x += moveGrid;
            }
            else if (currentPosition.direction == Direction.W)
            {
                newPosition.x += (moveGrid * -1);
            }
            return await Task.FromResult(newPosition);
        }

        public async Task SetDriveParameter(DriveCommand driveCommand)
        {
            await Task.Run(() => moveGrid = (int)driveCommand);
        }
    }
}
