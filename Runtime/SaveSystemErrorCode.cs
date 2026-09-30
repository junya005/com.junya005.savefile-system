using System;

namespace junya005.SaveFileSystem
{
    public enum SaveSystemErrorCode
    {
        None = 0,
        FileNotFound = 1,           // ファイルが存在しない（初回プレイ時など）
        StorageAccessDenied = 2,    // OSによってファイルの読み書きが拒否された
        DecryptionFailed = 3,       // 暗号化のパスワード相違、または暗号化データの破損
        DeserializationFailed = 4,  // JSONからクラスへの変換に失敗（クラス構造の不一致など）
        UnknownError = 99           // 予期せぬ致命的なエラー
    }
}
