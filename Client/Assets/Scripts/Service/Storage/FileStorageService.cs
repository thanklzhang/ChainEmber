using System.IO;
using System.Text;
using UnityEngine;

public class FileStorageService : IStorageService
{
    public void Save(string fileName, string content)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        File.WriteAllText(filePath, content, Encoding.UTF8);
    }

    public string Load(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        return File.Exists(filePath) ? File.ReadAllText(filePath, Encoding.UTF8) : null;
    }

    public bool Exists(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        return File.Exists(filePath);
    }
} 