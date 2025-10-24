using ZiggyCreatures.Caching.Fusion;

namespace FactElectFixed.Api.Services;

public class CachedXmlFileService(IFusionCache memoryCache) : ICachedXmlFileService
{
    public string GetXml(string filePath)
    {
        return memoryCache.GetOrSet(filePath, entry => File.ReadAllText(filePath), TimeSpan.FromMinutes(5));
    }
}
