using System.IO;

namespace OutdoorSolution.Services.Interfaces
{
    public interface IFileSystemService : IService
    {
        void DeleteImage(string relPath);

        string SaveImageStreamToFile(Stream imageStream, string extension);
    }
}
