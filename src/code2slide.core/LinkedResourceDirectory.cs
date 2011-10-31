using System.IO;
using System.Reflection;
using code2slide.core.Io;

namespace code2slide.core
{
    public class LinkedResourceDirectory : LinkedResource
    {
        public string ResourceDirectoryName { get; private set; }

        public LinkedResourceDirectory(string resourceDirectoryName)
        {
            ResourceDirectoryName = resourceDirectoryName;
        }

        public override void CopyTo(string outputDirectory)
        {
            var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var sourcePath = Path.Combine(assemblyDirectory, ResourceDirectoryName);
            var destinationPath = Path.Combine(outputDirectory, ResourceDirectoryName);

            FileSystem.CopyDirectoryTree(sourcePath, destinationPath);
        }
    }
}