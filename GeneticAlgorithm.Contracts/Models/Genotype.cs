using System.Linq;
using Entities.Models;

namespace GeneticAlgorithm.Contracts.Models
{
    public class Genotype
    {
        public Route[] Routes { get; }

        public double Fitness => Routes.Select(route => route.Fitness).Sum();

        public double Distance => Routes.Select(route => route.Distance).Sum();

        public double WaitingTime => Routes.Select(route => route.WaitingTime).Sum();

        public int SuitableMeetingsCount => Routes.SelectMany(route => route.SuitableMeetings).Count();

        public Genotype(Route[] routes)
        {
            Routes = routes;
        }
    }
}