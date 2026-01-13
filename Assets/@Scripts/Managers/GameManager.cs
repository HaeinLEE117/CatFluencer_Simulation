using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class GameData
{
    public int Gold;
    public List<int> UpgradeCount;

    // 구독자/채널 설정
    public int Subscriber;
    public string ChannelName;

    public int StartYear;
    public int StartMonth;
    public int StartWeek;

    // 동영상 밸런스 포인트
    public int TotalVidieoBalancePoints;

    // 시작 직원(초기 고용 직원 ID)
    public List<int> HiredEmployeeIds;

    // 주당 초(게임 시간 설정)
    public float SecondsPerWeek;
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
    public int Subscribers
    {
        get { return _gameData.Subscriber; }
        set
        {
            _gameData.Subscriber = value;
            EventManager.Instance.TriggerEvent(Define.EEventType.SubscriberChanged);
        }
    }

    // GameData proxy properties
    public string ChannelName
    {
        get { return _gameData.ChannelName; }
        set { _gameData.ChannelName = value; EventManager.Instance.TriggerEvent(Define.EEventType.ChannelNameChanged); }
    }

    public int NowYear => _gameData.StartYear;
    public int NowMonth => _gameData.StartMonth;
    public int NowWeek => _gameData.StartWeek;

    public int TotalVidieoBalancePoints
    {
        get { return _gameData.TotalVidieoBalancePoints; }
        set { _gameData.TotalVidieoBalancePoints = value; EventManager.Instance.TriggerEvent(Define.EEventType.VideoBalancePointsChanged); }
    }

    public float SecondsPerWeek
    {
        get { return _gameData.SecondsPerWeek; }
        set { _gameData.SecondsPerWeek = value; EventManager.Instance.TriggerEvent(Define.EEventType.SecondsPerWeekChanged); }
    }

    public List<int> UpgradeCount
    {
        get { return _gameData.UpgradeCount; }
        set { _gameData.UpgradeCount = value; EventManager.Instance.TriggerEvent(Define.EEventType.UpgradeCountChanged); }
    }

    // 초기 고용 직원 목록은 add 형태로만 갱신
    public List<int> HiredEmployeeIds => _gameData.HiredEmployeeIds;

    public void AddInitHiredEmployeeId(int id)
    {
        if (_gameData.HiredEmployeeIds == null)
            _gameData.HiredEmployeeIds = new List<int>();

        if (_gameData.HiredEmployeeIds.Contains(id))
            return;

        _gameData.HiredEmployeeIds.Add(id);
        EventManager.Instance.TriggerEvent(Define.EEventType.InitHiredEmployeesChanged);
    }


    #region Recording Video
    // Recording 상태를 중앙에서 확인할 수 있도록 브리지 제공
    public bool IsRecording => RecordingManager.Instance != null && RecordingManager.Instance.IsRecording;

    // 현재 촬영 정보는 계속 노출 (RecordingManager로 위임)
    public RecordingVideoData RecordingVideoData => RecordingManager.Instance != null ? RecordingManager.Instance.RecordingVideoData : null;
    public VideoBalanceData VideoBalanceData => RecordingManager.Instance != null ? RecordingManager.Instance.VideoBalanceData : null;

    // 기존 호출 호환을 위한 포워딩 메서드들
    public void UpdateVideoBalanceData(int length, int trend, int laugh, int info, int memory, int emotion)
    {
        RecordingManager.Instance.UpdateVideoBalanceData(length, trend, laugh, info, memory, emotion);
    }

    public void UpdateRecordingLocation(string location)
    {
        RecordingManager.Instance.UpdateRecordingLocation(location);
        EventManager.Instance.TriggerEvent(Define.EEventType.UI_LocationSelected);
    }

    public void UpdateRecordingCast(string cast)
    {
        RecordingManager.Instance.UpdateRecordingCast(cast);
        EventManager.Instance.TriggerEvent(Define.EEventType.UI_CastSelected);
    }

    public void UpdateRecordingContent(string content)
    {
        RecordingManager.Instance.UpdateRecordingContent(content);
        EventManager.Instance.TriggerEvent(Define.EEventType.UI_ContentSelected);
    }

    public void UpdateRecordingTitle(string title)
    {
        RecordingManager.Instance.UpdateRecordingTitle(title);
        EventManager.Instance.TriggerEvent(Define.EEventType.UI_CastSelected);
    }

    // 옵션: 촬영 수명주기 제어 포워딩 (필요 시 사용)
    public void StartRecording()
    {
        RecordingManager.Instance.StartRecording();
    }

    public void CancelRecording()
    {
        RecordingManager.Instance.CancelRecording();
    }

    public void FinishRecording()
    {
        RecordingManager.Instance.FinishRecording();
    }

    #endregion


    
}
