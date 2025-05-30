using System.IO;
using System.Text;
using UnityEngine;

public interface IStorageService
{
    void Save(string fileName, string content);
    string Load(string fileName);
    bool Exists(string fileName);
}