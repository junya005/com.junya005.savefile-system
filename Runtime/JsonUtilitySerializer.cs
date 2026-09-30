using System;
using UnityEngine;

namespace junya005.SaveFileSystem
{
    public class JsonUtilitySerializer : ISerializer
    {
        public string Serialize<T>(T data) where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Cannot serialize null object.");
            }

            try
            {
                return JsonUtility.ToJson(data, true);
            }
            catch (Exception ex)
            {
                throw new SaveSystemException(
                    SaveSystemErrorCode.UnknownError,
                    $"Failed to serialize object of type {typeof(T).Name}.",
                    ex);
            }
        }

        public T Deserialize<T>(string serializedData) where T : class
        {
            // nullもしくは空欄時は例外をスロー
            if (string.IsNullOrWhiteSpace(serializedData))
            {
                throw new SaveSystemException(
                    SaveSystemErrorCode.DeserializationFailed,
                    $"Deserialized object of type {typeof(T).Name} returned null.");
            }

            try
            {
                T data = JsonUtility.FromJson<T>(serializedData);

                if (data == null)
                {
                    throw new SaveSystemException(
                        SaveSystemErrorCode.DeserializationFailed,
                        $"Deserialized object of type {typeof(T).Name} returned null.");
                }

                return data;
            }
            catch (ArgumentException ex)
            {
                throw new SaveSystemException(
                    SaveSystemErrorCode.DeserializationFailed,
                    $"JSON format is invalid for type {typeof(T).Name}.",
                    ex);
            }
            catch (Exception ex)
            {
                throw new SaveSystemException(
                    SaveSystemErrorCode.UnknownError,
                    $"An unexpected error occurred during deserialization of {typeof(T).Name}.",
                    ex);
            }
        }
    }
}
