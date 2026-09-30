using System;
using UnityEngine;

namespace junya005.SaveFileSystem
{
    /// <summary>
    /// シリアライザーのインターフェース。
    /// このインターフェースを使用して、シリアライザーの差し替えを行うことができます。
    /// </summary>
    public interface ISerializer
    {
        string Serialize<T>(T data) where T : class;

        T Deserialize<T>(string serializedData) where T : class;
    }
}
