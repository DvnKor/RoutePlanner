using System;
using System.Collections.Generic;

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
    }
}