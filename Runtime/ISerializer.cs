using System;
using UnityEngine;

namespace junya005.SaveFileSystem
{
    public interface ISerializer
    {
        string Serialize<T>(T data) where T : class;

        T Deserialize<T>(string serializedData) where T : class;
    }
}
