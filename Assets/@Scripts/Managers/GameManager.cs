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

    //TODO: 나중에 동영상 밸런스 포인트 관련 데이터 추가
    public int TotalVidieoBalancePoints = 5;
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

// 현재 촬영중인 동영상 밸런스 데이터 클래스
[Serializable]
public class  VideoBalanceData
{
    public int Length;
    public int Trend;
    public int Laugh;
    public int Info;
    public int Memory;
    public int Emotion;
}

public class GameManager : Singleton<GameManager>
{
    private RecordingVideoData _recordingVideoData = new RecordingVideoData();
    [SerializeField]
    private VideoBalanceData _videoBalanceData = new VideoBalanceData();

    // 외부는 읽기만 가능
    public RecordingVideoData RecordingVideoData => _recordingVideoData;
    public VideoBalanceData VideoBalanceData => _videoBalanceData;

    public void UpdateVideoBalanceData(int length, int trend, int laugh, int info, int memory, int emotion)
    {
        _videoBalanceData.Length = length;
        _videoBalanceData.Trend = trend;
        _videoBalanceData.Laugh = laugh;
        _videoBalanceData.Info = info;
        _videoBalanceData.Memory = memory;
        _videoBalanceData.Emotion = emotion;
    }


    // 단일 필드 갱신 메서드들
    public void UpdateRecordingLocation(string location)
    {
        _recordingVideoData.Location = location;
    }

    public void UpdateRecordingCast(string cast)
    {
        _recordingVideoData.Cast = cast;
    }

    public void UpdateRecordingContent(string content)
    {
        _recordingVideoData.Content = content;
    }

    public void UpdateRecordingTitle(string title)
    {
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
