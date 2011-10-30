using System.IO;

namespace code2slide.core.Io
{
    public class FileSystem
    {
        public static void CopyDirectoryTree(string sourcePath, string destinationPath)
        {
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            CopyDirectoryStructure(sourcePath, destinationPath);
            CopyAllFiles(sourcePath, destinationPath);
        }

        private static void CopyAllFiles(string sourcePath, string destinationPath)
        {
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",
                SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), overwrite: true);
            }
        }

        private static void CopyDirectoryStructure(string sourcePath, string destinationPath)
        {
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*",
                SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
            }
        }
    }
}