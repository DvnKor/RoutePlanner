using System.Linq;
using Entities.Models;

namespace GeneticAlgorithm.Domain.Models
{
    public class Genotype
    {
        public Route[] Routes { get; }

        public double Fitness => Routes.Select(route => route.Fitness).Sum();

        public Genotype(Route[] routes)
        {
            Routes = routes;
        }
    }
}