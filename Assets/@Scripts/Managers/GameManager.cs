using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class GameData
{
    public int Gold;
    public int Level;
    public List<int> UpgradeCount;
}

// 현재 촬영중인 동영상을 나타내는 데이터 클래스
[Serializable]
public class RecordingVideoData
{
    //TODO: string => ID로 변경
    public string Location;
    public string Cast;
    public string Content;
    public string Title;
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private RecordingVideoData _recordingVideoData = new RecordingVideoData();

    // 외부는 읽기만 가능
    public RecordingVideoData RecordingVideoData => _recordingVideoData;

    // 명시적 갱신 API
    public void SetRecordingVideoData(string location=null, string cast = null, string content = null, string title = null)
    {
        _recordingVideoData.Location = location;
        _recordingVideoData.Cast = cast;
        _recordingVideoData.Content = content;
        _recordingVideoData.Title = title;
    }

    private GameData _gameData = new GameData();
    public GameData GameData
    {
        get { return _gameData; }
        set
        {
            _gameData = value;
        }
    }

    public int Gold
    {
        get { return _gameData.Gold; }
        set
        {
            _gameData.Gold = value;
            EventManager.Instance.TriggerEvent(Define.EEventType.GoldChanged);
        }
    }
}
