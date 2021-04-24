using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Common
{
    public static class ListExtensions
    {
        private static readonly Random Random = new Random();  

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            var result = new T[list.Count];
            list.CopyTo(result, 0);
            
            var n = list.Count;
            while (n > 1) {  
                n--;  
                var k = Random.Next(n + 1);  
                var value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }

            return result;
        }
        
        public static IEnumerable<double> CumulativeSum(this IEnumerable<double> values)
        {
            double sum = 0;
            foreach(var item in values)
            {
                sum += item;
                yield return sum;
            }        
        }

        public static IEnumerable<double> CumulativePercentage(this IEnumerable<double> values)
        {
            var cumulativeSum = values.CumulativeSum().ToArray();
            var sum = cumulativeSum.Last();
            foreach (var cumSum in cumulativeSum)
            {
                yield return (100 * cumSum) / sum;
            }
        }
    }
}