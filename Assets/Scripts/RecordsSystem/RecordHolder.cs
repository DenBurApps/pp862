using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace RecordSystem
{
    public static class RecordHolder
    {
        private static readonly string _savePath = Path.Combine(Application.persistentDataPath, "SaveData");
        private static Dictionary<GameType, RecordData> _records = new();

        public static IReadOnlyDictionary<GameType, RecordData> Records => _records;

        static RecordHolder()
        {
            foreach (GameType gameType in Enum.GetValues(typeof(GameType)))
            {
                _records[gameType] = new RecordData(0, 0, gameType);
            }

            LoadData();
        }

        public static void AddNewRecord(GameType gameType, int value, float timer)
        {
            if (_records.TryGetValue(gameType, out var record) && record.TotalScore < value)
            {
                record.TotalScore += value;
                record.TotalTime += timer;
                SaveData();
            }
        }

        public static RecordData GetRecordByType(GameType gameType)
        {
            return _records.GetValueOrDefault(gameType);
        }

        public static int GetTotalScores()
        {
            return _records.Values.Sum(record => record.TotalScore);
        }
        
        private static void SaveData()
        {
            try
            {
                var wrapper = new RecordDataWrapper(new List<RecordData>(_records.Values));
                var json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
                File.WriteAllText(_savePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data: {e}");
                throw;
            }
        }

        private static void LoadData()
        {
            if (!File.Exists(_savePath))
                return;

            try
            {
                var json = File.ReadAllText(_savePath);
                var wrapper = JsonConvert.DeserializeObject<RecordDataWrapper>(json);

                foreach (var record in wrapper.RecordDatas)
                {
                    if (Enum.IsDefined(typeof(GameType), record.GameType) && _records.ContainsKey(record.GameType))
                    {
                        _records[record.GameType] = record;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data: {e}");
            }
        }

        [Serializable]
        private class RecordDataWrapper
        {
            public List<RecordData> RecordDatas;

            public RecordDataWrapper(List<RecordData> records)
            {
                RecordDatas = records;
            }
        }

        [Serializable]
        public class RecordData
        {
            public int TotalScore;
            public float TotalTime;
            public GameType GameType;

            public RecordData(int totalScore, float bestTime, GameType gameType)
            {
                TotalScore = totalScore;
                GameType = gameType;
                TotalTime = bestTime;
            }
        }
    }

    public enum GameType
    {
        Classic,
        Piano,
        Sequence,
        Pole
    }
}