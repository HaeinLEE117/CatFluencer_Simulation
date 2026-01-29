using System;
using UnityEngine;
using static constants;

// 현재 촬영중인 동영상을 나타내는 데이터 클래스
[Serializable]
public class RecordingVideoData
{
    public int Location;
    public int Cast;
    public int Content;
    public string Title;
    public int castStat1;
    public int castStat2;
    public int castStat3;
    public float videoScore;
    public int recordingCost;

    public RecordingVideoData()
    {
        recordingCost = 0;

        int i = GameManager.Instance.GameData.UpdateVideoCount + 1;
        Title = LocalizationManager.Instance.GetLocalizedText("NEW_VIDEO") + " " + i.ToString();
    }
}

// 현재 촬영중인 동영상 밸런스 데이터 클래스
[Serializable]
public class VideoBalanceData
{
    public int Length;
    public int Trend;
    public int Laugh;
    public int Info;
    public int Memory;
    public int Emotion;
}

public enum VideoDataErrorTye
{
    None,
    NoLocation,
    NoCast,
    NoContent,
}

public class RecordingManager : Singleton<RecordingManager>
{
    // 현재 촬영 진행 여부
    public bool IsRecording { get; private set; }

    // 현재 촬영 데이터 (외부 읽기 전용으로 사용하길 권장)
    [field: SerializeField]
    public RecordingVideoData RecordingVideoData { get; private set; } = new RecordingVideoData();
    public VideoBalanceData VideoBalanceData { get; private set; } = new VideoBalanceData();


    private void OnEnable()
    {
        EventManager.Instance.AddEvent(Define.EEventType.RecordDatdChanged, CalculateRecordCost);
        EventManager.Instance.AddEvent(Define.EEventType.RecordingEnd, AutoUpdateVideoTitle);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveEvent(Define.EEventType.RecordDatdChanged, CalculateRecordCost);
        EventManager.Instance.RemoveEvent(Define.EEventType.RecordingEnd, AutoUpdateVideoTitle);
    }

    // 촬영 시작 
    public void StartRecording()
    {
        if(IsRecording)
            return;
        IsRecording = true;
        CalculdateRecordingVideoScroe();
        EventManager.Instance.TriggerEvent(Define.EEventType.RecordingStart);
    }

    public void CancelRecording()
    {
        IsRecording = false;
        EventManager.Instance.TriggerEvent(Define.EEventType.RecordingEnd);
    }

    public void FinishRecording()
    {
        // 결과 처리 / 보상 / 저장 등 필요 시 여기에 구현
        IsRecording = false;
        EventManager.Instance.TriggerEvent(Define.EEventType.RecordingEnd);
        //TODO: 촬영 완료 후 데이터 저장(플레이어 데이터 업데이트) 및 보상 처리
    }

    // 단일 필드 갱신
    public void UpdateRecordingLocation(int locationID)
    {
        RecordingVideoData.Location = locationID;
        EventManager.Instance.TriggerEvent(Define.EEventType.RecordDatdChanged);
    }

    public void UpdateRecordingCast(int cast)
    {
        RecordingVideoData.Cast = cast;
        EventManager.Instance.TriggerEvent(Define.EEventType.RecordDatdChanged);
    }

    public void UpdateRecordingContent(int contentID)
    {
        RecordingVideoData.Content = contentID;
        EventManager.Instance.TriggerEvent(Define.EEventType.RecordDatdChanged);
    }

    public void UpdateRecordingTitle(string title)
    {
        RecordingVideoData.Title = title;
        EventManager.Instance.TriggerEvent(Define.EEventType.RecordDatdChanged);
    }

    public void UpdateRecordingCastStat1(int stat)
    {
        RecordingVideoData.castStat1 = stat;
        EventManager.Instance.TriggerEvent(Define.EEventType.RecordDatdChanged);
    }

