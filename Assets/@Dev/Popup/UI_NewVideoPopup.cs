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

    private string _location;

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

    private void OpenLocationPopup()
    {
        UIManager.Instance.ShowPopupUI("UI_LocationPopup");
    }

    private void OnLocationSelected()
    {
        // Find the location popup and get selected location
        var popup = FindFirstObjectByType<UI_LocationPopup>();
        if (popup != null)
        {
            _location = popup.GetSelected();
            var txt = GetText((int)Texts.SelectedLocationText);
            if (txt != null) txt.text = _location;
        }
    }

    private void ClosePopup()
    {
        UIManager.Instance.ClosePopupUI();
    }
}
