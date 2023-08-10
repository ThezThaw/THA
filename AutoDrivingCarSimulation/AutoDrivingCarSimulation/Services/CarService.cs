using AutoDrivingCarSimulation.Data;

namespace AutoDrivingCarSimulation.Services
{
    public class CarService : ServiceBase, ICarService
    {
        private readonly IDataContext<List<CarData>> dataContext;
        public CarService(IPromptService promptService,
            IDataContext<List<CarData>> dataContext) :base(promptService)
        {
            this.dataContext = dataContext;
        }

        public async Task<CarData> Add()
        {
            var cars = await dataContext.GetData();

            var duplicateName = false;
            var carNameEmpty = false;//to check user input
            var carName = string.Empty;
            do
            {
                if (duplicateName)
                {
                    await promptService.ShowWarning(AppConst.PromptText.DuplicateCarName, true, true, carName);
                }
                else if (carNameEmpty)
                {
                    await promptService.ShowMessage("", true, true);
                }
                carName = await promptService.AskInput(AppConst.PromptText.AskCarName, false, true);
                carName = carName.Trim();
                carNameEmpty = string.IsNullOrEmpty(carName);
                if (carNameEmpty)
                {
                    duplicateName = false;
                }
                else
                {
                    duplicateName = cars is null ? false : cars.Any(x => x?.name == carName);
                }

            } while (duplicateName || carNameEmpty);

            var car = new CarData()
            {
                name = carName
            };
            
            if (cars is null) cars = new List<CarData>();            
            cars.Add(car);
            await dataContext.SetData(cars);
            await promptService.Clear();
            return car;
        }
    }

    public interface ICarService
    {
        Task<CarData> Add();
    }
}
