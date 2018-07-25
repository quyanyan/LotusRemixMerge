using System.Collections.Generic;
using System.Reflection;

namespace Processor.Helpers
{
    public interface IResourceLoader
    {
        HashSet<string> LoadIntoHashSet(Assembly assembly, string resourceName, char delimiter);
        IEnumerable<string> Load(Assembly assembly, string resourceName, char delimiter);
    }
}
