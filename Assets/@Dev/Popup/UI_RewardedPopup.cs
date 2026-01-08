using UnityEngine;

public class UI_RewardedPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        RewardedImage,
    }

    enum Buttons
    {
        OfficeMovingButton,
    }

    enum Texts
    {
        RewardedPopupText,
        DescribtionText,
        OfficeMovingButtonText,
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
