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
    public void StartRecording(RecordingVideoData initial = null)
    {
        RecordingManager.Instance.StartRecording(initial);
    }

    public void CancelRecording()
    {
        RecordingManager.Instance.CancelRecording();
    }

    public void FinishRecording()
    {
        RecordingManager.Instance.FinishRecording();
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
