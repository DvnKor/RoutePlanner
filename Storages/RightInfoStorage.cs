using System.Threading.Tasks;
using Entities;
using Infrastructure.Rights;

namespace Storages
{
    public interface IRightInfoStorage
    {
        Task<string> GetDescription(Right right);
    }
    
    public class RightInfoStorage : IRightInfoStorage
    {
        private readonly IRoutePlannerContextFactory _contextFactory;

        public RightInfoStorage(IRoutePlannerContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> GetDescription(Right right)
        {
            await using var ctx = _contextFactory.Create();
            var rightInfo = await ctx.RightInfos.FindAsync(right);
            return rightInfo?.Description;
        }
    }
}