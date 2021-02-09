using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Entities.Common
{
    public static class PropertyBuilderExtensions
    {
        public static PropertyBuilder<T> HasSimpleJsonConversion<T>(this PropertyBuilder<T> builder)
        {
            return builder.HasConversion(
                obj => JsonConvert.SerializeObject(obj),
                jsonObj => JsonConvert.DeserializeObject<T>(jsonObj));
        }
    }
}