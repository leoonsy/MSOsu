using System.IO;

namespace MSOsu.Service
{
    public interface IFileService<T>
    {
        string GetOpenFilter();
        string GetSaveFilter();
        T Read(string filename);
        void Save(string filename, T val);
    }
}

