using AutoDrivingCarSimulation.Data;
using Newtonsoft.Json;
using static AutoDrivingCarSimulation.AppEnum;

namespace AutoDrivingCarSimulation.Services
{
    public class Rotate : IDriveHolder<Rotate>, IDriveService
    {
        private int rotateDegree { get; set; }
        public Rotate() { }
        public async Task<PositionData> Drive(PositionData currentPosition)
        {
            var newPosition = JsonConvert.DeserializeObject<PositionData>(JsonConvert.SerializeObject(currentPosition));
            var newDirection = (int)currentPosition.direction + rotateDegree;
            //newDirection = newDirection % 360;            
            //((x % y) + y) % y
            newDirection = ((newDirection % 360) + 360) % 360; // find modulus 
            newPosition.direction = (Direction)newDirection;
            return await Task.FromResult(newPosition);
        }

        public async Task SetDriveParameter(DriveCommand driveCommand)
        {
            await Task.Run(() => rotateDegree = (int)driveCommand);
        }
    }
}
