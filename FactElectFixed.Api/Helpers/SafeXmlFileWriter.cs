using System.Text;

namespace FactElectFixed.Api.Helpers;

public static class SafeXmlFileWriter
{
    private static readonly object _fileLock = new();

    public static void SaveXml(string path, string xmlContent)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        string tempFile = path + ".tmp";

        lock (_fileLock)
        {
            using (var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.Write(xmlContent);
            }

            File.Copy(tempFile, path, true);

            File.Delete(tempFile);
        }
    }
}
