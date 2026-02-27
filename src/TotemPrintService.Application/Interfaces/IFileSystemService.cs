namespace TotemPrintService.Application.Interfaces;

public interface IFileSystemService
{
    void EnsureDirectoryExists(string path);
}
