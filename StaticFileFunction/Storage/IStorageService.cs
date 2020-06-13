using System.IO;
using System.Threading.Tasks;

namespace StaticFileFunction.Storage
{
    public interface IStorageService
    {
        Task<bool> FileExistsAsync(string path);

        Task<Stream> ReadAsync(string path);
    }
}
