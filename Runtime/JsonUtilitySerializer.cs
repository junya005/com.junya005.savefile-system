using System;
using UnityEngine;

namespace junya005.SaveFileSystem
{
    /// <summary>
    ///     Unity標準のJsonUtilityを使用したSerializer。
    ///     Json(string)とデータクラス間のデータ変換を行います。
    /// </summary>
    /// <remarks>
    ///     <b>使用例</b>
    ///     事前にMySaveDataClassの定義とインスタンス化を済ませているものとします。
    ///
    ///     <code>
    ///     ISerializer serializer = new JsonUtilitySerializer();
    ///     serializer.Serialized<MySaveDataClass>(mySaveDataClass);
    ///     MySaveDataClass saveData = serializer.Deserialized<MySaveDataClass>(json);
    ///     </code>
    ///
    ///     <b>採用理由</b>
    ///     Unity標準のJsonUtilityはデータクラスに対応しており拡張性が高いため採用しています。
    ///     また、外部依存やAOTエラーがないため、IL2CPPに最適ということも採用理由に含めています。
    ///
    ///     <b>現在判明している課題</b>
    ///     JsonUtilityはDictionaryや自作クラスのような複雑なデータ構造には対応できません。（Listは使用できます）
    /// </remarks>
    public class JsonUtilitySerializer : ISerializer
    {
        public string Serialize<T>(T data) where T : class
        {
            // 空のオブジェクトは処理できないため例外をスロー
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
                // JsonUtilityに予期せぬエラーが発生した場合は処理を中断し例外をスロー
                throw new SaveSystemException(
                    SaveSystemErrorCode.UnknownError,
                    $"Failed to serialize object of type {typeof(T).Name}.",
                    ex);
            }
        }

        public T Deserialize<T>(string serializedData) where T : class
        {
            // nullもしくは空欄の文字列は処理できないので例外をスロー
            if (string.IsNullOrWhiteSpace(serializedData))
            {
                throw new SaveSystemException(
                    SaveSystemErrorCode.DeserializationFailed,
                    $"Deserialized object of type {typeof(T).Name} returned null.");
            }

            try
            {
                T data = JsonUtility.FromJson<T>(serializedData);

                // 返り値が空の場合はデシリアライズに失敗したので例外をスロー
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
