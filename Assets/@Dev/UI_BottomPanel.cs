using System;
using TMPro;
using UnityEngine;
using static Define;

public class UI_BottomPanel : UI_UGUI
{
    enum GameObjects
    {
    }

    enum Buttons
    {
    }

    enum Texts
    {
        RecordingStatusText,
        TitleText,
        RemainText,
    }
    TMPro.TMP_Text _recordingStatusText;
    TMPro.TMP_Text _titleText;
    TMPro.TMP_Text _remainText;

    protected override void Awake()
    {
        base.Awake();


        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        _recordingStatusText = GetText((int)Texts.RecordingStatusText);
        _titleText = GetText((int)Texts.TitleText);
        _remainText = GetText((int)Texts.RemainText);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.Instance.AddEvent(Define.EEventType.RecordingStart, RefreshUI);
        EventManager.Instance.AddEvent(Define.EEventType.WeekAdvanced, RefreshUI);
        EventManager.Instance.AddEvent(Define.EEventType.RecordingEnd, RefreshUI);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (EventManager.Instance != null)
        {
            EventManager.Instance.RemoveEvent(Define.EEventType.RecordingStart, RefreshUI);
            EventManager.Instance.RemoveEvent(Define.EEventType.WeekAdvanced, RefreshUI);
            EventManager.Instance.RemoveEvent(Define.EEventType.RecordingEnd, RefreshUI);
        }
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        UpdateRecordingStatus();
    }

    private void UpdateRecordingStatus()
    {
        if (RecordingManager.Instance.IsRecording)
        {
            _recordingStatusText.SetLocalizedText("RECORDING_ALREADY_PROGRESSING");

            var recordingData = RecordingManager.Instance.RecordingVideoData;

            var remainTimeText = LocalizationManager.Instance.GetLocalizedText("REMAIN_WEEKS");

            _remainText.SetTextwithFont(remainTimeText + recordingData.remainRecordingWeeks);
            _titleText.SetTextwithFont(recordingData.Title);
        }
        else
        {
            _recordingStatusText.SetLocalizedText("NO_RECORDING");
            _titleText.SetTextwithFont(string.Empty);
            _remainText.SetTextwithFont(string.Empty);
        }
    }


}
