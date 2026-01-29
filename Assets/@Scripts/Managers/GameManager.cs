using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class GameData
{
    public int PlayerLevel;
    public int Gold;

    // 구독자/채널 설정
    public int Subscriber;
    public string ChannelName;

    public int Year;
    public int Month;
    public int Week;

    public int UpdateVideoCount;

    // 동영상 밸런스 포인트
    public int TotalVidieoBalancePoints;

    // 고용된 직원 목록 (ID 기준)
    public Dictionary<int, EmployeeData> HiredEmployees;
}

public class GameManager : Singleton<GameManager>
{
    List<EmployeeData> _hireCandidates = new List<EmployeeData>();
    public List<EmployeeData> HireCandidates
    {
        get { return _hireCandidates; }
        set { 
            _hireCandidates = value; 
            EventManager.Instance.TriggerEvent(Define.EEventType.HireableEmployeesChanged);
        }
    }
    [SerializeField]
    private GameData _gameData = new GameData();

    # region GameData proxy properties
    public GameData GameData
    {
        get { return _gameData; }
        set
        {
            _gameData = value;
            EventManager.Instance.TriggerEvent(Define.EEventType.GameDataChanged);
            UpdatePlayerLevelDatas();
        }
    }

    public int PlayerLevel
    {
        get => _gameData.PlayerLevel;
        set
        {
            if (_gameData.PlayerLevel == value)
                return;
            _gameData.PlayerLevel = value;
            UpdatePlayerLevelDatas();
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
    public int Subscriber
    {
        get { return _gameData.Subscriber; }
        set
        {
            _gameData.Subscriber = value;
            EventManager.Instance.TriggerEvent(Define.EEventType.SubscriberChanged);
        }
    }    
    public string ChannelName
    {
        get { return _gameData.ChannelName; }
        set { _gameData.ChannelName = value; }
    }
    public int Year
    {
        get { return _gameData.Year; }
        set { _gameData.Year = value; }
    }
    public int Month
    {
        get { return _gameData.Month; }
        set { _gameData.Month = value; }
    }
    public int Week
    {
        get { return _gameData.Week; }
        set { _gameData.Week = value; }
    }
    public int TotalVidieoBalancePoints
    {
        get { return _gameData.TotalVidieoBalancePoints; }
        set { _gameData.TotalVidieoBalancePoints = value; EventManager.Instance.TriggerEvent(Define.EEventType.VideoBalancePointsChanged); }
    }

    #endregion

    private void OnEnable()
    {
        // Subscribe to hired employees initialization/changes to refresh level-based data
        EventManager.Instance.AddEvent(Define.EEventType.HiredEmployeesChanged, UpdatePlayerLevelDatas);
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent leaks or calls on destroyed object
        EventManager.Instance.RemoveEvent(Define.EEventType.HiredEmployeesChanged, UpdatePlayerLevelDatas);
    }

    #region 레벨별 상한 데이터
    public Dictionary<int, EmployeeData> _availableEmployees {get;private set;} = new Dictionary<int, EmployeeData>();
    public Dictionary<int, ContentsData> _availableContents { get; private set; } = new Dictionary<int, ContentsData>();
    public Dictionary<int, LocationData> _availableLocations { get; private set; } = new Dictionary<int, LocationData>();
    public Dictionary<int, CastData> _availableCasts { get; private set; } = new Dictionary<int, CastData>();

    public Dictionary<int, CastData> AvailableCasts
    {
        get { return _availableCasts; }
        set { _availableCasts = value; }
    }

    public Dictionary<int, EmployeeData> HireableEmployees
    {
        get { return _availableEmployees; }
        set
        {
            _availableEmployees = value;
        }
    }
    public Dictionary<int, ContentsData> AvailableContents
    {
        get { return _availableContents; }
        set
        {
            _availableContents = value;
        }
    }
    public Dictionary<int, LocationData> AvailableLocations
    {
        get { return _availableLocations; }
        set { _availableLocations = value; }
    }

    private void UpdatePlayerLevelDatas()
    {
        int employeeLevelCap = PlayerLevel * constants.BASEEMPLOYEESFORLEVEL;
        int contentLevelCap = PlayerLevel * constants.BASECONTENTFORLEVEL;
        int locationLevelCap = PlayerLevel * constants.BASELOCATIONSFORLEVEL;
        int castLevelCap = PlayerLevel * constants.BASECASTFORLEVEL;

        // 직원, 콘텐츠, 위치 데이터 업데이트
        // DataManager에서 각 Dict의 앞부분을 레벨 캡만큼 가져와 세팅
        HireableEmployees.Clear();
        _availableContents.Clear();
        _availableLocations.Clear();
        _availableCasts.Clear();

        var empDict = DataManager.Instance?.EmployeeDict;
        var contDict = DataManager.Instance?.ContentsDict;
        var locDict = DataManager.Instance?.LocationDict;
        var castDict = DataManager.Instance?.CastDict;

        int count = 0;
        foreach (var emp in empDict)
        {
            int empID = emp.Key;
            EmployeeData empData = emp.Value;
            HireableEmployees.Add(empID, empData);
            count++;
            if (count >= (PlayerLevel * constants.BASEEMPLOYEESFORLEVEL))
                break;
        }

        count = 0;
        foreach (var kv in contDict)
        {
            _availableContents[kv.Key] = kv.Value;
            count++;
            if (count >= contentLevelCap) break;
        }

        count = 0;
        foreach (var kv in locDict)
        {
            _availableLocations[kv.Key] = kv.Value;
            count++;
            if (count >= locationLevelCap) break;
        }
        EmployeeData bestEmployee = GetLargestStat2Employee();
        CastData employeeCastData = new CastData
        {
            TemplateID = bestEmployee.TemplateID,
            NameTextID = bestEmployee.NameTextID,
            PhotoImageID  =bestEmployee.PhotoImageID,
            Stat1 = bestEmployee.Stat1,
            Stat2 = bestEmployee.Stat2,
            Stat3 = bestEmployee.Stat3,
            CastPay = 0
        };
        _availableCasts[0] = employeeCastData;

        count = 1;
        foreach (var kv in castDict)
        {
            _availableCasts[kv.Key] = kv.Value;
            count++;
            if (count >= castLevelCap) break;
        }
    }

    #endregion

    #region 날짜 흐름 처리
    private Coroutine _timeCoroutine;

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
        _gameData.Week++;
        EventManager.Instance.TriggerEvent(Define.EEventType.WeekAdvanced);

        if (_gameData.Week > constants.WEEKSPERMONTH)
        {
            _gameData.Week = 1;
            _gameData.Month++;
            EventManager.Instance.TriggerEvent(Define.EEventType.MonthAdvanced);

            if (_gameData.Month > constants.MONTHSPERYEAR)
            {
                _gameData.Month = 1;
                _gameData.Year++;
                EventManager.Instance.TriggerEvent(Define.EEventType.YearAdvanced);
            }
        }
    }
    #endregion
    // Centralized updater to apply a new GameData and raise relevant events
    public void ApplyGameData(GameData data)
    {
        if (data == null) return;
        _gameData = data;

        EventManager.Instance.TriggerEvent(Define.EEventType.GoldChanged);
        EventManager.Instance.TriggerEvent(Define.EEventType.SubscriberChanged);
        EventManager.Instance.TriggerEvent(Define.EEventType.VideoBalancePointsChanged);
        EventManager.Instance.TriggerEvent(Define.EEventType.SecondsPerWeekChanged);
        EventManager.Instance.TriggerEvent(Define.EEventType.HiredEmployeesChanged);
        EventManager.Instance.TriggerEvent(Define.EEventType.UpgradeCountChanged);

        // 재시작: 설정 변경 시 타이머 재시작 고려
        if (_timeCoroutine != null)
        {
            StopTime();
            StartTime();
        }
    }

    #region 데이터 유틸 함수
    public bool TryPayGold(int amount)
    {
        if (_gameData.Gold >= amount)
        {
            Gold -= amount;
            return true;
        }
        return false;
    }

    public EmployeeData GetLargestStat2Employee()
    {
        EmployeeData bestEmployee = null;
        foreach (var emp in HiredEmployees.Values)
        {
            if (bestEmployee == null || emp.Stat2 > bestEmployee.Stat2)
            {
                bestEmployee = emp;
            }
        }
        return bestEmployee;
    }

    #endregion

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

    // 현재 촬영 정보는 계속 노출
    public RecordingVideoData RecordingVideoData => RecordingManager.Instance != null ? RecordingManager.Instance.RecordingVideoData : null;
    public VideoBalanceData VideoBalanceData => RecordingManager.Instance != null ? RecordingManager.Instance.VideoBalanceData : null;

    // 기존 호출 호환을 위한 포워딩 메서드들
    public void UpdateVideoBalanceData(int length, int trend, int laugh, int info, int memory, int emotion)
    {
        RecordingManager.Instance.UpdateVideoBalanceData(length, trend, laugh, info, memory, emotion);
    }

    public void UpdateRecordingLocation(int locationID)
    {
        RecordingManager.Instance.UpdateRecordingLocation(locationID);
    }

    public void UpdateRecordingCast(int cast)
    {
        RecordingManager.Instance.UpdateRecordingCast(cast);
    }

    public void UpdateRecordingCastStat1(int stat)
    {
        RecordingManager.Instance.UpdateRecordingCastStat1(stat);
    }
    public void UpdateRecordingCastStat2(int stat)
    {
        RecordingManager.Instance.UpdateRecordingCastStat2(stat);
    }
    public void UpdateRecordingCastStat3(int stat)
    {
        RecordingManager.Instance.UpdateRecordingCastStat3(stat);
    }

    public void UpdateRecordingContent(int contentID)
    {
        RecordingManager.Instance.UpdateRecordingContent(contentID);
    }

    public void UpdateRecordingTitle(string title)
    {
        RecordingManager.Instance.UpdateRecordingTitle(title);
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

    public VideoDataErrorTye CheckVideoDataValidity()
    {
        return RecordingManager.Instance.CheckVideoDataValidity();
    }

   public bool IsVideoRecording()
    {
        return RecordingManager.Instance.IsRecording;
    }

    #endregion
}
