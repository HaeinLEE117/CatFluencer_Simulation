using System;
using UnityEngine;

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
    public int videoScore;
    public int recordingCost;
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

public class RecordingManager : Singleton<RecordingManager>
{
    // 현재 촬영 진행 여부
    public bool IsRecording { get; private set; }

    // 현재 촬영 데이터 (외부 읽기 전용으로 사용하길 권장)
    public RecordingVideoData RecordingVideoData { get; private set; } = new RecordingVideoData();
    public VideoBalanceData VideoBalanceData { get; private set; } = new VideoBalanceData();


    private void OnEnable()
    {
        EventManager.Instance.AddEvent(Define.EEventType.RecordDatdChanged, CalculateRecordCost);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveEvent(Define.EEventType.RecordDatdChanged, CalculateRecordCost);
    }

    // 촬영 시작 
    public void StartRecording()
    {
        IsRecording = true;
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


    private void CalculdateRecordingVideoScroe()
    {
        RecordingVideoData.videoScore = 0;

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
        Debug.Log($"Recording Cost Calculated: CastCost={castCost}, LocationCost={locationCost}, Total={RecordingVideoData.recordingCost}");

    }

}
