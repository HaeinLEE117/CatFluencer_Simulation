using UnityEngine;

public class UI_BottomPanel : UI_UGUI
{
    enum GameObjects
    {
    }

    enum Buttons
    {
        MenuButton,
    }

    enum Texts
    {
        MenuButtonText,

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
