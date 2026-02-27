using TotemPrintService.Application.Interfaces;

namespace TotemPrintService.Infrastructure.FileSystem;

public class FileSystemService : IFileSystemService
{
    public void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
}
