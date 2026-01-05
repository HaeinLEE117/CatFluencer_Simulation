using UnityEngine;

public class UI_LiveStreamPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {

    }

    enum Buttons
    {
        StartButton,
    }

    enum Texts
    {

    }

protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.StartButton).onClick.AddListener(ClosePopup);
    }

    private void ClosePopup()
    {
        UIManager.Instance.ClosePopupUI();
    }
}
