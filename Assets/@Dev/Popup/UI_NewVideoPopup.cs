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
        { Buttons.RecordingDetail_TitleButton,    "UI_TitleEnterPopup" },
    };

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.StartButton).onClick.AddListener(ClosePopup);

        // Open mapped 3rd-level popup when detail buttons are pressed (reusable logic)
        foreach (var kv in _detailPopupMap)
        {
            int idx = (int)kv.Key;
            string popupName = kv.Value;
            GetButton(idx).onClick.AddListener(() => {
                UIManager.Instance.ShowPopupUI(popupName); 
            });
        }

        // Subscribe to selection event(s)
        EventManager.Instance.AddEvent(Define.EEventType.UI_LocationSelected, OnDetailSelected);
        EventManager.Instance.AddEvent(Define.EEventType.UI_CastSelected, OnDetailSelected);
        EventManager.Instance.AddEvent(Define.EEventType.UI_ContentSelected, OnDetailSelected);

    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEvent(Define.EEventType.UI_LocationSelected, OnDetailSelected);
        EventManager.Instance.RemoveEvent(Define.EEventType.UI_CastSelected, OnDetailSelected);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        GetText((int)Texts.SelectedLocationText).text = GameManager.Instance.RecordingVideoData.Location;
        GetText((int)Texts.SelectedCastText).text = GameManager.Instance.RecordingVideoData.Cast;
        GetText((int)Texts.SelectedContentText).text = GameManager.Instance.RecordingVideoData.Content;
        GetText((int)Texts.EnteredTitleText).text = GameManager.Instance.RecordingVideoData.Title;

    }

    private void OnDetailSelected()
    {
        RefreshUI();
    }

    private void ClosePopup()
    {
        UIManager.Instance.ClosePopupUI();
    }
}
