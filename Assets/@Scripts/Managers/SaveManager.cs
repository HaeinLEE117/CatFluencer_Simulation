using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private const string SAVE_FILE_NAME = "GameData.json";
    public static string SavePath => Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

    private const float AUTO_SAVE_INTERVAL = 10f;
    private Coroutine _coAutoSave;

    #region AutoSave
    public void StartAutoSave()
    {
        StopAutoSave();

        if (_coAutoSave == null)
        {
            _coAutoSave = StartCoroutine(CoAutoSave());
            Debug.Log($"SaveManager: Auto-save started (interval: {AUTO_SAVE_INTERVAL}s)");
        }
    }

    public void StopAutoSave()
    {
        if (_coAutoSave != null)
        {
            StopCoroutine(_coAutoSave);
            _coAutoSave = null;
            Debug.Log("SaveManager: Auto-save stopped");
        }
    }

    private IEnumerator CoAutoSave()
    {
        WaitForSeconds wait = new WaitForSeconds(AUTO_SAVE_INTERVAL);

        while (true)
        {
            Save();
            yield return wait;
        }
    }
    #endregion

    public void Save()
    {
        GameData gameData = GameManager.Instance.GameData;
        if (gameData == null)
        {
            Debug.Log("SaveManager: GameData is null, cannot save.");
            return;
        }

        string json = JsonConvert.SerializeObject(gameData);
        File.WriteAllText(SavePath, json);
        Debug.Log($"SaveManager: Game saved to {SavePath}");
    }
    //TODO: LoadingScene에서 부르기
    public void Load()
    {
        if (File.Exists(SavePath) == false)
        {
            Debug.Log("SaveManager: No save file found. Starting with default data.");
            Reset();
            return;
        }

        string json = File.ReadAllText(SavePath);
        GameManager.Instance.GameData = JsonConvert.DeserializeObject<GameData>(json);
        Debug.Log($"SaveManager: Game loaded from {SavePath}");
    }

    public void Reset()
    {
        //TODO: 채널명 입력 받기
        GameData gameData = new GameData()
        {
            PlayerLevel = DataManager.Instance.GameConfig.InitialPlayerLevel,
            Gold = DataManager.Instance.GameConfig.InitialGold,
            Subscriber = DataManager.Instance.GameConfig.InitialSubscriber,
            ChannelName = DataManager.Instance.GameConfig.InitialChannelName,
            Year = DataManager.Instance.GameConfig.InitailYear,
            Month = DataManager.Instance.GameConfig.InitailMonth,
            Week = DataManager.Instance.GameConfig.InitailWeek,
            TotalVidieoBalancePoints = DataManager.Instance.GameConfig.InitialVideoBalancePoints,
            HiredEmployees = new Dictionary<int, EmployeeData>(),
        };

        int empID = DataManager.Instance.GameConfig.InitialHiredEmployee;
        EmployeeData employeeData = DataManager.Instance.EmployeeDict[empID];
        gameData.HiredEmployees.Add(employeeData.TemplateID, employeeData);

        GameManager.Instance.GameData = gameData;
        Save();
    }

    public void Delete()
    {
        if (File.Exists(SavePath) == false)
        {
            Debug.LogWarning("SaveManager: No save file to delete.");
            return;
        }

        File.Delete(SavePath);
        Debug.Log("SaveManager: Save file deleted.");
    }
}
