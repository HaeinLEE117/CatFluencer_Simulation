using UnityEngine;

public class UI_LiveStreamPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        StreamerPhoto,
    }

    enum Buttons
    {
        PreButton,
        NextButton,

        StartButton,
    }

    enum Texts
    {
        StreamerNameText,

        Stat1Text,
        Stat2Text,
        Stat3Text,

        Stat1PointText,
        Stat2PointText,
        Stat3PointText,

        StreamWeeksText,

        LiveStreamDescriptionText
    }

protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.StartButton).onClick.AddListener(ClosePopup);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }

    private void ClosePopup()
    {
        UIManager.Instance.ClosePopupUI();
    }
}
