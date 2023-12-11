using System;
using System.Collections.Generic;
using UnityEngine;
using File = System.IO.File;

public class HighScoreManager : MonoBehaviour
{
    public const int MaxRecordsNumber = 5;

    public static void SubmitPlayerScore(string playerName, int score)
    {
        var highScore = LoadHighScore();
        if (highScore.records.Count < MaxRecordsNumber)
        {
            highScore.records.Add(new HighScoreRecord { playerName = playerName, score = score });
            SaveHighScore(highScore);
        }
        else
        {
            var minHighScoreRecord = highScore.records[^1]; // Select from the last index
            if (score <= minHighScoreRecord.score) return;

            highScore.records.RemoveAt(highScore.records.Count - 1);
            highScore.records.Add(new HighScoreRecord { playerName = playerName, score = score });
            SaveHighScore(highScore);
        }
    }

    public static HighScore LoadHighScore()
    {
        var filePath = FilePath();
        return File.Exists(filePath)
            ? JsonUtility.FromJson<HighScore>(File.ReadAllText(filePath))
            : new HighScore
            {
                records = new List<HighScoreRecord>(MaxRecordsNumber)
            };
    }

    private static void SaveHighScore(HighScore highScore)
    {
        highScore.records.Sort();
        highScore.records.Reverse();
        File.WriteAllText(FilePath(), JsonUtility.ToJson(highScore));
    }

    private static string FilePath()
    {
        return $"{Application.persistentDataPath}/high-score.json";
    }

    [Serializable]
    public class HighScore
    {
        public List<HighScoreRecord> records;
    }

    [Serializable]
    public class HighScoreRecord : IComparable<HighScoreRecord>
    {
        public string playerName;
        public int score;

        public int CompareTo(HighScoreRecord other)
        {
            return score - other.score;
        }
    }
}