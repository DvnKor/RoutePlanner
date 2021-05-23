using System.Threading.Tasks;
using Entities;
using GeneticAlgorithm.Contracts;
using GeneticAlgorithm.Domain;
using SimpleInjector;
using Storages;

namespace RoutePlannerDaemon
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var container = new Container();
            
            container.RegisterSingleton<IPopulationGenerator, PopulationGenerator>();
            // реализовать routeStepCalculator 
            container.RegisterSingleton<IRouteStepCalculator, FakeRouteStepCalculator>();
            container.RegisterSingleton<IRouteParametersCalculator, RouteParametersCalculator>();
            
            container.RegisterSingleton<IFitnessCalculator, FitnessCalculator>();
            container.RegisterSingleton<IGenerationRanker, GenerationRanker>();
            
            container.RegisterSingleton<IGenerationSelector, GenerationSelector>();
            container.RegisterSingleton<IGenerationBreeder, GenerationBreeder>();
            container.RegisterSingleton<IGenerationMutator, GenerationMutator>();
            container.RegisterSingleton<IGenerationCreator, GenerationCreator>();
            
            container.RegisterSingleton<IRoutePlanner, RoutePlanner>();

            container.RegisterSingleton<IRoutePlannerContextFactory, RoutePlannerContextFactory>();
            container.RegisterSingleton<IMeetingStorage, MeetingStorage>();
            container.RegisterSingleton<IManagerScheduleStorage, ManagerScheduleStorage>();
            container.RegisterSingleton<IRouteStorage, RouteStorage>();
            
            container.Register<RoutesUpdater>();

            container.Verify();

            var routesUpdater = container.GetInstance<RoutesUpdater>();
            await routesUpdater.RunAsync();
        }
    }
}