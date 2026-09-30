using System;
using NUnit.Framework;

namespace junya005.SaveFileSystem.Tests
{
    public class JsonSerializerTest
    {
        [Serializable]
        class TestSaveData
        {
            public string PlayerName;
            public int Level;

            public TestSaveData(string name, int level)
            {
                PlayerName = name;
                Level = level;
            }
        }

        JsonUtilitySerializer _jsonUtilitySerializer;

        [SetUp]
        public void SetUp()
        {
            _jsonUtilitySerializer = new JsonUtilitySerializer();
        }

        [Test]
        public void Serialize_ValidObject_ReturnJsonString()
        {
            var testSaveData = new TestSaveData("Hero", 6);
            string json = _jsonUtilitySerializer.Serialize<TestSaveData>(testSaveData);

            Assert.IsFalse(string.IsNullOrWhiteSpace(json));
            StringAssert.Contains("Hero", json);
            StringAssert.Contains("6", json);
        }

        [Test]
        public void Serialize_InValidObject_ReturnNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _jsonUtilitySerializer.Serialize<TestSaveData>(null);
            });
        }

        [Test]
        public void DeSerialize_ValidJsonString_ReturnObject()
        {
            string json = "{\"PlayerName\":\"Hero\",\"Level\":6}";

            var result = _jsonUtilitySerializer.Deserialize<TestSaveData>(json);

            Assert.IsNotNull(result);
            Assert.AreEqual("Hero", result.PlayerName);
            Assert.AreEqual(6, result.Level);
        }

        [Test]
        public void DeSerialize_EmptyOrWhitespaceString_ThrowsSaveSystemException()
        {
            string json = "     ";

            var exception = Assert.Throws<SaveSystemException>(() =>
            {
                _jsonUtilitySerializer.Deserialize<TestSaveData>(json);
            });

            Assert.AreEqual(SaveSystemErrorCode.DeserializationFailed, exception.ErrorCode);
        }

        [Test]
        public void Deserialize_MalformedJsonString_ThrowSaveSystemException()
        {
            string json = "{ InValid : json ]";

            var exception = Assert.Throws<SaveSystemException>(() =>
            {
                _jsonUtilitySerializer.Deserialize<TestSaveData>(json);
            });

            Assert.AreEqual(SaveSystemErrorCode.DeserializationFailed, exception.ErrorCode);
        }
    }
}
