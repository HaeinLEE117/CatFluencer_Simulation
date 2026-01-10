using System;
using UnityEngine;

public class RecordingManager : Singleton<RecordingManager>
{
    // 현재 촬영 진행 여부
    public bool IsRecording { get; private set; }

    // 현재 촬영 데이터 (외부 읽기 전용으로 사용하길 권장)
    public RecordingVideoData RecordingVideoData { get; private set; } = new RecordingVideoData();
    public VideoBalanceData VideoBalanceData { get; private set; } = new VideoBalanceData();


    //TODO: 이벤트 매니저로 이전
    public event Action OnRecordingStarted;
    public event Action OnRecordingUpdated;
    public event Action OnRecordingEnded;

    // 촬영 시작 (선택적으로 초기 데이터 전달)
    public void StartRecording(RecordingVideoData initial = null)
    {
        if (initial != null)
            RecordingVideoData = initial;
        else
            RecordingVideoData = new RecordingVideoData();

        VideoBalanceData = new VideoBalanceData();
        IsRecording = true;
        OnRecordingStarted?.Invoke();
    }

    public void CancelRecording()
    {
        IsRecording = false;
        OnRecordingEnded?.Invoke();
    }

    public void FinishRecording()
    {
        // 결과 처리 / 보상 / 저장 등 필요 시 여기에 구현
        IsRecording = false;
        OnRecordingEnded?.Invoke();
    }

    // 단일 필드 갱신
    public void UpdateRecordingLocation(string location)
    {
        RecordingVideoData.Location = location;
        OnRecordingUpdated?.Invoke();
    }

    public void UpdateRecordingCast(string cast)
    {
        RecordingVideoData.Cast = cast;
        OnRecordingUpdated?.Invoke();
    }

    public void UpdateRecordingContent(string content)
    {
        RecordingVideoData.Content = content;
        OnRecordingUpdated?.Invoke();
    }

    public void UpdateRecordingTitle(string title)
    {
        RecordingVideoData.Title = title;
        OnRecordingUpdated?.Invoke();
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
        OnRecordingUpdated?.Invoke();
    }
}
