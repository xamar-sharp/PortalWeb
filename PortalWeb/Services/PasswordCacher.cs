using Microsoft.Extensions.Caching.Memory;
namespace PortalWeb.Services
{
    public sealed class PasswordCacher:IPasswordCacher
    {
        private readonly IMemoryCache _cache;
        public string Password { get
            {
                return _cache.Get("OldPassword") as string;
            }
            set
            {
                _cache.Set("OldPassword", value, new MemoryCacheEntryOptions() { Priority = CacheItemPriority.High }.SetAbsoluteExpiration(System.TimeSpan.FromSeconds(Program._singleTone.CacheLifetime)));
            }
        }
        public PasswordCacher(IMemoryCache cache)
        {
            _cache = cache;
        }
    }
}
