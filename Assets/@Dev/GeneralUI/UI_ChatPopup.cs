using UnityEngine;

public class UI_ChatPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
    }

    enum Buttons
    {
        CancleButton,
        ConfirmButton,
    }

    enum Texts
    {
        ConfirmPopupText,
        DescribtionText,

        CancleButtonText,
        ConfirmButtonText,
    }

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }


}
