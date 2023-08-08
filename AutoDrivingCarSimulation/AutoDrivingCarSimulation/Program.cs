// See https://aka.ms/new-console-template for more information


using AutoDrivingCarSimulation;
using AutoDrivingCarSimulation.Data;
using AutoDrivingCarSimulation.FormatChecker;
using AutoDrivingCarSimulation.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.Configure<ConsoleLifetimeOptions>(opt => opt.SuppressStatusMessages = true);

    #region DataContext
    services.AddScoped<IDataContext<SimulationFieldData>, DataContext<SimulationFieldData>>();
    #endregion

    #region FormatChecker
    services.AddScoped(s => new SimulationFieldInputChecker(@"^\d+\s\d+$"));
    #endregion

    #region Services
    services.AddScoped<IPromptService, PromptService>();
    services.AddScoped<ISimulationFieldService, SimulationFieldService>();
    #endregion    

    services.AddScoped<IProcess, Process>();
    services.AddHostedService<App>();
})
.Build()
.Run();
