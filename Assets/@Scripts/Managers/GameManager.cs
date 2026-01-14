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

    // 고용된 직원 목록 (ID 기준)
    public Dictionary<int, EmployeeData> HiredEmployees;

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

    private Coroutine _timeCoroutine;

    // 현재 날짜(년/월/주)
    public int NowYear => _gameData.StartYear;
    public int NowMonth => _gameData.StartMonth;
    public int NowWeek => _gameData.StartWeek;

    // 시간 흐름 시작/중지
    public void StartTime()
    {
        if (_timeCoroutine != null) return;
        float secondsPerWeek = DataManager.Instance?.GameConfig?.GetSecondsPerWeek() ?? 10f;
        _timeCoroutine = StartCoroutine(CoWeekTick(secondsPerWeek));
    }

    public void StopTime()
    {
        if (_timeCoroutine == null) return;
        StopCoroutine(_timeCoroutine);
        _timeCoroutine = null;
    }

    private System.Collections.IEnumerator CoWeekTick(float secondsPerWeek)
    {
        var wait = new WaitForSeconds(secondsPerWeek);
        while (true)
        {
            yield return wait;
            AdvanceWeek();
        }
    }

    public void AdvanceWeek()
    {
        _gameData.StartWeek++;
        EventManager.Instance.TriggerEvent(Define.EEventType.WeekAdvanced);

        if (_gameData.StartWeek > constants.WEEKSPERMONTH)
        {
            _gameData.StartWeek = 1;
            _gameData.StartMonth++;
            EventManager.Instance.TriggerEvent(Define.EEventType.MonthAdvanced);

            if (_gameData.StartMonth > constants.MONTHSPERYEAR)
            {
                _gameData.StartMonth = 1;
                _gameData.StartYear++;
                EventManager.Instance.TriggerEvent(Define.EEventType.YearAdvanced);
            }
        }
    }

    // Centralized updater to apply a new GameData and raise relevant events
    public void ApplyGameData(GameData data)
    {
        if (data == null) return;
        _gameData = data;

        EventManager.Instance.TriggerEvent(Define.EEventType.GoldChanged);
        EventManager.Instance.TriggerEvent(Define.EEventType.SubscriberChanged);
        EventManager.Instance.TriggerEvent(Define.EEventType.ChannelNameChanged);
        EventManager.Instance.TriggerEvent(Define.EEventType.VideoBalancePointsChanged);
        EventManager.Instance.TriggerEvent(Define.EEventType.SecondsPerWeekChanged);
        EventManager.Instance.TriggerEvent(Define.EEventType.InitHiredEmployeesChanged);
        EventManager.Instance.TriggerEvent(Define.EEventType.UpgradeCountChanged);

        // 재시작: 설정 변경 시 타이머 재시작 고려
        if (_timeCoroutine != null)
        {
            StopTime();
            StartTime();
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
    public bool GoldDeduct(int amount)
    {
        if (_gameData.Gold >= amount)
        {
            Gold -= amount;
            return true;
        }
        return false;
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

    public int TotalVidieoBalancePoints
    {
        get { return _gameData.TotalVidieoBalancePoints; }
        set { _gameData.TotalVidieoBalancePoints = value; EventManager.Instance.TriggerEvent(Define.EEventType.VideoBalancePointsChanged); }
    }

    public List<int> UpgradeCount
    {
        get { return _gameData.UpgradeCount; }
        set { _gameData.UpgradeCount = value; EventManager.Instance.TriggerEvent(Define.EEventType.UpgradeCountChanged); }
    }

    // EmployeeManager forwarders
    public IReadOnlyDictionary<int, EmployeeData> HiredEmployees => EmployeeManager.Instance.HiredEmployees;
    public bool HireEmployee(int employeeId) => EmployeeManager.Instance.Hire(employeeId);
    public bool FireEmployee(int employeeId) => EmployeeManager.Instance.Fire(employeeId);
    public bool ApplyEmployeeTraining(int employeeId, int stat1Delta = 0, int stat2Delta = 0, int stat3Delta = 0)
        => EmployeeManager.Instance.ApplyTraining(employeeId, stat1Delta, stat2Delta, stat3Delta);
    public int GetEmployeeTrainStat1Cost(int employeeId)
        => EmployeeManager.Instance.GetEmployeeTrainStat1Coast(employeeId);
    public int GetEmployeeTrainStat2Cost(int employeeId)
        => EmployeeManager.Instance.GetEmployeeTrainStat2Coast(employeeId);
    public int GetTraindeltaPointStat1(int employeeId)
        => EmployeeManager.Instance.GetTrainDeltaPointsStat1(employeeId);
    public int GetTraindeltaPointStat2(int employeeId)
        => EmployeeManager.Instance.GetTrainDeltaPointsStat2(employeeId);


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
