using UnityEngine;
using System.Collections.Generic;

public class UI_NewVideoPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {

    }

    enum Buttons
    {
        RecordingDetail_LocationButton,
        RecordingDetail_CastButton,
        RecordingDetail_ContentButton,
        RecordingDetail_TitleButton,

        StartButton,
    }

    enum Texts
    {
        RecordingCostText,

        LocationText,
        SelectedLocationText,

        CastText,
        SelectedCastText,

        ContentText,
        SelectedContentText,

        TitleText,
        EnteredTitleText,

        StartButtonText,
    }

    // Map detail buttons to 3rd-level popup prefab names
    private readonly Dictionary<Buttons, string> _detailPopupMap = new()
    {
        { Buttons.RecordingDetail_LocationButton, "UI_LocationPopup" },
        { Buttons.RecordingDetail_CastButton,     "UI_CastPopup" },
        { Buttons.RecordingDetail_ContentButton,  "UI_ContentPopup" },
        { Buttons.RecordingDetail_TitleButton,    "UI_TitleEnterPopup" }
    };

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        // Open mapped 3rd-level popup when detail buttons are pressed (reusable logic)
        foreach (var kv in _detailPopupMap)
        {
            int idx = (int)kv.Key;
            string popupName = kv.Value;
            GetButton(idx).onClick.AddListener(() => {
                UIManager.Instance.ShowPopupUI(popupName); 
            });
        }


        //TODO: Start Button에서 Directing팝업 뜰 때 각 데이터 널체크 하기
        GetButton((int)Buttons.StartButton).onClick.AddListener(OnStartButtonClicked);

    }

    protected override void OnEnable()
    {
        if(GameManager.Instance.IsRecording)
            UIManager.Instance.ShowConfirmPopup(LocalizationManager.Instance.GetLocalizedText("FAILED"), LocalizationManager.Instance.GetLocalizedText("RECORDING_ALREADY_PROGRESSING"), null,null, true);
        else
        {
            base.OnEnable();
            RefreshUI();
        }
    }

    private void OnDestroy()
    {
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        string locationTextId = DataManager.Instance.GetTextIDwithIntKey(GameManager.Instance.RecordingVideoData.Location);
        string contentTextId = DataManager.Instance.GetTextIDwithIntKey(GameManager.Instance.RecordingVideoData.Content);
        string castTextId = DataManager.Instance.GetTextIDwithIntKey(GameManager.Instance.RecordingVideoData.Cast);
        GetText((int)Texts.SelectedLocationText).SetLocalizedText(locationTextId);
        GetText((int)Texts.SelectedCastText).SetLocalizedText(castTextId);
        GetText((int)Texts.SelectedContentText).SetLocalizedText(contentTextId);
        GetText((int)Texts.RecordingCostText).SetTextwithFont(GameManager.Instance.RecordingVideoData.recordingCost.ToString() + " G");

        string title = GameManager.Instance.RecordingVideoData.Title;
        GetText((int)Texts.EnteredTitleText).SetTextwithFont(title);

    }

    private void OnStartButtonClicked()
    {
        VideoDataErrorTye errorType = GameManager.Instance.CheckVideoDataValidity();

        switch (errorType)
        {
            case VideoDataErrorTye.NoLocation:
                UIManager.Instance.ShowConfirmPopup(LocalizationManager.Instance.GetLocalizedText("FAILED"),LocalizationManager.Instance.GetLocalizedText("INVALID_LOCATION"),null);
                break;
            case VideoDataErrorTye.NoCast:
                UIManager.Instance.ShowConfirmPopup(LocalizationManager.Instance.GetLocalizedText("FAILED"),LocalizationManager.Instance.GetLocalizedText("INVALID_CAST"),null);
                break;
            case VideoDataErrorTye.NoContent:
                UIManager.Instance.ShowConfirmPopup(LocalizationManager.Instance.GetLocalizedText("FAILED"),LocalizationManager.Instance.GetLocalizedText("INVALID_CONTENT"),null);
                break;
            case VideoDataErrorTye.None:
                UIManager.Instance.ShowPopupUI(nameof(UI_DirectingBalancePopup));
                break;
        }

    }
}
