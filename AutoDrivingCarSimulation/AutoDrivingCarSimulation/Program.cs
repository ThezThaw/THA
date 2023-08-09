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
    services.AddScoped<IDataContext<List<CarData>>, DataContext<List<CarData>>>();
    #endregion

    #region FormatChecker
    services.AddScoped(s => new SimulationFieldInputChecker(@"^\d+\s\d+$"));
    services.AddScoped(s => new ProcessOptionChecker(@"^[12]$"));
    services.AddScoped(s => new PositionInputFormatChecker(@"^\d+\s\d+\s[NSWE]$"));
    services.AddScoped(s => new CommandInputChecker(@"^[LRF]+$"));
    #endregion

    #region Services
    services.AddScoped<IPromptService, PromptService>();    
    services.AddScoped<ISimulationFieldService, SimulationFieldService>();
    services.AddScoped<IAskOutput<ProcessOptionData>, AskProcessOptionService>();
    services.AddScoped<ICarService, CarService>();
    services.AddScoped<IAskPositionService, AskPositionService>();
    services.AddScoped<IAskCommandService, AskCommandService>();
    services.AddScoped<IDriveHolder<Rotate>, Rotate>();
    services.AddScoped<IDriveHolder<Move>, Move>();
    services.AddScoped<ISimulationService, SimulationService>();
    services.AddScoped<ISimulationFactory, SimulationFactory>();
    #endregion    

    services.AddScoped<IProcess, Process>();
    services.AddHostedService<App>();
})
.Build()
.Run();
