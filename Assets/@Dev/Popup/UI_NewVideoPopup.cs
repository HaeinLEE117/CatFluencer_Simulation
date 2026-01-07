using UnityEngine;

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

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.StartButton).onClick.AddListener(ClosePopup);

        // Open Location popup when location button is pressed.
        GetButton((int)Buttons.RecordingDetail_LocationButton).onClick.AddListener(OpenLocationPopup);

        // Subscribe to selection event
        EventManager.Instance.AddEvent(Define.EEventType.UI_LocationSelected, OnLocationSelected);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEvent(Define.EEventType.UI_LocationSelected, OnLocationSelected);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        GetText((int)Texts.SelectedLocationText).text = GameManager.Instance.RecordingVideoData.Location;
        GetText((int)Texts.SelectedCastText).text = GameManager.Instance.RecordingVideoData.Cast;
        GetText((int)Texts.SelectedContentText).text = GameManager.Instance.RecordingVideoData.Content;
        GetText((int)Texts.EnteredTitleText).text = GameManager.Instance.RecordingVideoData.Title;

    }

    private void OnLocationSelected()
    {
        RefreshUI();
    }

    private void ClosePopup()
    {
        UIManager.Instance.ClosePopupUI();
    }

    private void OpenLocationPopup()
    {
        UIManager.Instance.ShowPopupUI("UI_LocationPopup");
    }
}
