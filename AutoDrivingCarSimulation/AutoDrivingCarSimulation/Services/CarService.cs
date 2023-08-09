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
            var duplicateName = false;
            var carName = string.Empty;
            do
            {
                if (duplicateName)
                {
                    await promptService.ShowWarning(AppConst.PromptText.DuplicateCarName, true, true, carName);
                }
                carName = await promptService.AskInput(AppConst.PromptText.AskCarName, false, true);
                duplicateName = dataContext.GetData() is null ? false : dataContext.GetData().Any(x => x?.name == carName);

            } while (duplicateName);

            var car = new CarData()
            {
                name = carName
            };

            var cars = dataContext.GetData();
            if (cars is null) cars = new List<CarData>();            
            cars.Add(car);
            dataContext.SetData(cars);
            await promptService.Clear();
            return car;
        }
    }

    public interface ICarService
    {
        Task<CarData> Add();
    }
}