    public void UpdateRecordingCastStat2(int stat)
    {
        RecordingVideoData.castStat2 = stat;
        EventManager.Instance.TriggerEvent(Define.EEventType.RecordDatdChanged);
    }

    public void UpdateRecordingCastStat3(int stat)
    {
        RecordingVideoData.castStat3 = stat;
        EventManager.Instance.TriggerEvent(Define.EEventType.RecordDatdChanged);
    }


    private void CalculdateRecordingVideoScroe()
    {

        if (CheckVideoDataValidity() != VideoDataErrorTye.None)
            return;
        if (!IsRecording)
            return;

        var config = DataManager.Instance.VideoLocationsConfig;
        if (config == null)
        {
            Debug.LogError("VideoLocationsConfig not loaded.");
            return;
        }

        int castStatScore = Mathf.Clamp(RecordingVideoData.castStat1 / 10, MIN_CAST_STAT1_SCORE, MAX_CAST_STAT1_SCORE) +
             Mathf.Clamp(RecordingVideoData.castStat2 / 5, MIN_CAST_STAT2_SCORE, MAX_CAST_STAT2_SCORE) +
             Mathf.Clamp(RecordingVideoData.castStat3 / 5, MIN_CAST_STAT3_SCORE, MAX_CAST_STAT3_SCORE);
        Debug.Log($"Cst Score : {castStatScore}");

        bool isGoodCombo = CheckLocationContentCombo(config);
        if (isGoodCombo)
            RecordingVideoData.videoScore =castStatScore * constants.COMBO_BONUS_MULTIPLIER;
        else
            RecordingVideoData.videoScore = castStatScore;
        Debug.Log($"Is Combo? : {isGoodCombo}");

    }

    private bool CheckLocationContentCombo(VideoLocationsConfig config)
    {
        foreach (LocationGoodComboData combodata in config.GoodComboContents)
        {
            if (combodata.locationId == RecordingVideoData.Location)
            {
                foreach (int id in combodata.goodContentsIds)
                {
                    if (id == RecordingVideoData.Content)
                        return true;
                }
            }
        }

        return false;
    }


    // 밸런스 일괄 갱신
    public void UpdateVideoBalanceData(int length, int trend, int laugh, int info, int memory, int emotion)
    {
        VideoBalanceData.Length = length;
        VideoBalanceData.Trend = trend;
        VideoBalanceData.Laugh = laugh;
        VideoBalanceData.Info = info;
        VideoBalanceData.Memory = memory;
        VideoBalanceData.Emotion = emotion;
        EventManager.Instance.TriggerEvent(Define.EEventType.RecordDatdChanged);
    }

    public void CalculateRecordCost()
    {
        int castCost = 0;
        int locationCost = 0;

        DataManager.Instance.CastDict.TryGetValue(RecordingVideoData.Cast, out CastData castData);
        DataManager.Instance.LocationDict.TryGetValue(RecordingVideoData.Location, out LocationData locationData);
        if (castData == null)
            DataManager.Instance.EmployeeDict.TryGetValue(RecordingVideoData.Cast, out EmployeeData employeeData);
        else
            castCost = castData.CastPay;
        locationCost = locationData?.Coast ?? 0;

        RecordingVideoData.recordingCost = castCost + locationCost;
    }

    public void AutoUpdateVideoTitle()
    {
        int i = GameManager.Instance.GameData.UpdateVideoCount + 1;
        RecordingVideoData.Title = LocalizationManager.Instance.GetLocalizedText("NEW_VIDEO") + " " + i.ToString();
    }

    public VideoDataErrorTye CheckVideoDataValidity()
    {
        if (RecordingVideoData.Location == 0)
            return VideoDataErrorTye.NoLocation;
        if (RecordingVideoData.Cast == 0)
            return VideoDataErrorTye.NoCast;
        if (RecordingVideoData.Content == 0)
            return VideoDataErrorTye.NoContent;
        return VideoDataErrorTye.None;
    }

}
