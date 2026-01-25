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
        base.OnEnable();
        RefreshUI();
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
    }

    private void OnDetailSelected()
    {
        RefreshUI();
    }

    private void ClosePopup()
    {
        UIManager.Instance.ClosePopupUI();
    }

    private void OnStartButtonClicked()
    {
        if (GameManager.Instance.RecordingVideoData == null)
            return;

        //TODO: 임시 타이틀 설정, 플레이어 데이터에서 여태 업로드한 갯수 파악 후 "동영상 {n}" 형식으로 설정
        if (GameManager.Instance.RecordingVideoData.Title == null)
            GameManager.Instance.UpdateRecordingTitle("tmp");

        UIManager.Instance.ShowPopupUI(nameof(UI_DirectingBalancePopup));
    }
}
