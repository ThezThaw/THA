namespace AutoDrivingCarSimulation.Services
{
    public class ServiceBase
    {
        protected readonly IPromptService promptService;
        public ServiceBase(IPromptService promptService)
        {
            this.promptService = promptService;
        }
    }
}
